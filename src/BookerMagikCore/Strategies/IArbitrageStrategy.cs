using System.Collections.Generic;
using BookerMagikCore.Services.Arbitrage;

namespace BookerMagikCore.Strategies
{
    public interface IArbitrageStrategy
    {
        IEnumerable<SportArbitrageService> FindArbitrageEvents();
    }
}