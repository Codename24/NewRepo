using MediatR;
using TaxCalculator.Application.Commands;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Models;

namespace TaxCalculator.Application.Handlers
{
    /// <summary>
    /// Command Handler for Performing Tax Calculation
    /// </summary>
    public class CalculateTaxHandler : IRequestHandler<CalculateTaxCommand, TaxResult>
    {
        private readonly ITaxCalculatorService _taxCalculatorService;

        public CalculateTaxHandler(ITaxCalculatorService taxCalculatorService)
        {
            _taxCalculatorService = taxCalculatorService;
        }

        public async Task<TaxResult> Handle(CalculateTaxCommand request, CancellationToken cancellationToken)
        {
            var taxResult = await _taxCalculatorService.CalculateTax(request.AnnualSalary);
            return taxResult;
        }
    }
}
