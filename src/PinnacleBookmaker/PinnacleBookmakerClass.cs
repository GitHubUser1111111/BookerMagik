﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookerMagikCore.Bookmaker;
using EntityLibrary.Abstract.Sport;
using EntityLibrary.Business.Sport.Football;
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

        public override async Task<IEnumerable<FootballSportEvent>> ReadEvents()
        {
            long lastFixture = 0;
            var leagues = new[] { configuration.Leagues }.ToList();

            int.TryParse(configuration.Sports, out var sportId);

            var leagueIds= new List<int>();
            foreach (var league in configuration.Leagues.Split())
            {
                if(int.TryParse(league, out int leagueId))
                    leagueIds.Add(leagueId);
            }

            var fixtures = await api.GetFixtures(new GetFixturesRequest(sportId, leagueIds, lastFixture));

            //Subsequent calls to GetOdds or GetFixtures should pass these 'Last' values to get only what changed since instead of the full snapshot
            lastFixture = fixtures.Last;

            var footballEvents = new List<FootballSportEvent>();

            foreach (var sportEvent in fixtures.Leagues.SelectMany(x => x.Events))
            {
                var footballEvent = new FootballSportEvent(sportEvent.Start, new FootballTeam(sportEvent.Home),
                    new FootballTeam(sportEvent.Away));

                footballEvents.Add(footballEvent);
            }

            return footballEvents;
        }

        public override async Task<IEnumerable<SportLeague>> ReadLeagues()
        {
            bool isSportFiltered = !string.IsNullOrWhiteSpace(configuration.Sports);
            bool isLeagueFiltered = !string.IsNullOrWhiteSpace(configuration.Leagues);

            List<int> sportIds = new List<int>();
            if (!isSportFiltered)
            {
                var sports = await api.GetSports();
                sportIds.AddRange(sports.Select(x => x.Id));
            }
            else if(int.TryParse(configuration.Sports, out int sportId))
            {
                sportIds.Add(sportId);
            }

            List<SportLeague> result = new List<SportLeague>();
            foreach (var sportId in sportIds)
            {
                var leagues = await api.GetLeagues(sportId);
                result.AddRange(leagues.Select(x => new SportLeague(x.Name)));
            }

            return result;
        }
    }
}