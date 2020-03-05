using System.Threading.Tasks;

namespace BookerMagikCore.Bookmaker
{
    public interface IBookmaker
    {
        Task<bool> Login(string jsonConfiguration);

        Task<bool> ReadEvents();
    }
}
