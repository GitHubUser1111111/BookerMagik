using System.Collections.Generic;
using System.Threading.Tasks;
using EntityLibrary.Abstract.Sport;
using EntityLibrary.Business.Sport.Football;

namespace BookerMagikCore.Bookmaker
{
    public interface IBookmaker
    {
        Task<bool> Login(string jsonConfiguration);

        Task<IEnumerable<FootballSportEvent>> ReadEvents();

        Task<IEnumerable<SportLeague>> ReadLeagues();
    }
}
