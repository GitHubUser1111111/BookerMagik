namespace EntityLibrary.Bookmaker.Sport.Odd
{
    public class SportOdd
    {
        public OddType OddType { get; }
        public decimal Coefficient { get; }

        public SportOdd(OddType oddType, decimal coefficient)
        {
            OddType = oddType;
            Coefficient = coefficient;
        }
    }
}
