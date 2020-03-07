using System;
using BookerMagikCore.Infrastructure;
using BookerMagikCore.Sport;
using EntityLibrary.Bookmaker;
using EntityLibrary.Bookmaker.Sport;
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
            var e1 = new BookmakerTwoParticipantSportEvent(sameTime, new BookmakerEventParticipant(a),
                new BookmakerEventParticipant(b));
            var e2 = new BookmakerTwoParticipantSportEvent(sameTime, new BookmakerEventParticipant(c),
                new BookmakerEventParticipant(d));

            // Act
            var isSame = sameTimeEventsSearch.CheckIsSameEvents(e1, e2);

            // Assert
            Assert.True(isSame);
        }
    }
}