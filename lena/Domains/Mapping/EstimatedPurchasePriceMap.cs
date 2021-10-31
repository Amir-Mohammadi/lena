using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EstimatedPurchasePriceMap : IEntityTypeConfiguration<EstimatedPurchasePrice>
  {
    public void Configure(EntityTypeBuilder<EstimatedPurchasePrice> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_EstimatedPurchasePrice");
      builder.Property(x => x.Id);
      builder.Property(x => x.PurchaseOrderId);
      builder.HasOne(x => x.PurchaseOrder).WithMany(x => x.EstimatedPurchasePrices).HasForeignKey(x => x.PurchaseOrderId);
    }
  }
}