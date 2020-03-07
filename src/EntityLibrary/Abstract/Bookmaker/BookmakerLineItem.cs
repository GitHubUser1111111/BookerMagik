using System;
using System.Collections.Generic;
using System.Text;
using EntityLibrary.Abstract.Sport;

namespace EntityLibrary.Abstract.Bookmaker
{
    public class BookmakerLineItem
    {
        public KindOfSport Sport { get; set; }

        public string Competition { get; set; }

        public IReadOnlyCollection<SportEventAbstract> SportEvents { get; set; }
    }
}
