using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Domain.Models
{
    public class TaxResult
    {
        public double GrossAnnualSalary { get; set; }
        public double NetAnnualSalary { get; set; }
        public double GrossMonthlySalary { get; set; }
        public double NetMonthlySalary { get; set; }
        public double TotalTax { get; set; }
    }
}
