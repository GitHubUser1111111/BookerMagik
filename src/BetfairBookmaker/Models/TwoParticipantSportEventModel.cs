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

        public void UpdateMatchOdds(MarketCatalogue matchOddsCatalogue, MarketBook matchOddsEvent)
        {
            var homeRunner = matchOddsCatalogue.Runners.FirstOrDefault(x => x.RunnerName == HomeTeam.Name);
            var drawRunner = matchOddsCatalogue.Runners.FirstOrDefault(x => x.RunnerName == "The Draw");
            var awayRunner = matchOddsCatalogue.Runners.FirstOrDefault(x => x.RunnerName == AwayTeam.Name);

            if (homeRunner != null && awayRunner != null && drawRunner != null && matchOddsEvent.Runners.All(x => x.LastPriceTraded.HasValue))
            {
                var homeMarketRunner = matchOddsEvent.Runners.FirstOrDefault(x => x.SelectionId == homeRunner.SelectionId);
                var drawMarketRunner = matchOddsEvent.Runners.FirstOrDefault(x => x.SelectionId == drawRunner.SelectionId);
                var awayMarketRunner = matchOddsEvent.Runners.FirstOrDefault(x => x.SelectionId == awayRunner.SelectionId);

                UpdateOdd(SportOddType.HomeWin, (decimal)homeMarketRunner.LastPriceTraded);
                UpdateOdd(SportOddType.Draw, (decimal)drawMarketRunner.LastPriceTraded);
                UpdateOdd(SportOddType.AwayWin, (decimal)awayMarketRunner.LastPriceTraded);
            }
        }
    }
}