using System.Collections.Generic;
using System.Linq;
using BookerMagikCore.Infrastructure;
using EntityLibrary.Abstract.Sport;

namespace BookerMagikCore.Sport
{
    public class SameTimeEventsSearch : ISameTimeEventsSearch
    {
        private readonly ISimilarStringsCalculator _similarStringsCalculator;
        private readonly ILongestCommonSubsequence _commonSubsequence;
        private readonly int threashold;

        private readonly IReadOnlyCollection<string> abbreviation = new[]
        {
            "fc", "bc"
        };

        public SameTimeEventsSearch(ISimilarStringsCalculator similarStringsCalculator, ILongestCommonSubsequence commonSubsequence)
        {
            _similarStringsCalculator = similarStringsCalculator;
            _commonSubsequence = commonSubsequence;
            threashold = 5;
        }

        public bool CheckIsSameEvents(SportEventAbstract a, SportEventAbstract b)
        {
            if (a.KindOfSport != b.KindOfSport)
                // dif by kind of sport
                return false;

            var t1 = a.StartTime.ToUniversalTime();
            var t2 = b.StartTime.ToUniversalTime();

            if (!t1.Equals(t2))
                // dif by time
                return false;

            var h1 = a.HomeTeam.Name.ToLower();
            var h2 = a.AwayTeam.Name.ToLower();
            var h11 = b.HomeTeam.Name.ToLower();
            var h22 = b.AwayTeam.Name.ToLower();


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

            int totalDistance = 0;

            // home
            for (int i = 0; i < h1count.Count; i++)
            {
                var d = CalculateDistance(h1count[i], h11count[i]);
                if (d >= threashold)
                    return false;

                totalDistance += d;
            }
            
            // away
            for (int i = 0; i < h1count.Count; i++)
            {
                var d = CalculateDistance(h2count[i], h22count[i]);
                if (d >= threashold)
                    return false;

                totalDistance += d;
            }
            
            return totalDistance < threashold;
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
            {
                // one of word is abbreviation
                return 1;
            }

            var distance = _similarStringsCalculator.DamerauLevenshteinDistance(a, b);
            return distance;
        }
    }
}