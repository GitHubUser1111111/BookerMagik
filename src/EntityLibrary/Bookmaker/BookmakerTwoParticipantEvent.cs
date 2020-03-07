using System;
using System.Diagnostics;

namespace EntityLibrary.Bookmaker
{
    [DebuggerDisplay("{StartTime}: {HomeTeam.Name} vs {AwayTeam.Name}")]
    public class BookmakerTwoParticipantEvent : BookmakerEvent
    {
        public BookmakerTwoParticipantEvent(DateTime startTime,
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