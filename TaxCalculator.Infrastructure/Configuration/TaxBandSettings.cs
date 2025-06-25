namespace TaxCalculator.Infrastructure.Configuration
{
    public record TaxBand(
        int LowerLimit,
        int? UpperLimit,
        int TaxRate
    );

    public class TaxBandSettings
    {
        public List<TaxBand> TaxBands { get; set; } = new List<TaxBand>();
    }
}
