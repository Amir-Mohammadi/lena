using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseOrderSummaryMap : IEntityTypeConfiguration<PurchaseOrderSummary>
  {
    public void Configure(EntityTypeBuilder<PurchaseOrderSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PurchaseOrderSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.CargoedQty);
      builder.Property(x => x.ReceiptedQty);
      builder.Property(x => x.QualityControlPassedQty);
      builder.Property(x => x.QualityControlFailedQty);
      builder.Property(x => x.PurchaseOrderId);
      builder.HasRowVersion();
      builder.HasOne(x => x.PurchaseOrder).WithOne(x => x.PurchaseOrderSummary).HasForeignKey<PurchaseOrderSummary>(x => x.PurchaseOrderId);
    }
  }
}