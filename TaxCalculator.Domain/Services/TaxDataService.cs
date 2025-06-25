using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Models;
using TaxCalculator.Infrastructure.Entities;
using TaxCalculator.Infrastructure.Persistance;

namespace TaxCalculator.Domain.Services
{
    public class TaxDataService : ITaxDataService
    {
        private readonly TaxDbContext _dbContext;
        private readonly IMapper _mapper;

        public TaxDataService(
            TaxDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task SaveResultAsync(TaxResult result)
        {
            var taxResultEntity = _mapper.Map<TaxResultEntity>(result);
            await _dbContext.TaxResults.AddAsync(taxResultEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaxResult>> GetSpecifiedAmountOfRecords(int amount, CancellationToken cancellationToken)
        {
            var result = await _dbContext
                .TaxResults
                .OrderByDescending(tr => tr.CreatedDate)
                .Take(amount)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<TaxResult>>(result);
        }
    }
}
