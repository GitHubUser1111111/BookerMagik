using System;

namespace BetfairBookmaker.Models
{
    [Serializable]
    public class ConfigurationModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string CertificatePath { get; set; }

        public string CertificatePassword { get; set; }

        public string ApplicationKey { get; set; }

        public string ApiEndpoint { get; set; }

        public string Sports { get; set; }

        public string Leagues { get; set; }

        public string Countries { get; set; }

        public int AddDays { get; set; }
    }
}