using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BookerMagikCore.Bookmaker;

namespace BookerMagikWpfPrism.Services
{
    public class BookmakerService : IBookmakerService
    {
        public string LoadBookmakerConfiguration(string bookmaker)
        {
            using var r = new StreamReader($"Configurations//{bookmaker}.json");
            return r.ReadToEnd();
        }

        public async Task<bool> LoginBookmaker(IBookmaker bookmaker, string configuration)
        {
            return await bookmaker.Login(configuration);
        }
    }
}
