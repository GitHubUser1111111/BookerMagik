using EntityLibrary.Abstract.Bookmaker;

namespace EntityLibrary.Business.Sport.Odd
{
    public class SportResultOdd : BookmakerEventResultAbstract
    {
        public SportResultOdd(decimal coefficient) : base(coefficient)
        {
        }
    }
}