using System.Collections.Generic;
using System.Linq;
using EntityLibrary.Bookmaker;
using PinnacleWrapper.Data;

namespace PinnacleBookmaker.Models
{
    public class BookmakerLineModel : BookmakerLine
    {
        private readonly IDictionary<long, TwoParticipantSportEventModel> _eventsDictionary =
            new Dictionary<long, TwoParticipantSportEventModel>();
        
        public int UpdateEvents(GetFixturesResponse fixtures)
        {
            foreach (var fixturesEvent in fixtures.Leagues.SelectMany(x => x.Events).Where(x => x.ParentId == 0))
            {
                if (_eventsDictionary.ContainsKey(fixturesEvent.Id))
                {
                    // event already contains
                    continue;
                }

                _eventsDictionary.Add(new KeyValuePair<long, TwoParticipantSportEventModel>(fixturesEvent.Id,
                    new TwoParticipantSportEventModel(fixturesEvent)));
            }

            return _eventsDictionary.Count;
        }

        public void UpdateOdds(GetOddsResponse oddsResponse)
        {
            foreach (var oddsEvent in oddsResponse.Leagues.SelectMany(x => x.Events))
            {
                if (!_eventsDictionary.ContainsKey(oddsEvent.Id))
                {
                    // not exist event
                    continue;
                }

                var eventModel = _eventsDictionary[oddsEvent.Id];
                if (oddsEvent.Periods.Count < 2)
                    continue;

                var fullTimePeriod = oddsEvent.Periods[1];
                eventModel.UpdateOdds(fullTimePeriod.MoneyLine);
            }
        }

        public override int EventsCount => _eventsDictionary.Count;

        public override IReadOnlyCollection<BookmakerTwoParticipantEvent> SportEvents =>
            _eventsDictionary.Values.ToList().AsReadOnly();
    }
}
