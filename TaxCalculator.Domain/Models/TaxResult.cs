namespace TaxCalculator.Domain.Models
{
    public class TaxResult
    {
        public double GrossAnnualSalary { get; set; }
        public double NetAnnualSalary { get; set; }
        public double GrossMonthlySalary { get; set; }
        public double NetMonthlySalary { get; set; }
        public double TotalTax { get; set; }
        public double TotalMonthlyTaxes { get; set; }
    }
}
