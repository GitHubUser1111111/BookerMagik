using System.Threading.Tasks;

namespace BookerMagikCore.Bookmaker
{
    public abstract class BookmakerAbstract : IBookmaker
    {
        public abstract Task<bool> Login(string jsonConfiguration);
        public abstract Task<bool> ReadEvents();
    }
}
