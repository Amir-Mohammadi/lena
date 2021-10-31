using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionScheduleSummaryMap : IEntityTypeConfiguration<ProductionScheduleSummary>
  {
    public void Configure(EntityTypeBuilder<ProductionScheduleSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionScheduleSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProducedQty);
      builder.Property(x => x.ProductionScheduleId);
      builder.HasRowVersion();
    }
  }
}