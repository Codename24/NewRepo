using MediatR;
using TaxCalculator.Domain.Models;

namespace TaxCalculator.Application.Queries
{
    public class RetrieveTaxResultsQuery : IRequest<List<TaxResult>>
    {
        public int Amount { get; set; }
    }
}
