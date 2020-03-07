using System;
using EntityLibrary.Bookmaker;

namespace BookerMagikCore.Common.EventArguments
{
    public class LineUpdatedEventArgs : EventArgs
    {
        public LineUpdatedEventArgs(BookmakerLine line)
        {
            Line = line;
        }

        public BookmakerLine Line { get; }
    }
}