namespace TaxCalculator.Domain.Models
{
    public class TaxBand
    {
        public int LowerLimit { get; set; }
        public int? UpperLimit { get; set; }
        public int TaxRate { get; set; }
    }
}
