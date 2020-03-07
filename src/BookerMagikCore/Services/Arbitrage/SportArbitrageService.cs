using EntityLibrary.Bookmaker;
using EntityLibrary.Business.Arbitrage;

namespace BookerMagikCore.Services.Arbitrage
{
    public class SportArbitrageService : ISportArbitrageService
    {
        public BookmakerEventArbitrage GetTwoResultSportArbitrage(BookmakerEventOdd r1, BookmakerEventOdd r2)
        {
            var profit = 1 / r1.Coefficient + 1 / r2.Coefficient;
            return profit < 1 ? new BookmakerEventArbitrage(profit) : null;
        }

        public BookmakerEventArbitrage Get1X2Arbitrage(BookmakerEventOdd homeWin, BookmakerEventOdd awayNotLoose)
        {
            return GetTwoResultSportArbitrage(homeWin, awayNotLoose);
        }

        public BookmakerEventArbitrage Get2X1Arbitrage(BookmakerEventOdd awayWin, BookmakerEventOdd homeNotLoose)
        {
            return GetTwoResultSportArbitrage(awayWin, homeNotLoose);
        }

        /// <summary>
        /// </summary>
        /// <param name="coefficient">bookmaker event coefficient</param>
        /// <param name="arbitrageWeight">arbitrage weight</param>
        /// <param name="totalAmount">total amount available on event</param>
        /// <returns>bet amount</returns>
        public decimal CalculateBetAmount(decimal coefficient, decimal arbitrageWeight, decimal totalAmount)
        {
            var oddAmount = 1 / coefficient / arbitrageWeight * totalAmount;
            return oddAmount;
        }
    }
}