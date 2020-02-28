using System;
using EntityLibrary.Business.Sport.Football;
using EntityLibrary.Business.Sport.Odd;
using EntityLibrary.Services.Arbitrage;
using Xunit;

namespace Tests.Sport
{
    public class SportEventArbitrage_Tests
    {
        public SportEventArbitrage_Tests()
        {
            
        }

        [Fact]
        public void GetArbitrage_Valid()
        {
            // Arrange
            var sportArbitrageService = new SportArbitrageService();
            var homeWin = new SportResultOdd(2.6m);
            var awayNotLoose = new SportResultOdd(1.79m);
            
            // Act
            var arbitrage = sportArbitrageService.GetTwoResultSportArbitrage(homeWin, awayNotLoose);

            // Assert
            Assert.NotNull(arbitrage);
            Assert.True(arbitrage.Weight < 1);
            Assert.True(arbitrage.Profit > 0);
        }
    }
}