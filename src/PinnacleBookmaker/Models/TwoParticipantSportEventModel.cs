using System.Linq;
using EntityLibrary.Bookmaker;
using EntityLibrary.Bookmaker.Sport;
using PinnacleWrapper.Data;

namespace PinnacleBookmaker.Models
{
    public class TwoParticipantSportEventModel : BookmakerTwoParticipantEvent
    {
        private const decimal NotLooseSpreadPositive = 0.5m;
        private const decimal NotLooseSpreadNegative = -0.5m;

        private readonly FixturesEvent _fixturesEvent;

        public TwoParticipantSportEventModel(FixturesEvent fixturesEvent) : base(fixturesEvent.Start,
            new BookmakerEventParticipant(fixturesEvent.Home), new BookmakerEventParticipant(fixturesEvent.Away))
        {
            _fixturesEvent = fixturesEvent;
        }

        public long Id => _fixturesEvent.Id;

        public void UpdateOdds(GetOddsEvent oddsEvent)
        {
            var fullTime = oddsEvent.Periods.FirstOrDefault(x => x.Number == 1);
            if (fullTime == null) return;

            // money line
            var moneyLineType = fullTime.MoneyLine;
            UpdateOdd(SportOddType.HomeWin, moneyLineType.Home);
            UpdateOdd(SportOddType.Draw, moneyLineType.Draw);
            UpdateOdd(SportOddType.AwayWin, moneyLineType.Away);

            // spread
            var homeNotLoose = fullTime.Spreads?.FirstOrDefault(x => x.HomeHandicap == NotLooseSpreadPositive);
            if (homeNotLoose != null)
            {
                UpdateOdd(SportOddType.HomeNotLoose, homeNotLoose.Home);
            }
            var awayNotLoose = fullTime.Spreads?.FirstOrDefault(x => x.HomeHandicap == NotLooseSpreadNegative);
            if (awayNotLoose != null)
            {
                UpdateOdd(SportOddType.AwayNotLoose, awayNotLoose.Away);
            }
        }
    }
}