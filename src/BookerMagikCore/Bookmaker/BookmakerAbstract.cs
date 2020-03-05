using System.Collections.Generic;
using System.Threading.Tasks;
using EntityLibrary.Abstract.Sport;
using EntityLibrary.Business.Sport.Football;

namespace BookerMagikCore.Bookmaker
{
    public abstract class BookmakerAbstract : IBookmaker
    {
        public abstract Task<bool> Login(string jsonConfiguration);
        public abstract Task<IEnumerable<FootballSportEvent>> ReadEvents();
        public abstract Task<IEnumerable<SportLeague>> ReadLeagues();
    }
}
