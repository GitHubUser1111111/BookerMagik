namespace BookerMagikCore.Infrastructure
{
    public interface ILongestCommonSubsequence
    {
        /// <summary>
        ///     Find subsequence
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        string Find(string a, string b);
    }
}