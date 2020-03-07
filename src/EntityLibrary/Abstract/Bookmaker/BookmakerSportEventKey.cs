using System;
using System.Collections.Generic;
using System.Text;
using EntityLibrary.Abstract.Sport;

namespace EntityLibrary.Abstract.Bookmaker
{
    public struct BookmakerSportEventKey
    {
        public KindOfSport Sport { get; set; }

        public string Competition { get; set; }
    }
}
