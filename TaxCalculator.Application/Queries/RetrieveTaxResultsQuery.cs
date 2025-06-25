using MediatR;
using TaxCalculator.Domain.Models;

namespace TaxCalculator.Application.Queries
{
    public class RetrieveTaxResultsQuery : IRequest<IEnumerable<TaxResult>>
    {
        public int Amount { get; set; }
    }
}
