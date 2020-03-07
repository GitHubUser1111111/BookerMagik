using System.Collections.Generic;
using System.Linq;
using BookerMagikCore.Infrastructure;
using EntityLibrary.Bookmaker;
using EntityLibrary.Bookmaker.Sport;

namespace BookerMagikCore.Sport
{
    public class SameTimeEventsSearch : ISameTimeEventsSearch
    {
        private readonly ILongestCommonSubsequence _commonSubsequence;
        private readonly ISimilarStringsCalculator _similarStringsCalculator;

        private readonly IReadOnlyCollection<string> abbreviation = new[]
        {
            "fc", "bc"
        };

        private readonly int threashold;

        public SameTimeEventsSearch(ISimilarStringsCalculator similarStringsCalculator,
            ILongestCommonSubsequence commonSubsequence)
        {
            _similarStringsCalculator = similarStringsCalculator;
            _commonSubsequence = commonSubsequence;
            threashold = 5;
        }

        public bool CheckIsSameEvents(BookmakerTwoParticipantEvent a, BookmakerTwoParticipantEvent b)
        {
            var t1 = a.StartTime.ToUniversalTime();
            var t2 = b.StartTime.ToUniversalTime();

            if (!t1.Equals(t2))
                // dif by time
                return false;

            var h1 = a.HomeTeam.Name.ToLower();
            var h2 = a.AwayTeam.Name.ToLower();
            var h11 = b.HomeTeam.Name.ToLower();
            var h22 = b.AwayTeam.Name.ToLower();

            // first N symbols
            return h1.StartsWith(h11.Substring(0, 3)) && h2.StartsWith(h22.Substring(0, 3));


            if (h1.Equals(h11) && h2.Equals(h22))
                // time and team equals
                return true;

            // words count
            var h1count = h1.Split().ToList();
            var h11count = h11.Split().ToList();

            // trim abbreviation
            h1count.RemoveAll(x => abbreviation.Contains(x));
            h11count.RemoveAll(x => abbreviation.Contains(x));
            if (h1count.Count != h11count.Count)
                return false;

            var h2count = h2.Split().ToList();
            var h22count = h22.Split().ToList();

            // trim abbreviation
            h2count.RemoveAll(x => abbreviation.Contains(x));
            h22count.RemoveAll(x => abbreviation.Contains(x));
            if (h2count.Count != h22count.Count)
                return false;

            var totalDistance = 0;

            // home
            for (var i = 0; i < h1count.Count; i++)
            {
                var d = CalculateDistance(h1count[i], h11count[i]);
                if (d >= threashold)
                    return false;

                totalDistance += d;
            }

            // away
            for (var i = 0; i < h2count.Count; i++)
            {
                var d = CalculateDistance(h2count[i], h22count[i]);
                if (d >= threashold)
                    return false;

                totalDistance += d;
            }

            return totalDistance < threashold;
        }

        public bool CheckIsSameLeague(SportLeague a, SportLeague b)
        {
            var splitsA = a.Name.Split(' ', '-').ToList();
            splitsA.RemoveAll(string.IsNullOrWhiteSpace);
            var splitsB = b.Name.Split(' ', '-').ToList();
            splitsB.RemoveAll(string.IsNullOrWhiteSpace);
            if (splitsA.Count != splitsB.Count) return false;

            for (var i = 0; i < splitsA.Count; i++)
            {
                if (splitsA[i].Length < 3 || splitsB[i].Length < 3)
                    return false;

                if (splitsA[i] != splitsB[i] && !splitsA[i].StartsWith(splitsB[i].Substring(0, 3)))
                    return false;
            }

            return true;
        }

        private int CalculateDistance(string a, string b)
        {
            var sub = _commonSubsequence.Find(a, b);
            if (string.IsNullOrWhiteSpace(sub))
                return threashold;

            if (a.Length < b.Length)
            {
                if (!a.StartsWith(sub))
                    return threashold;
            }
            else
            {
                if (!b.StartsWith(sub))
                    return threashold;
            }

            var c = a.TrimStart(sub.ToCharArray());
            var d = b.TrimStart(sub.ToCharArray());

            if (string.IsNullOrWhiteSpace(c) || string.IsNullOrWhiteSpace(d))
                // one of word is abbreviation
                return 1;

            var distance = _similarStringsCalculator.DamerauLevenshteinDistance(a, b);
            return distance;
        }
    }
}