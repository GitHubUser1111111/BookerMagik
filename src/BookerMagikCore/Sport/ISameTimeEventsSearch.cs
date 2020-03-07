using EntityLibrary.Bookmaker;
using EntityLibrary.Bookmaker.Sport;

namespace BookerMagikCore.Sport
{
    public interface ISameTimeEventsSearch
    {
        /// <summary>
        ///     Check if events is same
        /// </summary>
        /// <param name="a">sport event</param>
        /// <param name="b">sport event</param>
        /// <returns>same events</returns>
        bool CheckIsSameEvents(BookmakerTwoParticipantEvent a, BookmakerTwoParticipantEvent b);

        /// <summary>
        ///     Check if leagues is same
        /// </summary>
        /// <param name="a">sport league</param>
        /// <param name="b">sport league</param>
        /// <returns>same events</returns>
        bool CheckIsSameLeague(SportLeague a, SportLeague b);
    }
}