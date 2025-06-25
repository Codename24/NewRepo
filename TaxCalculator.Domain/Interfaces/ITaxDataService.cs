using TaxCalculator.Domain.Models;

namespace TaxCalculator.Domain.Interfaces
{
    public interface ITaxDataService
    {
        Task SaveResultAsync(TaxResult result);
        Task<IEnumerable<TaxResult>> GetSpecifiedAmountOfRecords(int amount, CancellationToken cancellationToken);
    }
}
