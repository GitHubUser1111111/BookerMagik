using System;
using EntityLibrary.Abstract.Bookmaker;

namespace EntityLibrary.Abstract.Sport
{
    public abstract class SportEventAbstract : BookmakerEventAbstract
    {
        protected SportEventAbstract(DateTime startTime, KindOfSport kindOfSport,
            SportEventParticipantAbstract homeTeam,
            SportEventParticipantAbstract awayTeam) : base(startTime, kindOfSport)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
        }

        public BookmakerEventParticipantAbstract HomeTeam { get; }
        public BookmakerEventParticipantAbstract AwayTeam { get; }
    }
}