using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionPlanSummaryMap : IEntityTypeConfiguration<ProductionPlanSummary>
  {
    public void Configure(EntityTypeBuilder<ProductionPlanSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionPlanSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProducedQty);
      builder.Property(x => x.ScheduledQty);
      builder.Property(x => x.ProductionPlanId);
      builder.HasRowVersion();
    }
  }
}