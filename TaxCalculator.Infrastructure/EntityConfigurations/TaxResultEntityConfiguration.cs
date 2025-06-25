using TaxCalculator.Infrastructure.Entities;

namespace TaxCalculator.Infrastructure.EntityConfigurations
{
    public class TaxResultEntityConfiguration : BaseEntityConfiguration<TaxResultEntity>
    {

        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TaxResultEntity> builder)
        {
            base.Configure(builder);
        }
    }
}
