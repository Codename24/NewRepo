using MediatR;
using TaxCalculator.Domain.Models;

namespace TaxCalculator.Application.Commands
{
    public class SaveTaxResultCommand : IRequest<bool>
    {
        public TaxResult TaxResult { get; set; }
    }
}
