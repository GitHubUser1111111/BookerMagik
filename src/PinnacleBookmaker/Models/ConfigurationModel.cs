using System;
using PinnacleWrapper.Enums;

namespace PinnacleBookmaker.Models
{
    [Serializable]
    public class ConfigurationModel
    {
        public string ApiEndpoint { get; set; }
        public string Currency { get; set; }
        public OddsFormat OddsFormat { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Sports { get; set; }
        public string Leagues { get; set; }
        public string Countries { get; set; }
        public int AddDays { get; set; }
    }
}