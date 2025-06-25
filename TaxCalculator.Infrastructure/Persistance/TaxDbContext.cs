using Microsoft.EntityFrameworkCore;
using TaxCalculator.Infrastructure.Entities;

namespace TaxCalculator.Infrastructure.Persistance
{
    public class TaxDbContext : DbContext
    {
        public TaxDbContext(DbContextOptions<TaxDbContext> options) : base(options) { }
        public DbSet<TaxResultEntity> TaxResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaxDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            SetAuditDates();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditDates();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetAuditDates()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                }
            }
        }
    }
}
