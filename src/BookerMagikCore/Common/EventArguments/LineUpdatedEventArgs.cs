using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EntityLibrary.Bookmaker;
using EntityLibrary.Bookmaker.Sport;

namespace BookerMagikCore.Common.EventArguments
{
    public class LineUpdatedEventArgs : EventArgs
    {
        public LineUpdatedEventArgs(IEnumerable<BookmakerTwoParticipantEvent> sportEvents)
        {
            SportEvents = new ReadOnlyCollection<BookmakerTwoParticipantEvent>(sportEvents.ToList());
        }

        public ReadOnlyCollection<BookmakerTwoParticipantEvent> SportEvents { get; }
    }
}