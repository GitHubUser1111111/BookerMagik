namespace EntityLibrary.Business.Arbitrage
{
    public class BookmakerEventArbitrage
    {
        public decimal Weight { get; }

        public decimal Profit => 100m - (Weight * 100);
        public BookmakerEventArbitrage(decimal weight)
        {
            Weight = weight;
        }
    }
}
