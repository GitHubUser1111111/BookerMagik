using System;
using System.IO;
using System.Threading.Tasks;
using BetfairBookmaker;
using BetfairBookmaker.Contracts;
using NLog;
using PinnacleBookmaker;
using PinnacleBookmaker.Contracts;

namespace SampleConsoleApp
{
    internal class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static async Task Main(string[] args)
        {
            Logger.Info("Starting...");

            // Betfair
            var betfairBookmaker = await CreateBetfairBookmakerAsync();
            Console.WriteLine("Get Betfair events...");
            var betfairEvents = await betfairBookmaker.ReadEvents();

            // Pinnacle
            var pinnacleBookmaker = await CreatePinnacleBookmakerAsync();
            Console.WriteLine("Get Betfair events...");
            var pinnacleEvents = await pinnacleBookmaker.ReadEvents();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            Logger.Info("Exit...");
        }

        private static async Task<IBetfairBookmaker> CreateBetfairBookmakerAsync()
        {
            Console.WriteLine("Get Betfair configuration...");
            using var r = new StreamReader("Configurations//betfair.json");
            var json = r.ReadToEnd();
            Console.WriteLine("Create Betfair bookmaker...");
            IBetfairBookmaker bookmaker = new BetfairBookmakerClass();
            Console.WriteLine("Login Betfair bookmaker...");
            var res = await bookmaker.Login(json);
            Console.Write($"{res}\n");
            return bookmaker;
        }

        private static async Task<IPinnacleBookmaker> CreatePinnacleBookmakerAsync()
        {
            Console.WriteLine("Get Pinnacle configuration...");
            using var r = new StreamReader("Configurations//pinnacle.json");
            var json = r.ReadToEnd();
            Console.WriteLine("Create Pinnacle bookmaker...");
            IPinnacleBookmaker bookmaker = new PinnacleBookmakerClass();
            Console.WriteLine("Login Pinnacle bookmaker...");
            var res = await bookmaker.Login(json);
            Console.Write($"{res}\n");
            return bookmaker;
        }
    }
}