using System;
using System.Collections.Generic;
using System.Text;
using BookerMagikCore.Common.EventArguments;
using BookerMagikCore.Services.Arbitrage;
using BookerMagikCore.Sport;
using BookerMagikCore.Strategies;
using EntityLibrary.Bookmaker;

namespace BookerMagikCore.Bookmaker
{
    public class GlobalBookmaker : IGlobalBookmaker
    {
        private readonly ISameTimeEventsSearch _sameTimeEventsSearch;
        private readonly ISportArbitrageService _sportArbitrageService;

        private readonly List<IBookmaker> _bookmakers = new List<IBookmaker>();

        private readonly object lineLock = new object();
        private readonly BookmakerGlobalLine _globalLine = new BookmakerGlobalLine();

        public GlobalBookmaker(ISameTimeEventsSearch sameTimeEventsSearch, ISportArbitrageService sportArbitrageService)
        {
            _sameTimeEventsSearch = sameTimeEventsSearch;
            _sportArbitrageService = sportArbitrageService;
        }

        public int RegisterBookmaker(IBookmaker bookmaker)
        {
            _bookmakers.Add(bookmaker);
            bookmaker.LineUpdated += Bookmaker_LineUpdated;
            bookmaker.StartReadLineThread();
            return _bookmakers.Count;
        }

        private void Bookmaker_LineUpdated(object sender, LineUpdatedEventArgs e)
        {
            var bookmaker = (IBookmaker) sender;
            Console.WriteLine($"{bookmaker.Name}: Events count = {e.Line.EventsCount}");

            // add to merge line
            UpdateEvents(bookmaker.Name, e.Line.SportEvents);

            // get merged
            var merged = GetBookmakerMergeEvents();

            if (merged.Count == 0)
                return;

            // check if has arbitrage
            FindArbitrage(merged);
        }

        public void UpdateEvents(string bookmaker, IEnumerable<BookmakerTwoParticipantEvent> events)
        {
            lock (lineLock)
            {
                foreach (var bookmakerEvent in events)
                {
                    TryLinkEvent(bookmakerEvent);
                }
            }
        }

        private void FindArbitrage(List<BookmakerMergeEvent> events)
        {
            foreach (var bookmakerMergeEvent in events)
            {
                var arbitrage = _sportArbitrageService.GetArbitrage(bookmakerMergeEvent);
                if (arbitrage != null)
                {
                    var profit = arbitrage.Profit;
                }
            }
        }

        private List<BookmakerMergeEvent> GetBookmakerMergeEvents()
        {
            lock (lineLock)
            {
                return _globalLine.GetBookmakerMergeEvents();
            }
        }

        private void TryLinkEvent(BookmakerTwoParticipantEvent bookmakerEvent)
        {
            if (bookmakerEvent.IsLinked)
            {
                // event is already linked
                return;
            }

            // check if same event is already exist in line
            if (TryGetMergeEvent(bookmakerEvent))
            {
                return;
            }

            // new merge event
            RegisterNewMergeEvent(bookmakerEvent);
        }

        private bool TryGetMergeEvent(BookmakerTwoParticipantEvent bookmakerEvent)
        {
            bool isLinked = false;

            foreach (var pair in _globalLine.Events)
            {
                foreach (var globalEvent in pair.Value.Events)
                {
                    if (!_sameTimeEventsSearch.CheckIsSameEvents(bookmakerEvent, globalEvent)) continue;

                    // link
                    bookmakerEvent.AttachGlobalLineGuid(pair.Key);
                    pair.Value.AddEvent(bookmakerEvent);
                    isLinked = true;
                    break;
                }

                if (isLinked)
                    break;
            }

            return isLinked;
        }

        private void RegisterNewMergeEvent(BookmakerTwoParticipantEvent bookmakerEvent)
        {
            Guid newGuid = Guid.NewGuid();
            var newMergeEvent = new BookmakerMergeEvent();
            bookmakerEvent.AttachGlobalLineGuid(newGuid);
            newMergeEvent.AddEvent(bookmakerEvent);
            _globalLine.AddNewBookmakerMergeEvent(newGuid, newMergeEvent);
        }
    }
}
