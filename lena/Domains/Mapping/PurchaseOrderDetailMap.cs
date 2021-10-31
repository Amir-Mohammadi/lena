using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseOrderDetailMap : IEntityTypeConfiguration<PurchaseOrderDetail>
  {
    public void Configure(EntityTypeBuilder<PurchaseOrderDetail> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_PurchaseOrderDetail");
      builder.Property(x => x.Id);
      builder.Property(x => x.PurchaseOrderId);
      builder.Property(x => x.PurchaseRequestId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.HasOne(x => x.PurchaseOrder).WithMany(x => x.PurchaseOrderDetails).HasForeignKey(x => x.PurchaseOrderId);
      builder.HasOne(x => x.PurchaseRequest).WithMany(x => x.PurchaseOrderDetails).HasForeignKey(x => x.PurchaseRequestId);
      builder.HasOne(x => x.Unit).WithMany(x => x.PurchaseOrderDetails).HasForeignKey(x => x.UnitId);
    }
  }
}
