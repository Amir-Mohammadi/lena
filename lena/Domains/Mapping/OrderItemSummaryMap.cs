using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OrderItemSummaryMap : IEntityTypeConfiguration<OrderItemSummary>
  {
    public void Configure(EntityTypeBuilder<OrderItemSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("OrderItemSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.PlannedQty);
      builder.Property(x => x.ProducedQty);
      builder.Property(x => x.BlockedQty);
      builder.Property(x => x.PermissionQty);
      builder.Property(x => x.PreparingSendingQty);
      builder.Property(x => x.SendedQty);
      builder.Property(x => x.SentToOtherCustomersQty);
      builder.Property(x => x.BlockedQtyOtherCustomers);
      builder.Property(x => x.OrderItemId);
      builder.HasRowVersion();
      builder.HasOne(x => x.OrderItem).WithOne(x => x.OrderItemSummary).HasForeignKey<OrderItemSummary>(x => x.OrderItemId);
    }
  }
}