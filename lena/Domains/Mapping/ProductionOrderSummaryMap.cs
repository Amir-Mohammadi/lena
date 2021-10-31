using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionOrderSummaryMap : IEntityTypeConfiguration<ProductionOrderSummary>
  {
    public void Configure(EntityTypeBuilder<ProductionOrderSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionOrderSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProducedQty);
      builder.Property(x => x.InProductionQty);
      builder.Property(x => x.Description);
      builder.Property(x => x.ProductionOrderId);
      builder.HasRowVersion();
    }
  }
}