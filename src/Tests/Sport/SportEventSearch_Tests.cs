using System;
using BookerMagikCore.Infrastructure;
using BookerMagikCore.Sport;
using EntityLibrary.Business.Sport.Football;
using Xunit;

namespace Tests.Sport
{
    public class SportEventSearch_Tests
    {
        [Theory]
        [InlineData("home", "away", "home", "away")]
        [InlineData("Juventus", "Milan", "juventus", "milan")]
        [InlineData("Man United", "Real M", "Manchester Utd", "Real Madrid")]
        [InlineData("Chelsea FC", "Barcelona", "Chelsea", "fc Barcelona")]
        public void SearchSameEvents_Valid(string a, string b, string c, string d)
        {
            // Arrange
            ISimilarStringsCalculator similarStringsCalculator = new SimilarStringsCalculator();
            ILongestCommonSubsequence longestCommonSubsequence = new LongestCommonSubsequence();
            ISameTimeEventsSearch sameTimeEventsSearch =
                new SameTimeEventsSearch(similarStringsCalculator, longestCommonSubsequence);
            var sameTime = DateTime.UtcNow;
            var e1 = new FootballSportEvent(sameTime, new FootballTeam(a), new FootballTeam(b));
            var e2 = new FootballSportEvent(sameTime, new FootballTeam(c), new FootballTeam(d));

            // Act
            var isSame = sameTimeEventsSearch.CheckIsSameEvents(e1, e2);

            // Assert
            Assert.True(isSame);
        }
    }
}