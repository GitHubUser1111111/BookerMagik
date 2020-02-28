using System;
using System.Collections.Generic;
using System.Text;
using EntityLibrary.Abstract.Bookmaker;

namespace EntityLibrary.Abstract.Sport
{
    public abstract class SportEventParticipantAbstract : BookmakerEventParticipantAbstract
    {
        protected SportEventParticipantAbstract(string name) : base(name)
        {
        }
    }
}
