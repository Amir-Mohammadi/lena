using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OrderItemChangeRequestMap : IEntityTypeConfiguration<OrderItemChangeRequest>
  {
    public void Configure(EntityTypeBuilder<OrderItemChangeRequest> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_OrderItemChangeRequest");
      builder.Property(x => x.Id);
      builder.Property(x => x.Qty);
      builder.Property(x => x.RequestDate).HasColumnType("smalldatetime");
      builder.Property(x => x.DeliveryDate).HasColumnType("smalldatetime");
      builder.Property(x => x.OrderItemId);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.OrderItemChangeStatus);
      builder.HasOne(x => x.OrderItem).WithMany(x => x.OrderItemChangeRequests).HasForeignKey(x => x.OrderItemId);
      builder.HasOne(x => x.Unit).WithMany(x => x.OrderItemChangeRequests).HasForeignKey(x => x.UnitId);
    }
  }
}
