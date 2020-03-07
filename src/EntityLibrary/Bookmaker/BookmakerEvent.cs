using System;

namespace EntityLibrary.Bookmaker
{
    public class BookmakerEvent
    {
        public BookmakerEvent(DateTime startTime)
        {
            StartTime = startTime.ToUniversalTime();
        }

        public DateTime StartTime { get; }

        public bool IsStarted => DateTime.UtcNow >= StartTime.ToUniversalTime();
    }
}