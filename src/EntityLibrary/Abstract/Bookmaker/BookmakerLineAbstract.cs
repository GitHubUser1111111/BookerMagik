using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLibrary.Abstract.Bookmaker
{
    public abstract class BookmakerLineAbstract
    {
        private List<BookmakerEventAbstract> _events;

        protected BookmakerLineAbstract()
        {
            _events = new List<BookmakerEventAbstract>();
        }

        public void AddEvent(BookmakerEventAbstract bookmakerEvent)
        {
            _events.Add(bookmakerEvent);
        }

        public void RemoveAllStartedEvents(DateTime eventTime)
        {
            _events.RemoveAll(x => x.StartTime >= eventTime);
        }
    }
}
