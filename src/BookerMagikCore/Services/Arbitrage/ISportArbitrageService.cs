using EntityLibrary.Bookmaker;
using EntityLibrary.Business.Arbitrage;

namespace BookerMagikCore.Services.Arbitrage
{
    public interface ISportArbitrageService
    {
        BookmakerEventArbitrage GetTwoResultSportArbitrage(BookmakerEventOdd r1, BookmakerEventOdd r2);

        BookmakerEventArbitrage Get1X2Arbitrage(BookmakerEventOdd homeWin, BookmakerEventOdd awayNotLoose);

        BookmakerEventArbitrage Get2X1Arbitrage(BookmakerEventOdd awayWin, BookmakerEventOdd homeNotLoose);

        decimal CalculateBetAmount(decimal coefficient, decimal arbitrageWeight, decimal totalAmount);
    }
}