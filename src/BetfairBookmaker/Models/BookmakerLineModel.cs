using System.Collections.Generic;
using System.Linq;
using BetfairApi.TO;
using EntityLibrary.Bookmaker;

namespace BetfairBookmaker.Models
{
    public class BookmakerLineModel : BookmakerLine
    {
        private readonly IDictionary<string, TwoParticipantSportEventModel> _eventsDictionary =
            new Dictionary<string, TwoParticipantSportEventModel>();

        public IReadOnlyCollection<string> EventIds => _eventsDictionary.Keys.ToList().AsReadOnly();

        public int UpdateEvents(IEnumerable<EventResult> eventResults)
        {
            foreach (var sportEvent in eventResults.Where(x => x.Event.OpenDate.HasValue))
            {
                var name = sportEvent.Event.Name;
                if (!name.Contains(" v "))
                    continue;

                var teams = name.Split(" v ");
                if (teams.Length != 2)
                    continue;

                if (_eventsDictionary.ContainsKey(sportEvent.Event.Id))
                    // event is already exist
                    continue;

                var bookmakerTwoParticipantSportEvent =
                    new TwoParticipantSportEventModel(sportEvent, teams[0].Trim(), teams[1].Trim());

                _eventsDictionary.Add(sportEvent.Event.Id, bookmakerTwoParticipantSportEvent);
            }

            return _eventsDictionary.Count;
        }

        public void UpdateOdds(string eventId, MarketCatalogue matchOddsCatalogue, MarketBook matchOddsEvent)
        {
            _eventsDictionary[eventId].UpdateMatchOdds(matchOddsCatalogue, matchOddsEvent);
        }

        public override int EventsCount => _eventsDictionary.Count;
        public override IReadOnlyCollection<BookmakerTwoParticipantEvent> SportEvents => _eventsDictionary.Values.ToList().AsReadOnly();
    }
}