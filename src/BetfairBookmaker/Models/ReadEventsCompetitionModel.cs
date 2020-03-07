using BetfairApi.TO;

namespace BetfairBookmaker.Models
{
    public class ReadEventsCompetitionModel
    {
        public string Competition { get; set; }

        public TimeRange EventTimeRange { get; set; }
    }
}