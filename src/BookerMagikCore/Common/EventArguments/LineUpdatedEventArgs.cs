using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EntityLibrary.Business.Sport.Football;

namespace BookerMagikCore.Common.EventArguments
{
    public class LineUpdatedEventArgs : EventArgs
    {
        public ReadOnlyCollection<FootballSportEvent> SportEvents { get; }

        public LineUpdatedEventArgs(IEnumerable<FootballSportEvent> sportEvents)
        {
            SportEvents = new ReadOnlyCollection<FootballSportEvent>(sportEvents.ToList());
        }
    }
}
