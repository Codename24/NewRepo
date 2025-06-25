namespace TaxCalculator.Infrastructure.Configuration
{
    //public record TaxBand(
    //    int LowerLimit,
    //    int? UpperLimit,
    //    int TaxRate
    //);
    public class TaxBand
    {
        public int LowerLimit { get; set; }
        public int? UpperLimit { get; set; }
        public int TaxRate { get; set; }
    }
    public class TaxBandSettings
    {
        public List<TaxBand> TaxBands { get; set; } = new List<TaxBand>();
    }
}
