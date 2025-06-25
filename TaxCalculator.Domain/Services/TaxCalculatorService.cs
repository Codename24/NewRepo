using Microsoft.Extensions.Options;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Models;
using TaxCalculator.Infrastructure.Configuration;

namespace TaxCalculator.Domain.Services
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private readonly List<TaxBand> _taxBands;
        public TaxCalculatorService(IOptions<TaxBandSettings> taxBandSettings)
        {
            _taxBands = taxBandSettings.Value.TaxBands ?? throw new ArgumentNullException(nameof(taxBandSettings));
        }

        public async Task<TaxResult> CalculateTax(int annualSalary)
        {
            int totalTax = 0;

            foreach (var band in _taxBands)
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
