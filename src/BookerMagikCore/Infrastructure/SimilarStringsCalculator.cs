namespace BookerMagikCore.Infrastructure
{
    public class SimilarStringsCalculator : ISimilarStringsCalculator
    {
        public int DamerauLevenshteinDistance(string firstText, string secondText)
        {
            var n = firstText.Length + 1;
            var m = secondText.Length + 1;
            var arrayD = new int[n, m];

            for (var i = 0; i < n; i++) arrayD[i, 0] = i;

            for (var j = 0; j < m; j++) arrayD[0, j] = j;

            for (var i = 1; i < n; i++)
            for (var j = 1; j < m; j++)
            {
                var cost = firstText[i - 1] == secondText[j - 1] ? 0 : 1;

                arrayD[i, j] = Minimum(arrayD[i - 1, j] + 1, // delete
                    arrayD[i, j - 1] + 1, // insert
                    arrayD[i - 1, j - 1] + cost); // change

                if (i > 1 && j > 1
                          && firstText[i - 1] == secondText[j - 2]
                          && firstText[i - 2] == secondText[j - 1])
                    arrayD[i, j] = Minimum(arrayD[i, j],
                        arrayD[i - 2, j - 2] + cost); // change
            }

            return arrayD[n - 1, m - 1];
        }

        private int Minimum(int a, int b)
        {
            return a < b ? a : b;
        }

        private int Minimum(int a, int b, int c)
        {
            return (a = a < b ? a : b) < c ? a : c;
        }
    }
}