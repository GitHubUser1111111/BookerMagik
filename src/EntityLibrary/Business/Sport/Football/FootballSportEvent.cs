using System;
using EntityLibrary.Abstract.Sport;
using EntityLibrary.Business.Sport.Odd;

namespace EntityLibrary.Business.Sport.Football
{
    public class FootballSportEvent : SportEventAbstract
    {
        /// <summary>
        /// 1
        /// </summary>
        public SportResultOdd HomeWin { get; set; }
        
        /// <summary>
        /// 2
        /// </summary>
        public SportResultOdd AwayWin { get; set; }
        
        /// <summary>
        /// X
        /// </summary>
        public SportResultOdd Draw { get; set; }

        /// <summary>
        /// 1X
        /// </summary>
        public SportResultOdd HomeNotLoose { get; set; }
        
        /// <summary>
        /// X2
        /// </summary>
        public SportResultOdd AwayNotLoose { get; set; }

        public FootballSportEvent(DateTime startTime, FootballTeam homeTeam, FootballTeam awayTeam) : base(startTime, KindOfSport.Football, homeTeam, awayTeam)
        {
        }
    }
}
