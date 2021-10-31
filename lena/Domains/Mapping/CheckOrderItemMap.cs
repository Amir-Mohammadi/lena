using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CheckOrderItemMap : IEntityTypeConfiguration<CheckOrderItem>
  {
    public void Configure(EntityTypeBuilder<CheckOrderItem> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_CheckOrderItem");
      builder.Property(x => x.Id);
      builder.Property(x => x.OrderItemConfirmationId);
      builder.Property(x => x.Confirmed);
      builder.HasOne(x => x.OrderItemConfirmation).WithMany(x => x.CheckOrderItems).HasForeignKey(x => x.OrderItemConfirmationId);
    }
  }
}
