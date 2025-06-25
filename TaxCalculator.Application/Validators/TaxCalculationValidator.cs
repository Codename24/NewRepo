using FluentValidation;
using TaxCalculator.Application.Commands;

namespace TaxCalculator.Application.Validators
{
    public class TaxCalculationValidator : AbstractValidator<CalculateTaxCommand>
    {
        public TaxCalculationValidator()
        {
            RuleFor(x => x.AnnualSalary)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Annual salary cannot be negative.");

            // In case Annual salary would be greater then int.MaxValue / 2 we could face overflow exception during calculation
            RuleFor(x => x.AnnualSalary)
                .LessThanOrEqualTo(int.MaxValue / 2)
                .WithMessage($"Annual salary exceeds the maximum supported value ({int.MaxValue / 2}).");
        }
    }
}
