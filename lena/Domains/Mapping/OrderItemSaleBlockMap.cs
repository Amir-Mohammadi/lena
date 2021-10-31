using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OrderItemSaleBlockMap : IEntityTypeConfiguration<OrderItemSaleBlock>
  {
    public void Configure(EntityTypeBuilder<OrderItemSaleBlock> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_OrderItemSaleBlock");
      builder.Property(x => x.Id);
      builder.Property(x => x.CheckOrderItemId);
      builder.HasOne(x => x.CheckOrderItem).WithMany(x => x.OrderItemSaleBlocks).HasForeignKey(x => x.CheckOrderItemId);
    }
  }
}
