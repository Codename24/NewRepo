using TaxCalculator.Domain.Models;

namespace TaxCalculator.Domain.Interfaces
{
    public interface ITaxCalculatorService
    {
        Task<TaxResult> CalculateTax(int annualSalary);
    }
}
