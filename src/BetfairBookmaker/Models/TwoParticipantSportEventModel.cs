using System.Linq;
using BetfairApi.TO;
using EntityLibrary.Bookmaker;
using EntityLibrary.Bookmaker.Sport;

namespace BetfairBookmaker.Models
{
    public class TwoParticipantSportEventModel : BookmakerTwoParticipantEvent
    {
        private readonly EventResult _eventResult;

        public string Id => _eventResult.Event.Id;
        public TwoParticipantSportEventModel(EventResult eventResult, string homeTeam, string awayTeam) : base(
            eventResult.Event.OpenDate.Value, new BookmakerEventParticipant(homeTeam),
            new BookmakerEventParticipant(awayTeam))
        {
            _eventResult = eventResult;
        }

        public void UpdateMatchOdds(MarketCatalogue marketCatalogue, MarketBook marketBook)
        {
            if (marketCatalogue.MarketName == "Double Chance")
            {
                var homeOrDrawRunner = marketCatalogue.Runners.FirstOrDefault(x => x.RunnerName == "Home or Draw");
                if (homeOrDrawRunner != null)
                {
                    var homeOrDrawMarketRunner =
                        marketBook.Runners.First(x => x.SelectionId == homeOrDrawRunner.SelectionId);
                    if (homeOrDrawMarketRunner.LastPriceTraded.HasValue)
                        UpdateOdd(SportOddType.HomeNotLoose, (decimal) homeOrDrawMarketRunner.LastPriceTraded);
                }


                var drawOrAwayRunner = marketCatalogue.Runners.FirstOrDefault(x => x.RunnerName == "Draw or Away");
                if (drawOrAwayRunner != null)
                {
                    var drawOrAwayMarketRunner =
                        marketBook.Runners.First(x => x.SelectionId == drawOrAwayRunner.SelectionId);
                    if (drawOrAwayMarketRunner.LastPriceTraded.HasValue)
                        UpdateOdd(SportOddType.AwayNotLoose, (decimal)drawOrAwayMarketRunner.LastPriceTraded);
                }

                //var homeOrAwayRunner = marketCatalogue.Runners.FirstOrDefault(x => x.RunnerName == "Home or Away");
                //if (homeOrAwayRunner != null)
                //{
                //    var homeOrAwayMarketRunner =
                //        marketBook.Runners.First(x => x.SelectionId == homeOrAwayRunner.SelectionId);
                //    if (homeOrAwayMarketRunner.LastPriceTraded.HasValue)
                //        UpdateOdd(SportOddType., (decimal)homeOrAwayMarketRunner.LastPriceTraded);
                //}
            }

            if (marketCatalogue.MarketName == "Match Odds")
            {
                var homeRunner = marketCatalogue.Runners.FirstOrDefault(x => x.RunnerName == HomeTeam.Name);
                var drawRunner = marketCatalogue.Runners.FirstOrDefault(x => x.RunnerName == "The Draw");
                var awayRunner = marketCatalogue.Runners.FirstOrDefault(x => x.RunnerName == AwayTeam.Name);

                if (homeRunner != null && awayRunner != null && drawRunner != null &&
                    marketBook.Runners.All(x => x.LastPriceTraded.HasValue))
                {
                    var homeMarketRunner =
                        marketBook.Runners.FirstOrDefault(x => x.SelectionId == homeRunner.SelectionId);
                    var drawMarketRunner =
                        marketBook.Runners.FirstOrDefault(x => x.SelectionId == drawRunner.SelectionId);
                    var awayMarketRunner =
                        marketBook.Runners.FirstOrDefault(x => x.SelectionId == awayRunner.SelectionId);

                    UpdateOdd(SportOddType.HomeWin, (decimal) homeMarketRunner.LastPriceTraded);
                    UpdateOdd(SportOddType.Draw, (decimal) drawMarketRunner.LastPriceTraded);
                    UpdateOdd(SportOddType.AwayWin, (decimal) awayMarketRunner.LastPriceTraded);
                }
            }
        }
    }
}