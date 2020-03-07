using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityLibrary.Bookmaker
{
    public class BookmakerGlobalLine
    {
        public Dictionary<Guid, BookmakerMergeEvent> Events { get; } = new Dictionary<Guid, BookmakerMergeEvent>();

        public void AddNewBookmakerMergeEvent(Guid guid, BookmakerMergeEvent bookmakerMergeEvent)
        {
            Events.Add(guid, bookmakerMergeEvent);
        }

        public BookmakerMergeEvent GetExistMergeEvent(Guid guid)
        {
            return Events.ContainsKey(guid) ? Events[guid] : null;
        }

        public List<BookmakerMergeEvent> GetBookmakerMergeEvents()
        {
            return Events.Values.Where(x => x.Events.Count > 1).ToList();
        }
    }
}