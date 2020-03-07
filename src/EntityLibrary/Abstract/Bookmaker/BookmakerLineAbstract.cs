using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using EntityLibrary.Abstract.Sport;

namespace EntityLibrary.Abstract.Bookmaker
{
    public abstract class BookmakerLineAbstract
    {
        private Dictionary<BookmakerSportEventKey, List<SportEventAbstract>> _items;

        protected BookmakerLineAbstract()
        {
            _items = new Dictionary<BookmakerSportEventKey, List<SportEventAbstract>>();
        }

        public void AddEvent(BookmakerEventAbstract bookmakerEvent)
        {
            //_events.Add(bookmakerEvent);
        }

        public void RemoveAllStartedEvents(DateTime eventTime)
        {
           // _events.RemoveAll(x => x.StartTime >= eventTime);
        }
    }
}
