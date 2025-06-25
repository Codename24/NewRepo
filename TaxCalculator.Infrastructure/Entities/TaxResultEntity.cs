namespace TaxCalculator.Infrastructure.Entities
{
    public class TaxResultEntity : BaseEntity
    {
        public int GrossAnnualSalary { get; set; }
        public int NetAnnualSalary { get; set; }
        public int GrossMonthlySalary { get; set; }
        public int NetMonthlySalary { get; set; }
        public int TotalTax { get; set; }
    }
}
