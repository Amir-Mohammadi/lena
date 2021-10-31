using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OrderItemMap : IEntityTypeConfiguration<OrderItem>
  {
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_OrderItem");
      builder.Property(x => x.Id);
      builder.Property(x => x.OrderId);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.CanceledQty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.DeliveryDate).HasColumnType("smalldatetime");
      builder.Property(x => x.RequestDate).HasColumnType("smalldatetime");
      builder.Property(x => x.BillOfMaterialVersion);
      builder.Property(x => x.Status);
      builder.Property(x => x.HasChange);
      builder.Property(x => x.OrderItemConfirmationDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.OrderItemConfirmationConfirmed);
      builder.Property(x => x.OrderItemHasActivated);
      builder.Property(x => x.CheckOrderItemDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.CheckOrderItemConfirmed);
      builder.Property(x => x.OrderItemChangeStatus);
      builder.Property(x => x.IsArchive);
      builder.Property(x => x.ProductPackBillOfMaterialVersion);
      builder.Property(x => x.ProductPackBillOfMaterialStuffId);
      builder.HasOne(x => x.Order).WithMany(x => x.OrderItems).HasForeignKey(x => x.OrderId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.OrderItems).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Unit).WithMany(x => x.OrderItems).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.BillOfMaterial).WithMany(x => x.OrderItems).HasForeignKey(x => new
      {
        x.BillOfMaterialVersion,
        x.StuffId
      });
    }
  }
}