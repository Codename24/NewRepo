using FluentAssertions;
using FluentValidation;
using Moq;
using TaxCalculator.Application.Commands;
using TaxCalculator.Application.Handlers;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Models;

namespace TaxCalculator.Application.Tests
{
    public class CalculateTaxHandlerTests
    {
        private readonly Mock<ITaxCalculatorService> _mockTaxCalculatorService;
        private readonly CalculateTaxHandler _handler;
        private readonly IValidator<CalculateTaxCommand> _validator;

        public CalculateTaxHandlerTests()
        {
            // Mock the TaxCalculatorService
            _mockTaxCalculatorService = new Mock<ITaxCalculatorService>();

            // Mock response for valid tax calculation
            _mockTaxCalculatorService
                .Setup(s => s.CalculateTax(It.IsAny<int>()))
                .ReturnsAsync(new TaxResult
                {
                    GrossAnnualSalary = 50000,
                    NetAnnualSalary = 40000,
                    TotalTax = 10000,
                    GrossMonthlySalary = 4166,
                    NetMonthlySalary = 3333,
                    TotalMonthlyTaxes = 833,
                });

            // Initialize the handler
            _handler = new CalculateTaxHandler(
                _mockTaxCalculatorService.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnTaxResult_ForValidRequest()
        {
            // Arrange
            var request = new CalculateTaxCommand
            {
                AnnualSalary = 50000
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.GrossAnnualSalary.Should().Be(50000);
            result.NetAnnualSalary.Should().Be(40000);
            result.TotalTax.Should().Be(10000);
        }

        [Fact]
        public void Handle_ShouldThrowArgumentException_ForNegativeSalary()
        {
            // Arrange
            var request = new CalculateTaxCommand
            {
                AnnualSalary = -10000
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Annual salary cannot be negative.");
        }

        [Fact]
        public void Handle_ShouldThrowArgumentException_ForSalaryExceedingMaxValue()
        {
            // Arrange
            var request = new CalculateTaxCommand
            {
                AnnualSalary = int.MaxValue
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ArgumentException>()
                .WithMessage($"Annual salary exceeds the maximum supported value ({int.MaxValue / 2}).");
        }
    }
}
