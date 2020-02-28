using EntityLibrary.Abstract.Sport;

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
        bool CheckIsSameEvents(SportEventAbstract a, SportEventAbstract b);
    }
}