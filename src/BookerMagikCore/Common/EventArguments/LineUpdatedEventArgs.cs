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
        public LineUpdatedEventArgs(IEnumerable<BookmakerTwoParticipantSportEvent> sportEvents)
        {
            SportEvents = new ReadOnlyCollection<BookmakerTwoParticipantSportEvent>(sportEvents.ToList());
        }

        public ReadOnlyCollection<BookmakerTwoParticipantSportEvent> SportEvents { get; }
    }
}