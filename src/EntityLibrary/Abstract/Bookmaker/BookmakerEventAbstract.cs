using System;
using EntityLibrary.Abstract.Sport;

namespace EntityLibrary.Abstract.Bookmaker
{
    public abstract class BookmakerEventAbstract
    {
        protected BookmakerEventAbstract(DateTime startTime, KindOfSport sport)
        {
            Sport = sport;
            StartTime = startTime.ToUniversalTime();
        }

        public KindOfSport Sport { get; }

        public DateTime StartTime { get; }

        public bool IsStarted => DateTime.UtcNow >= StartTime.ToUniversalTime();
    }
}