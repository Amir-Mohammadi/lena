using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OrderItemConfirmationMap : IEntityTypeConfiguration<OrderItemConfirmation>
  {
    public void Configure(EntityTypeBuilder<OrderItemConfirmation> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_OrderItemConfirmation");
      builder.Property(x => x.Id);
      builder.Property(x => x.Confirmed);
      builder.Property(x => x.OrderItemId);
      builder.HasOne(x => x.OrderItem).WithMany(x => x.OrderItemConfirmations).HasForeignKey(x => x.OrderItemId);
    }
  }
}
