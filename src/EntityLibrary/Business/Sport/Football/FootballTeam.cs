using System;
using System.Collections.Generic;
using System.Text;
using EntityLibrary.Abstract.Sport;

namespace EntityLibrary.Business.Sport.Football
{
    public class FootballTeam : SportEventParticipantAbstract
    {
        public FootballTeam(string name) : base(name)
        {
        }
    }
}
