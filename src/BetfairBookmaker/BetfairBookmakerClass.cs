using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetfairApi;
using BetfairApi.TO;
using BetfairBookmaker.Contracts;
using BetfairBookmaker.Models;
using BookerMagikCore.Bookmaker;
using Newtonsoft.Json;

namespace BetfairBookmaker
{
    public class BetfairBookmakerClass : BookmakerAbstract, IBetfairBookmaker
    {
        private IClient api;
        private ConfigurationModel configuration;
        private string token;
        
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

        public override async Task<bool> ReadEvents()
        {
            var time = new TimeRange { From = DateTime.Now, To = DateTime.Now.AddDays(configuration.AddDays) };
            var marketFilter = new MarketFilter
            {
                EventTypeIds = new HashSet<string>(new[] { configuration.Sports }),
                CompetitionIds = new HashSet<string>(new[] { configuration.Leagues }),
                MarketCountries = new HashSet<string>(new[] { configuration.Countries }),
                MarketStartTime = time
            };
            
            //as an example we requested runner metadata 
            ISet<MarketProjection> marketProjections = new HashSet<MarketProjection>();
            marketProjections.Add(MarketProjection.RUNNER_METADATA);
            var listEvents = await api.listEvents(marketFilter);

            return true;
        }
    }
}