using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using TaxCalculator.Domain.Services;
using TaxCalculator.Infrastructure.Configuration;

namespace TaxCalculator.Domain.Tests.Services
{
    public class TaxCalculatorServiceTests
    {
        private readonly Mock<IOptions<TaxBandSettings>> _mockTaxBandSettings;
        private readonly TaxCalculatorService _taxCalculatorService;
        private readonly int monthsAmount = 12;

        public TaxCalculatorServiceTests()
        {
            // Mock the TaxBandSettings
            _mockTaxBandSettings = new Mock<IOptions<TaxBandSettings>>();

            var taxBands = new List<TaxBand>
            {
                new TaxBand(0, 5000, 0),    
                new TaxBand(5000, 20000, 20),
                new TaxBand(20000, null, 40)
            };

            var taxBandSettings = new TaxBandSettings { TaxBands = taxBands };
            _mockTaxBandSettings
                .Setup(x => x.Value)
                .Returns(taxBandSettings);

            // Initialize the service
            _taxCalculatorService = new TaxCalculatorService(_mockTaxBandSettings.Object);
        }

        [Fact]
        public async Task CalculateTax_ShouldReturnCorrectResult_WhenWithinFirstTaxBand()
        {
            // Arrange
            int annualSalary = 4000;

            // Act
            var result = await _taxCalculatorService.CalculateTax(annualSalary);

            // Assert
            result.Should().NotBeNull();
            result.GrossAnnualSalary.Should().Be(4000);
            result.NetAnnualSalary.Should().Be(4000);
            result.TotalTax.Should().Be(0);
            result.GrossMonthlySalary.Should().Be(4000 / monthsAmount);
            result.NetMonthlySalary.Should().Be(4000 / monthsAmount);
        }

        [Fact]
        public async Task CalculateTax_ShouldReturnCorrectResult_WhenWithinSecondTaxBand()
        {
            // Arrange
            int annualSalary = 15000;

            // Act
            var result = await _taxCalculatorService.CalculateTax(annualSalary);

            // Assert
            result.Should().NotBeNull();
            result.GrossAnnualSalary.Should().Be(15000);
            result.NetAnnualSalary.Should().Be(13000);
            result.TotalTax.Should().Be(2000);
            result.GrossMonthlySalary.Should().Be(15000 / monthsAmount);
            result.NetMonthlySalary.Should().Be(13000 / monthsAmount);
            result.TotalMonthlyTaxes.Should().Be(2000 / monthsAmount);
        }

        [Fact]
        public async Task CalculateTax_ShouldReturnCorrectResult_WhenWithinThirdTaxBand()
        {
            // Arrange
            int annualSalary = 50000;

            // Act
            var result = await _taxCalculatorService.CalculateTax(annualSalary);

            // Assert
            result.Should().NotBeNull();
            result.GrossAnnualSalary.Should().Be(50000);
            result.NetAnnualSalary.Should().Be(35000);
            result.TotalTax.Should().Be(15000);
            result.GrossMonthlySalary.Should().Be(50000 / monthsAmount);
            result.NetMonthlySalary.Should().Be(35000 / monthsAmount);
            result.TotalMonthlyTaxes.Should().Be(15000 / monthsAmount);
        }

        [Fact]
        public async Task CalculateTax_ShouldReturnCorrectResult_ForBoundaryValueAtSecondBand()
        {
            // Arrange
            int annualSalary = 20000;

            // Act
            var result = await _taxCalculatorService.CalculateTax(annualSalary);

            // Assert
            result.Should().NotBeNull();
            result.GrossAnnualSalary.Should().Be(20000);
            result.NetAnnualSalary.Should().Be(17000);
            result.TotalTax.Should().Be(3000);
            result.GrossMonthlySalary.Should().Be(20000 / monthsAmount);
            result.NetMonthlySalary.Should().Be(17000 / monthsAmount);
            result.TotalMonthlyTaxes.Should().Be(3000 / monthsAmount);
        }

        [Fact]
        public void TaxCalculatorService_ShouldThrowException_WhenTaxBandsAreMissing()
        {
            // Arrange
            var mockEmptySettings = new Mock<IOptions<TaxBandSettings>>();
            mockEmptySettings
                .Setup(x => x.Value)
                .Returns(new TaxBandSettings { TaxBands = new List<TaxBand>() });

            // Act
            var act = () => new TaxCalculatorService(mockEmptySettings.Object);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("TaxBands configuration is missing or empty.");
        }
    }
}
