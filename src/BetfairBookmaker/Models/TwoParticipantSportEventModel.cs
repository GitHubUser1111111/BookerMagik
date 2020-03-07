using System;
using EntityLibrary.Bookmaker;
using EntityLibrary.Bookmaker.Sport;

namespace BetfairBookmaker.Models
{
    public class TwoParticipantSportEventModel : BookmakerTwoParticipantSportEvent
    {
        public TwoParticipantSportEventModel(DateTime startTime, BookmakerEventParticipant homeTeam,
            BookmakerEventParticipant awayTeam) : base(startTime, homeTeam, awayTeam)
        {
        }
    }
}