using System;
using EntityLibrary.Abstract.Sport;

namespace EntityLibrary.Business.Sport.Odd
{
    public class SportThreeResultOdd
    {
        public SportResultOdd HomeWin { get; }
        public SportResultOdd AwayWin { get; }
        public SportResultOdd Draw { get; }

        public SportThreeResultOdd(SportResultOdd homeWin, SportResultOdd awayWin,
            SportResultOdd draw)
        {
            HomeWin = homeWin;
            AwayWin = awayWin;
            Draw = draw;
        }
    }
}
