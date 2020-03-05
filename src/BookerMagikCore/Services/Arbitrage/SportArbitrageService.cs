using EntityLibrary.Business.Arbitrage;
using EntityLibrary.Business.Sport.Odd;

namespace BookerMagikCore.Services.Arbitrage
{
    public class SportArbitrageService : ISportArbitrageService
    {
        public BookmakerEventArbitrage GetTwoResultSportArbitrage(SportResultOdd r1, SportResultOdd r2)
        {
            var profit = 1 / r1.Coefficient + 1 / r2.Coefficient;
            return profit < 1 ? new BookmakerEventArbitrage(profit) : null;
        }

        public BookmakerEventArbitrage Get1X2Arbitrage(SportResultOdd homeWin, SportResultOdd awayNotLoose)
        {
            return GetTwoResultSportArbitrage(homeWin, awayNotLoose);
        }

        public BookmakerEventArbitrage Get2X1Arbitrage(SportResultOdd awayWin, SportResultOdd homeNotLoose)
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
            decimal oddAmount = (1 / coefficient / arbitrageWeight) * totalAmount;
            return oddAmount;
        }
    }
}