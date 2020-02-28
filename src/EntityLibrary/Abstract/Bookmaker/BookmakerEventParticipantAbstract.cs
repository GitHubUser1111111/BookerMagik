using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLibrary.Abstract.Bookmaker
{
    public abstract class BookmakerEventParticipantAbstract
    {
        public string Name { get; }

        protected BookmakerEventParticipantAbstract(string name)
        {
            Name = name;
        }
    }
}
