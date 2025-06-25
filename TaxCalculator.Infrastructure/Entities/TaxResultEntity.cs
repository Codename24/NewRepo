namespace TaxCalculator.Infrastructure.Entities
{
    public class TaxResultEntity : BaseEntity
    {
        public double GrossAnnualSalary { get; set; }
        public double NetAnnualSalary { get; set; }
        public double GrossMonthlySalary { get; set; }
        public double NetMonthlySalary { get; set; }
        public double TotalTax { get; set; }
    }
}
