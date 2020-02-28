namespace BookerMagikCore.Infrastructure
{
    public interface ISimilarStringsCalculator
    {
        /// <summary>
        ///     Calculate Damerau-Levenshtein distance
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>distance</returns>
        int DamerauLevenshteinDistance(string a, string b);
    }
}