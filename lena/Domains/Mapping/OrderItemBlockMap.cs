using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OrderItemBlockMap : IEntityTypeConfiguration<OrderItemBlock>
  {
    public void Configure(EntityTypeBuilder<OrderItemBlock> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_OrderItemBlock");
      builder.Property(x => x.Id);
      builder.Property(x => x.OrderItemId);
      builder.Property(x => x.OrderItemBlockType);
      builder.HasOne(x => x.OrderItem).WithMany(x => x.OrderItemBlocks).HasForeignKey(x => x.OrderItemId);
    }
  }
}
