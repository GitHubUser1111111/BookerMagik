namespace EntityLibrary.Bookmaker.Sport.Odd
{
    /// <summary>
    /// Tennis e.g.
    /// </summary>
    public class SportTwoResultOdd
    {
        public SportResultOdd HomeWin { get; }
        public SportResultOdd AwayWin { get; }

        public SportTwoResultOdd(SportResultOdd homeWin, SportResultOdd awayWin)
        {
            HomeWin = homeWin;
            AwayWin = awayWin;
        }
    }
}
