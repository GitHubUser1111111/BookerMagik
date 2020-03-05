using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BetfairBookmaker;
using BetfairBookmaker.Contracts;
using BookerMagikCore.Infrastructure;
using BookerMagikCore.Sport;
using EntityLibrary.Abstract.Sport;
using EntityLibrary.Business.Sport.Football;
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
            Console.WriteLine("Get Betfair leagues...");
            var betfairLeagues = await betfairBookmaker.ReadLeagues();
            Console.WriteLine("Get Betfair events...");
            var betfairEvents = await betfairBookmaker.ReadEvents();

            // Pinnacle
            var pinnacleBookmaker = await CreatePinnacleBookmakerAsync();
            Console.WriteLine("Get Pinnacle leagues...");
            var pinnacleLeagues = await pinnacleBookmaker.ReadLeagues();
            Console.WriteLine("Get Pinnacle events...");
            var pinnacleEvents = await pinnacleBookmaker.ReadEvents();

            // searcher
            ISameTimeEventsSearch sameTimeEvents =
                new SameTimeEventsSearch(new SimilarStringsCalculator(), new LongestCommonSubsequence());

            // Find same leagues
            Console.WriteLine("Start link leagues...");
            LinkLeagues(sameTimeEvents, betfairLeagues.ToList(), pinnacleLeagues.ToList());

            // Find same events
            Console.WriteLine("Start link events...");
            LinkEvents(sameTimeEvents, betfairEvents.ToList(), pinnacleEvents.ToList());
            //
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

        private static int LinkLeagues(ISameTimeEventsSearch search, List<SportLeague> leaguesA,
            List<SportLeague> leaguesB)
        {
            foreach (var la in leaguesA)
            {
                foreach (var lb in leaguesB)
                {
                    if (search.CheckIsSameLeague(la, lb))
                    {
                        Console.WriteLine("===========================");
                        Console.WriteLine($"Betfair: {la.Name}");
                        Console.WriteLine($"Pinnacle: {lb.Name}");
                    }
                }
            }

            return 0;
        }

        private static int LinkEvents(ISameTimeEventsSearch search, List<FootballSportEvent> eventsA, List<FootballSportEvent> eventsB)
        {
            List<Tuple<FootballSportEvent, FootballSportEvent>> linked = new List<Tuple<FootballSportEvent, FootballSportEvent>>();
            List<Tuple<FootballSportEvent, FootballSportEvent>> notLinked = new List<Tuple<FootballSportEvent, FootballSportEvent>>();
            foreach (var a in eventsA)
            {
                foreach (var b in eventsB)
                {
                    bool isSame = search.CheckIsSameEvents(a, b);
                    if(isSame)
                        linked.Add(new Tuple<FootballSportEvent, FootballSportEvent>(a, b));
                    else
                    {
                        //if (a.HomeTeam.Name.Contains("Arse") && b.AwayTeam.Name.Contains("West"))
                        //{
                        //    Console.WriteLine("SSS");
                        //}

                        notLinked.Add(new Tuple<FootballSportEvent, FootballSportEvent>(a, b));
                    }
                    
                }
            }

            foreach (var link in linked)
            {
                Console.WriteLine("===========================");
                Console.WriteLine($"Betfair: {link.Item1.HomeTeam.Name} vs {link.Item2.AwayTeam.Name}");
                Console.WriteLine($"Pinnacle: {link.Item2.HomeTeam.Name} vs {link.Item2.AwayTeam.Name}");
            }
            
            //foreach (var link in notLinked)
            //{
            //    if (link.Item1.HomeTeam.Name.Length <= 3 || link.Item1.AwayTeam.Name.Length <= 3 ||
            //        link.Item2.HomeTeam.Name.Length <= 3 || link.Item2.AwayTeam.Name.Length <= 3)
            //    {
            //        Console.WriteLine("==========Short event===========");
            //        Console.WriteLine($"Betfair: {link.Item1.HomeTeam.Name} vs {link.Item2.AwayTeam.Name}");
            //        Console.WriteLine($"Pinnacle: {link.Item2.HomeTeam.Name} vs {link.Item2.AwayTeam.Name}");
            //        continue;
            //    }


            //    var aHome = link.Item1.HomeTeam.Name.Substring(0, 3);
            //    var aAway = link.Item1.AwayTeam.Name.Substring(0, 3);

            //    var bHome = link.Item2.HomeTeam.Name.Substring(0, 3);
            //    var bAway = link.Item2.AwayTeam.Name.Substring(0, 3);

            //    if (aHome != bHome || aAway != bAway) continue;


            //    Console.WriteLine("==========Similar event===========");
            //    Console.WriteLine($"Betfair: {link.Item1.StartTime} {link.Item1.HomeTeam.Name} vs {link.Item2.AwayTeam.Name}");
            //    Console.WriteLine($"Pinnacle: {link.Item2.StartTime} {link.Item2.HomeTeam.Name} vs {link.Item2.AwayTeam.Name}");
            //}

            return linked.Count;
        }
    }
}