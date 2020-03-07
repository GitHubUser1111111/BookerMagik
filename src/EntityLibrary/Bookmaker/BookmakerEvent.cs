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

        public Guid? GlobalLineGuid { get; private set; }

        public bool IsLinked => GlobalLineGuid.HasValue;

        public void AttachGlobalLineGuid(Guid guid)
        {
            GlobalLineGuid = guid;
        }
    }
}