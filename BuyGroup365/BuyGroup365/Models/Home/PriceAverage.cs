namespace BuyGroup365.Models.Home
{
    public class PriceAverage
    {
        public PriceAverage()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string PriceKey { get; set; }
        public string PriceTile { get; set; }
        public double PriceFrom { get; set; }
        public double PriceTo { get; set; }

        public PriceAverage(string PriceKey, string PriceTile, double PriceFrom, double PriceTo)
        {
            this.PriceKey = PriceKey;
            this.PriceTile = PriceTile;
            this.PriceFrom = PriceFrom;
            this.PriceTo = PriceTo;
        }
    }
}