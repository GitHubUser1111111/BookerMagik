using EntityLibrary.Bookmaker;
using EntityLibrary.Bookmaker.Sport;
using PinnacleWrapper.Data;

namespace PinnacleBookmaker.Models
{
    public class TwoParticipantSportEventModel : BookmakerTwoParticipantEvent
    {
        private readonly FixturesEvent _fixturesEvent;

        public TwoParticipantSportEventModel(FixturesEvent fixturesEvent) : base(fixturesEvent.Start,
            new BookmakerEventParticipant(fixturesEvent.Home), new BookmakerEventParticipant(fixturesEvent.Away))
        {
            _fixturesEvent = fixturesEvent;
        }

        public long Id => _fixturesEvent.Id;

        public void UpdateOdds(MoneyLineType moneyLineType)
        {
            UpdateOdd(SportOddType.HomeWin, moneyLineType.Home);
            UpdateOdd(SportOddType.Draw, moneyLineType.Draw);
            UpdateOdd(SportOddType.AwayWin, moneyLineType.Away);
        }
    }
}