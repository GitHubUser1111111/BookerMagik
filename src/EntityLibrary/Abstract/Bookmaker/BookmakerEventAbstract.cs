using System;

namespace EntityLibrary.Abstract.Bookmaker
{
    public abstract class BookmakerEventAbstract
    {
        protected BookmakerEventAbstract(DateTime startTime)
        {
            StartTime = startTime.ToUniversalTime();
        }

        public DateTime StartTime { get; }

        public bool IsStarted => DateTime.UtcNow >= StartTime.ToUniversalTime();
    }
}