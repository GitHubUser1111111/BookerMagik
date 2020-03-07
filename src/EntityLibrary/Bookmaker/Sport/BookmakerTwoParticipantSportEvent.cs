using System;

namespace EntityLibrary.Bookmaker.Sport
{
    public class BookmakerTwoParticipantSportEvent : BookmakerEvent
    {
        public BookmakerTwoParticipantSportEvent(DateTime startTime,
            BookmakerEventParticipant homeTeam,
            BookmakerEventParticipant awayTeam) : base(startTime)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
        }

        public BookmakerEventParticipant HomeTeam { get; }
        public BookmakerEventParticipant AwayTeam { get; }
    }
}