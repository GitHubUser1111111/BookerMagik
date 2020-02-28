using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EntityLibrary.Abstract.Bookmaker
{
    public abstract class BookmakerEventAbstract
    {
        public DateTime StartTime { get; }

        public bool IsStarted => DateTime.UtcNow >= StartTime.ToUniversalTime();

        public IReadOnlyCollection<BookmakerEventParticipantAbstract> Participants { get; }

        protected BookmakerEventAbstract(DateTime startTime, IEnumerable<BookmakerEventParticipantAbstract> participants)
        {
            StartTime = startTime.ToUniversalTime();
            Participants = new ReadOnlyCollection<BookmakerEventParticipantAbstract>(participants.ToList());
        }
    }
}
