using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseOrderDetailSummaryMap : IEntityTypeConfiguration<PurchaseOrderDetailSummary>
  {
    public void Configure(EntityTypeBuilder<PurchaseOrderDetailSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PurchaseOrderDetailSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.CargoedQty);
      builder.Property(x => x.ReceiptedQty);
      builder.Property(x => x.QualityControlPassedQty);
      builder.Property(x => x.QualityControlFailedQty);
      builder.Property(x => x.PurchaseOrderDetailId);
      builder.HasRowVersion();
      builder.HasOne(x => x.PurchaseOrderDetail).WithOne(x => x.PurchaseOrderDetailSummary).HasForeignKey<PurchaseOrderDetailSummary>(x => x.PurchaseOrderDetailId);
    }
  }
}