using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLibrary.Bookmaker
{
    public abstract class BookmakerLine
    {
        public abstract int EventsCount { get; }

        public abstract IReadOnlyCollection<BookmakerTwoParticipantEvent> SportEvents { get; }

        protected BookmakerLine()
        {
            
        }
    }
}
