using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetfairApi;
using BetfairApi.TO;
using BetfairBookmaker.Contracts;
using BetfairBookmaker.Models;
using BookerMagikCore.Bookmaker;
using EntityLibrary.Bookmaker;
using EntityLibrary.Bookmaker.Sport;
using Newtonsoft.Json;

namespace BetfairBookmaker
{
    public class BetfairBookmakerClass : BookmakerAbstract, IBetfairBookmaker
    {
        private IClient api;
        private ConfigurationModel configuration;
        private string token;

        protected override string ReadLineThreadName => "Betfair read line thread";

        protected override object ReadLineThreadParameter => new ReadEventsCompetitionModel
        {
            Competition = configuration.Leagues,
            EventTimeRange = new TimeRange {From = DateTime.Now, To = DateTime.Now.AddDays(configuration.AddDays)}
        };

        protected override TimeSpan WaitStopReadThreadTimeout => TimeSpan.FromSeconds(30);

        public override string Name => "Betfair";

        public override async Task<bool> Login(string jsonConfiguration)
        {
            configuration = JsonConvert.DeserializeObject<ConfigurationModel>(jsonConfiguration);
            token = RescriptClient.GetToken(configuration.CertificatePath, configuration.CertificatePassword,
                configuration.ApplicationKey, configuration.Username, configuration.Password);

            if (string.IsNullOrWhiteSpace(token))
                return false;

            api = new RescriptClient(configuration.ApiEndpoint, configuration.ApplicationKey, token);

            return await Task.FromResult(true);
        }

        public override async Task<IEnumerable<BookmakerTwoParticipantEvent>> ReadEvents()
        {
            var time = new TimeRange {From = DateTime.Now, To = DateTime.Now.AddDays(configuration.AddDays)};
            var marketFilter = new MarketFilter
            {
                EventTypeIds = new HashSet<string>(new[] {configuration.Sports}),
                CompetitionIds = new HashSet<string>(new[] {configuration.Leagues}),
                MarketCountries = new HashSet<string>(new[] {configuration.Countries}),
                MarketStartTime = time
            };

            //as an example we requested runner metadata 
            ISet<MarketProjection> marketProjections = new HashSet<MarketProjection>();
            marketProjections.Add(MarketProjection.RUNNER_METADATA);
            var listEvents = await api.listEvents(marketFilter);

            var bookmakerTwoParticipantSportEvents = new List<BookmakerTwoParticipantEvent>();

            foreach (var sportEvent in listEvents.Where(x => x.Event.OpenDate.HasValue))
            {
                var name = sportEvent.Event.Name;
                if (!name.Contains(" v "))
                    continue;

                var teams = name.Split(" v ");
                var eventTime = sportEvent.Event.OpenDate.Value;
                var bookmakerTwoParticipantSportEvent =
                    new BookmakerTwoParticipantEvent(eventTime, new BookmakerEventParticipant(teams[0].Trim()),
                        new BookmakerEventParticipant(teams[1].Trim()));

                bookmakerTwoParticipantSportEvents.Add(bookmakerTwoParticipantSportEvent);
            }

            return bookmakerTwoParticipantSportEvents;
        }

        public override async Task<IEnumerable<SportLeague>> ReadLeagues()
        {
            var marketFilter = new MarketFilter
            {
                EventTypeIds = new HashSet<string>(new[] {configuration.Sports}),
                MarketCountries = new HashSet<string>(new[] {configuration.Countries})
            };

            var leagues = await api.listCompetitions(marketFilter);

            var result = new List<SportLeague>();
            result.AddRange(leagues.Select(x => new SportLeague(x.Competition.Name)));
            return result;
        }

        public override async Task<IEnumerable<SportType>> ReadSports()
        {
            var marketFilter = new MarketFilter
            {
                EventTypeIds = new HashSet<string>(new[] {configuration.Sports}),
                MarketCountries = new HashSet<string>(new[] {configuration.Countries})
            };

            var types = await api.listEventTypes(marketFilter);

            return types.Select(x => new SportType(x.EventType.Name));
        }

        protected override async void ReadLineFunction(object param)
        {
            var model = (ReadEventsCompetitionModel) param;

            // read events
            var marketFilter = new MarketFilter
            {
                CompetitionIds = new HashSet<string>(new[] {model.Competition}),
                MarketStartTime = model.EventTimeRange,
                MarketTypeCodes = new HashSet<string>(new List<string>(new[] {"MATCH_ODDS"})) // type of odds
                //MarketBettingTypes = new HashSet<MarketBettingType>(new List<MarketBettingType>(new [] {MarketBettingType.FIXED_ODDS, MarketBettingType.ODDS}))
            };

            var marketProjection = new HashSet<MarketProjection>(new List<MarketProjection>(new[]
            {
                MarketProjection.RUNNER_DESCRIPTION,
                MarketProjection.RUNNER_METADATA
            }));

            var price = new PriceProjection();
            price.Virtualise = false;
            price.PriceData = new HashSet<PriceData>(new PriceData[]
            {
                PriceData.EX_ALL_OFFERS, PriceData.EX_BEST_OFFERS, PriceData.SP_AVAILABLE, PriceData.SP_TRADED,
                PriceData.EX_TRADED
            });

            BookmakerLineModel line = new BookmakerLineModel();

            while (true)
            {
                // get events
                var listEvents = await api.listEvents(marketFilter);
                line.UpdateEvents(listEvents);
                

                // get markets
                foreach (var sportEventId in line.EventIds)
                {
                    marketFilter.EventIds = new HashSet<string>(new[] { sportEventId});
                    var marketCatalogue =
                        await api.listMarketCatalogue(marketFilter, marketProjection, MarketSort.FIRST_TO_START);

                    var matchOddsCatalogue = marketCatalogue[0];
                    var marketBookList = await api.listMarketBook(new List<string>(new[] {matchOddsCatalogue.MarketId}),
                        price, OrderProjection.ALL);
                    var marketBook = marketBookList[0];
                    
                    line.UpdateOdds(sportEventId, matchOddsCatalogue, marketBook);
                }

                OnBookmakerLineChanged(line);

                // stop
                if (StopReadLineThreadEvent.WaitOne(TimeSpan.FromSeconds(1))) break;
            }
        }
    }
}