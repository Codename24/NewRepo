using MediatR;
using Microsoft.Extensions.Logging;
using TaxCalculator.Application.Commands;
using TaxCalculator.Domain.Interfaces;

namespace TaxCalculator.Application.Handlers
{
    public class SaveTaxResultHandler : IRequestHandler<SaveTaxResultCommand, bool>
    {
        private readonly ITaxDataService _taxDataService;
        private readonly ILogger<SaveTaxResultHandler> _logger;
        public SaveTaxResultHandler(ITaxDataService taxDataService, ILogger<SaveTaxResultHandler> logger)
        {
            _taxDataService = taxDataService;
            _logger = logger;
        }

        public async Task<bool> Handle(SaveTaxResultCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _taxDataService.SaveResultAsync(request.TaxResult);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save TaxResult for AnnualSalary: {AnnualSalary}", request.TaxResult.GrossAnnualSalary);
                throw new InvalidOperationException("An error occurred while saving tax result. See inner exception for details.", ex);
            }
        }
    }
}
