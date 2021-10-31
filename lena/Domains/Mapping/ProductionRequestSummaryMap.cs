using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionRequestSummaryMap : IEntityTypeConfiguration<ProductionRequestSummary>
  {
    public void Configure(EntityTypeBuilder<ProductionRequestSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionRequestSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.PlannedQty);
      builder.Property(x => x.ScheduledQty);
      builder.Property(x => x.ProducedQty);
      builder.Property(x => x.ProductionRequestId);
      builder.HasRowVersion();
    }
  }
}