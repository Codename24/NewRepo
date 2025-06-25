using Microsoft.Extensions.Options;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Models;
using TaxCalculator.Infrastructure.Configuration;

namespace TaxCalculator.Domain.Services
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private readonly int monthsAmount = 12;
        private readonly List<TaxBand> _taxBands;
        public TaxCalculatorService(IOptions<TaxBandSettings> taxBandSettings)
        {
            var settings = taxBandSettings.Value;
            if (settings == null || settings.TaxBands.Count == 0)
            {
                throw new InvalidOperationException("TaxBands configuration is missing or empty.");
            }

            _taxBands = settings.TaxBands.OrderBy(b => b.LowerLimit).ToList();
        }

        public async Task<TaxResult> CalculateTax(int annualSalary)
        {
            int totalTax = 0;

            foreach (var band in _taxBands)
            {
                // Stop if the salary is less than the lower limit of the current band
                if (annualSalary <= band.LowerLimit)
                {
                    break;
                }

                // Calculate taxable income within the current band
                int taxableIncomeInBand = annualSalary - band.LowerLimit;

                // If the band has an upper limit, cap taxable income to the band limit
                if (band.UpperLimit.HasValue)
                {
                    taxableIncomeInBand = Math.Min(taxableIncomeInBand, band.UpperLimit.Value - band.LowerLimit);
                }

                // Calculate tax for the taxable income in the current band
                totalTax += (taxableIncomeInBand * band.TaxRate) / 100;
            }

            int netSalary = annualSalary - totalTax;

            return new TaxResult
            {
                GrossAnnualSalary = annualSalary,
                NetAnnualSalary = netSalary,
                GrossMonthlySalary = annualSalary / monthsAmount,
                NetMonthlySalary = netSalary / monthsAmount,
                TotalTax = totalTax,
                TotalMonthlyTaxes = totalTax / monthsAmount,
            };

        }

    }
}
