using EntityLibrary.Business.Arbitrage;
using EntityLibrary.Business.Sport.Odd;

namespace EntityLibrary.Services.Arbitrage
{
    public interface ISportArbitrageService
    {
        BookmakerEventArbitrage GetTwoResultSportArbitrage(SportResultOdd r1, SportResultOdd r2);

        BookmakerEventArbitrage Get1X2Arbitrage(SportResultOdd homeWin, SportResultOdd awayNotLoose);

        BookmakerEventArbitrage Get2X1Arbitrage(SportResultOdd awayWin, SportResultOdd homeNotLoose);

        decimal CalculateBetAmount(decimal coefficient, decimal arbitrageWeight, decimal totalAmount);
    }
}