using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookerMagikCore.Common.EventArguments;
using EntityLibrary.Bookmaker;
using EntityLibrary.Bookmaker.Sport;

namespace BookerMagikCore.Bookmaker
{
    public interface IBookmaker
    {
        event EventHandler<LineUpdatedEventArgs> LineUpdated;

        Task<bool> Login(string jsonConfiguration);

        Task<IEnumerable<BookmakerTwoParticipantEvent>> ReadEvents();

        Task<IEnumerable<SportLeague>> ReadLeagues();

        Task<IEnumerable<SportType>> ReadSports();

        void StartReadLineThread();

        void StopReadLineThread();
    }
}