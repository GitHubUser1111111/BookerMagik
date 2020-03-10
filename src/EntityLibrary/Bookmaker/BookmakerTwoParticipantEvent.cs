using System;
using System.Collections.Generic;
using System.Diagnostics;
using EntityLibrary.Bookmaker.Sport;

namespace EntityLibrary.Bookmaker
{
    [DebuggerDisplay("{StartTime}: {HomeTeam.Name} vs {AwayTeam.Name}")]
    public class BookmakerTwoParticipantEvent : BookmakerEvent
    {
        private readonly Dictionary<SportOddType, decimal> _oddsDictionary = new Dictionary<SportOddType, decimal>();

        public IReadOnlyDictionary<SportOddType, decimal> OddsDictionary => _oddsDictionary;

        public BookmakerTwoParticipantEvent(DateTime startTime,
            BookmakerEventParticipant homeTeam,
            BookmakerEventParticipant awayTeam) : base(startTime)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
        }

        public BookmakerEventParticipant HomeTeam { get; }
        public BookmakerEventParticipant AwayTeam { get; }

        public void UpdateOdd(SportOddType oddType, decimal coefficient)
        {
            if (_oddsDictionary.ContainsKey(oddType))
            {
                _oddsDictionary[oddType] = coefficient;
            }
            else
            {
                _oddsDictionary.Add(oddType, coefficient);
            }
        }
    }
}