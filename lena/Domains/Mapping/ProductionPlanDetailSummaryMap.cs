using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionPlanDetailSummaryMap : IEntityTypeConfiguration<ProductionPlanDetailSummary>
  {
    public void Configure(EntityTypeBuilder<ProductionPlanDetailSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionPlanDetailSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProducedQty);
      builder.Property(x => x.ScheduledQty);
      builder.Property(x => x.ProductionPlanDetailId);
      builder.HasRowVersion();
    }
  }
}