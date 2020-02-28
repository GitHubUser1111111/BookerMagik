using System;
using System.Collections.Generic;
using System.Text;
using EntityLibrary.Abstract.Bookmaker;

namespace EntityLibrary.Abstract.Sport
{
    public abstract class SportEventAbstract : BookmakerEventAbstract
    {
        public KindOfSport KindOfSport { get; }
        public BookmakerEventParticipantAbstract HomeTeam { get; }
        public BookmakerEventParticipantAbstract AwayTeam { get; }

        protected SportEventAbstract(DateTime startTime, KindOfSport kindOfSport, SportEventParticipantAbstract homeTeam,
            SportEventParticipantAbstract awayTeam) : base(startTime, new[] {homeTeam, awayTeam})
        {
            KindOfSport = kindOfSport;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
        }
    }
}
