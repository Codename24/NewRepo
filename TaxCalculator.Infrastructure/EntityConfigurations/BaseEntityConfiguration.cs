using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TaxCalculator.Infrastructure.Entities;

namespace TaxCalculator.Infrastructure.EntityConfigurations
{
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.CreatedDate)
                   .IsRequired();

            builder.Property(e => e.UpdatedDate)
                   .IsRequired();                   
        }
    }
}
