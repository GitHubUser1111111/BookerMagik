﻿using System;
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

        public override async Task<IEnumerable<BookmakerTwoParticipantSportEvent>> ReadEvents()
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

            var BookmakerTwoParticipantSportEvents = new List<BookmakerTwoParticipantSportEvent>();

            foreach (var sportEvent in listEvents.Where(x => x.Event.OpenDate.HasValue))
            {
                var name = sportEvent.Event.Name;
                if (!name.Contains(" v "))
                    continue;

                var teams = name.Split(" v ");
                var eventTime = sportEvent.Event.OpenDate.Value;
                var BookmakerTwoParticipantSportEvent =
                    new BookmakerTwoParticipantSportEvent(eventTime, new BookmakerEventParticipant(teams[0].Trim()),
                        new BookmakerEventParticipant(teams[1].Trim()));

                BookmakerTwoParticipantSportEvents.Add(BookmakerTwoParticipantSportEvent);
            }

            return BookmakerTwoParticipantSportEvents;
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
                MarketStartTime = model.EventTimeRange
            };

            while (true)
            {
                // action
                var listEvents = await api.listEvents(marketFilter);

                var BookmakerTwoParticipantSportEvents = new List<BookmakerTwoParticipantSportEvent>();

                foreach (var sportEvent in listEvents.Where(x => x.Event.OpenDate.HasValue))
                {
                    var name = sportEvent.Event.Name;
                    if (!name.Contains(" v "))
                        continue;

                    var teams = name.Split(" v ");
                    var eventTime = sportEvent.Event.OpenDate.Value;
                    var BookmakerTwoParticipantSportEvent =
                        new BookmakerTwoParticipantSportEvent(eventTime, new BookmakerEventParticipant(teams[0].Trim()),
                            new BookmakerEventParticipant(teams[1].Trim()));

                    BookmakerTwoParticipantSportEvents.Add(BookmakerTwoParticipantSportEvent);
                }

                OnBookmakerLineChanged(BookmakerTwoParticipantSportEvents);

                // stop
                if (StopReadLineThreadEvent.WaitOne(1000)) break;
            }
        }
    }
}