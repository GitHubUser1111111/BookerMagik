using BookerMagikCore.Services.Arbitrage;
using EntityLibrary.Bookmaker;
using Xunit;

namespace Tests.Sport
{
    public class SportEventArbitrage_Tests
    {
        [Fact]
        public void GetArbitrage_Valid()
        {
            // Arrange
            var sportArbitrageService = new SportArbitrageService();
            var homeWin = new BookmakerEventOdd(2.6m);
            var awayNotLoose = new BookmakerEventOdd(1.79m);

            // Act
            var arbitrage = sportArbitrageService.GetTwoResultSportArbitrage(homeWin, awayNotLoose);

            // Assert
            Assert.NotNull(arbitrage);
            Assert.True(arbitrage.Weight < 1);
            Assert.True(arbitrage.Profit > 0);
        }
    }
}