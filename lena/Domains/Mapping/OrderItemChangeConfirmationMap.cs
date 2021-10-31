using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OrderItemChangeConfirmationMap : IEntityTypeConfiguration<OrderItemChangeConfirmation>
  {
    public void Configure(EntityTypeBuilder<OrderItemChangeConfirmation> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_OrderItemChangeConfirmation");
      builder.Property(x => x.Id);
      builder.Property(x => x.OrderItemChangeRequestId);
      builder.Property(x => x.Confirmed);
      builder.HasOne(x => x.OrderItemChangeRequest).WithMany(x => x.OrderItemChangeConfirmations).HasForeignKey(x => x.OrderItemChangeRequestId);
    }
  }
}
