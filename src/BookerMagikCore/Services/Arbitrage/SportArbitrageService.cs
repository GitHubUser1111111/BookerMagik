using System;
using System.Collections.Generic;
using System.Linq;
using EntityLibrary.Bookmaker;
using EntityLibrary.Bookmaker.Sport;
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

        public BookmakerEventArbitrage GetArbitrage(BookmakerMergeEvent mergeEvent)
        {
            if (mergeEvent.Events == null || mergeEvent.Events.Count != 2)
                return null;

            // events
            var b1 = mergeEvent.Events.ElementAt(0);
            var b2 = mergeEvent.Events.ElementAt(1);

            // odds
            if (b1.OddsDictionary.ContainsKey(SportOddType.HomeWin) &&
                b2.OddsDictionary.ContainsKey(SportOddType.AwayNotLoose))
            {
                // 1 X2
                var c1 = b1.OddsDictionary[SportOddType.HomeWin];
                var c2 = b2.OddsDictionary[SportOddType.AwayNotLoose];

                var arbitrage = GetTwoResultSportArbitrage(new BookmakerEventOdd(c1), new BookmakerEventOdd(c2));
                if (arbitrage != null)
                {
                    return arbitrage;
                }
            }

            if (b1.OddsDictionary.ContainsKey(SportOddType.AwayWin) &&
                b2.OddsDictionary.ContainsKey(SportOddType.HomeNotLoose))
            {
                // 1X 2
                var c1 = b1.OddsDictionary[SportOddType.AwayWin];
                var c2 = b2.OddsDictionary[SportOddType.HomeNotLoose];

                var arbitrage = GetTwoResultSportArbitrage(new BookmakerEventOdd(c1), new BookmakerEventOdd(c2));
                if (arbitrage != null)
                {
                    return arbitrage;
                }
            }

            if (b1.OddsDictionary.ContainsKey(SportOddType.HomeNotLoose) &&
                b2.OddsDictionary.ContainsKey(SportOddType.AwayWin))
            {
                // 1X 2
                var c1 = b1.OddsDictionary[SportOddType.HomeNotLoose];
                var c2 = b2.OddsDictionary[SportOddType.AwayWin];

                var arbitrage = GetTwoResultSportArbitrage(new BookmakerEventOdd(c1), new BookmakerEventOdd(c2));
                if (arbitrage != null)
                {
                    return arbitrage;
                }
            }

            if (b1.OddsDictionary.ContainsKey(SportOddType.AwayNotLoose) &&
                b2.OddsDictionary.ContainsKey(SportOddType.HomeWin))
            {
                // 1X 2
                var c1 = b1.OddsDictionary[SportOddType.AwayNotLoose];
                var c2 = b2.OddsDictionary[SportOddType.HomeWin];

                var arbitrage = GetTwoResultSportArbitrage(new BookmakerEventOdd(c1), new BookmakerEventOdd(c2));
                if (arbitrage != null)
                {
                    return arbitrage;
                }
            }

            return null;
        }

        public BookmakerEventArbitrage GetThreeWayArbitrage(BookmakerEventOdd homeWin, BookmakerEventOdd draw,
            BookmakerEventOdd awayWin)
        {
            var profit = (1 / homeWin.Coefficient) + (1 / draw.Coefficient) + (1 / awayWin.Coefficient);
            return profit < 1 ? new BookmakerEventArbitrage(profit) : null;
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