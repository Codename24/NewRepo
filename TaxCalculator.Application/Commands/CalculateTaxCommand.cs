using MediatR;
using TaxCalculator.Domain.Models;

namespace TaxCalculator.Application.Commands
{
    public class CalculateTaxCommand : IRequest<TaxResult>
    {
        public int AnnualSalary { get; set; }
    }
}
