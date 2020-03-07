using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookerMagikCore.Bookmaker;
using EntityLibrary.Bookmaker;
using EntityLibrary.Bookmaker.Sport;
using Newtonsoft.Json;
using PinnacleBookmaker.Contracts;
using PinnacleBookmaker.Models;
using PinnacleWrapper;
using PinnacleWrapper.Data;

namespace PinnacleBookmaker
{
    public class PinnacleBookmakerClass : BookmakerAbstract, IPinnacleBookmaker
    {
        private PinnacleClient api;
        private ConfigurationModel configuration;

        protected override string ReadLineThreadName => "Pinnacle read line thread";
        protected override object ReadLineThreadParameter => configuration;
        protected override TimeSpan WaitStopReadThreadTimeout => TimeSpan.FromSeconds(30);

        public override async Task<bool> Login(string jsonConfiguration)
        {
            configuration = JsonConvert.DeserializeObject<ConfigurationModel>(jsonConfiguration);

            var httpClient =
                HttpClientFactory.GetNewInstance(configuration.Username, configuration.Password, true,
                    configuration.ApiEndpoint);

            api = new PinnacleClient(configuration.Currency, configuration.OddsFormat, httpClient);

            var balance = await api.GetClientBalance();
            return balance != null;
        }

        public override async Task<IEnumerable<BookmakerTwoParticipantSportEvent>> ReadEvents()
        {
            long lastFixture = 0;
            var leagues = new[] {configuration.Leagues}.ToList();

            int.TryParse(configuration.Sports, out var sportId);

            var leagueIds = new List<int>();
            foreach (var league in configuration.Leagues.Split())
                if (int.TryParse(league, out var leagueId))
                    leagueIds.Add(leagueId);

            var fixtures = await api.GetFixtures(new GetFixturesRequest(sportId, leagueIds, lastFixture));
            return MapToSportEvents(fixtures);
        }

        public override async Task<IEnumerable<SportLeague>> ReadLeagues()
        {
            var isSportFiltered = !string.IsNullOrWhiteSpace(configuration.Sports);
            var isLeagueFiltered = !string.IsNullOrWhiteSpace(configuration.Leagues);

            var sportIds = new List<int>();
            if (!isSportFiltered)
            {
                var sports = await api.GetSports();
                sportIds.AddRange(sports.Select(x => x.Id));
            }
            else if (int.TryParse(configuration.Sports, out var sportId))
            {
                sportIds.Add(sportId);
            }

            var result = new List<SportLeague>();
            foreach (var sportId in sportIds)
            {
                var leagues = await api.GetLeagues(sportId);
                result.AddRange(leagues.Select(x => new SportLeague(x.Name)));
            }

            return result;
        }

        public override async Task<IEnumerable<SportType>> ReadSports()
        {
            var sports = await api.GetSports();
            return sports.Select(x => new SportType(x.Name));
        }

        protected override async void ReadLineFunction(object param)
        {
            var configuration = (ConfigurationModel) param;

            long lastFixture = 0;
            long lastOdds = 0;

            var leagues = new[] {configuration.Leagues}.ToList();

            int.TryParse(configuration.Sports, out var sportId);

            var leagueIds = new List<int>();
            foreach (var league in configuration.Leagues.Split())
                if (int.TryParse(league, out var leagueId))
                    leagueIds.Add(leagueId);

            var fixtureRequest = new GetFixturesRequest(sportId, leagueIds, lastFixture);
            var oddsRequest = new GetOddsRequest(sportId, leagueIds, lastOdds);

            while (true)
            {
                var fixtures = await api.GetFixtures(fixtureRequest);

                // events
                if (fixtures != null)
                {
                    var footballEvents = MapToSportEvents(fixtures);
                    OnBookmakerLineChanged(footballEvents);
                    fixtureRequest.Since = fixtures.Last;
                }

                var odds = await api.GetOdds(oddsRequest);
                if (odds != null) oddsRequest.Since = odds.Last;

                // stop
                if (StopReadLineThreadEvent.WaitOne(1000)) break;
            }
        }

        public static IEnumerable<BookmakerTwoParticipantSportEvent> MapToSportEvents(GetFixturesResponse fixtures)
        {
            var footballEvents = new List<BookmakerTwoParticipantSportEvent>();

            foreach (var sportEvent in fixtures.Leagues.SelectMany(x => x.Events))
            {
                var footballEvent = new BookmakerTwoParticipantSportEvent(sportEvent.Start,
                    new BookmakerEventParticipant(sportEvent.Home),
                    new BookmakerEventParticipant(sportEvent.Away));

                footballEvents.Add(footballEvent);
            }

            return footballEvents;
        }
    }
}