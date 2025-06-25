using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Models;

namespace TaxCalculator.Domain.Services
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private static readonly List<TaxBand> TaxBands = new()
        {
            new TaxBand { LowerLimit = 0, UpperLimit = 5000, TaxRate = 0 },
            new TaxBand { LowerLimit = 5000, UpperLimit = 20000, TaxRate = 20 },
            new TaxBand { LowerLimit = 20000, UpperLimit = null, TaxRate = 40 }
        };

        public async Task<TaxResult> CalculateTax(int annualSalary)
        {
            int totalTax = 0;

            foreach (var band in TaxBands)
            {
                if (annualSalary < band.LowerLimit) break;

                var taxableIncomeInBand = Math.Min(
                    annualSalary - band.LowerLimit,
                    band.UpperLimit.HasValue ? band.UpperLimit.Value - band.LowerLimit : int.MaxValue
                );

                totalTax += (taxableIncomeInBand * band.TaxRate) / 100;
            }

            int netSalary = annualSalary - totalTax;

            return new TaxResult
            {
                GrossAnnualSalary = annualSalary,
                NetAnnualSalary = netSalary,
                GrossMonthlySalary = annualSalary / 12,
                NetMonthlySalary = netSalary / 12,
                TotalTax = totalTax
            };
        }

    }
}
