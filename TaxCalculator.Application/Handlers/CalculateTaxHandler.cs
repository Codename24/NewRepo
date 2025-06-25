using FluentValidation;
using MediatR;
using TaxCalculator.Application.Commands;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaxCalculator.Application.Handlers
{
    /// <summary>
    /// Command Handler for Performing Tax Calculation
    /// </summary>
    public class CalculateTaxHandler : IRequestHandler<CalculateTaxCommand, TaxResult>
    {
        private readonly ITaxCalculatorService _taxCalculatorService;
        private readonly IValidator<CalculateTaxCommand> _validator;
        //Suggested improvement to the system is to move Validation logic into ValidationBehaviour and handle it on middleware level

        public CalculateTaxHandler(
            ITaxCalculatorService taxCalculatorService,
            IValidator<CalculateTaxCommand> validator)
        {
            _taxCalculatorService = taxCalculatorService;
            _validator = validator;
        }

        public async Task<TaxResult> Handle(CalculateTaxCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);
            var taxResult = await _taxCalculatorService.CalculateTax(request.AnnualSalary);
            return taxResult;
        }
    }
}
