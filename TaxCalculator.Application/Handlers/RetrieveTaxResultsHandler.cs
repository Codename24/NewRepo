using MediatR;
using TaxCalculator.Application.Queries;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Models;

namespace TaxCalculator.Application.Handlers
{
    public class RetrieveTaxResultsHandler : IRequestHandler<RetrieveTaxResultsQuery, IEnumerable<TaxResult>>
    {
        private readonly ITaxDataService _taxDataService;
        public RetrieveTaxResultsHandler(ITaxDataService taxDataService) {
            _taxDataService = taxDataService;
        }
        public async Task<IEnumerable<TaxResult>> Handle(RetrieveTaxResultsQuery request, CancellationToken cancellationToken)
        {
            return await _taxDataService.GetSpecifiedAmountOfRecords(request.Amount, cancellationToken);
        }
    }

}
