using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookerMagikCore.Common.EventArguments;
using EntityLibrary.Abstract.Bookmaker;
using EntityLibrary.Abstract.Sport;
using EntityLibrary.Business.Sport.Football;

namespace BookerMagikCore.Bookmaker
{
    public interface IBookmaker
    {
        event EventHandler<LineUpdatedEventArgs> LineUpdated;

        Task<bool> Login(string jsonConfiguration);

        Task<IEnumerable<FootballSportEvent>> ReadEvents();

        Task<IEnumerable<SportLeague>> ReadLeagues();

        Task<IEnumerable<SportType>> ReadSports();

        void StartReadLineThread();

        void StopReadLineThread();
    }
}
