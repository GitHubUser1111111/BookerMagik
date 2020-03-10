using System.Collections.Generic;
using System.Diagnostics;

namespace EntityLibrary.Bookmaker
{
    [DebuggerDisplay("Events count: {Events.Count}")]
    public class BookmakerMergeEvent
    {
        private readonly List<BookmakerTwoParticipantEvent> _events = new List<BookmakerTwoParticipantEvent>();

        public IReadOnlyCollection<BookmakerTwoParticipantEvent> Events => _events;

        public int AddEvent(BookmakerTwoParticipantEvent bookmakerEvent)
        {
            _events.Add(bookmakerEvent);
            return _events.Count;
        }
    }
}