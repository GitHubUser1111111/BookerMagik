using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLibrary.Bookmaker
{
    public class BookmakerMergeEvent
    {
        private readonly List<BookmakerTwoParticipantEvent> _events = new List<BookmakerTwoParticipantEvent>();

        public IReadOnlyCollection<BookmakerTwoParticipantEvent> Events => _events;

        public BookmakerMergeEvent()
        {
            
        }

        public int AddEvent(BookmakerTwoParticipantEvent bookmakerEvent)
        {
            if (_events.Count == 2)
            {
                int a = 2;
                a++;
            }

            _events.Add(bookmakerEvent);
            return _events.Count;
        }
    }
}
