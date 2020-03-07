using System;
using System.Collections.Generic;
using System.Text;
using BookerMagikCore.Services.Arbitrage;

namespace BookerMagikCore.Strategies
{
    public interface IArbitrageStrategy
    {
        IEnumerable<SportArbitrageService> FindArbitrageEvents();
    }
}
