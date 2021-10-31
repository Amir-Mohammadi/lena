using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OrderMap : IEntityTypeConfiguration<Order>
  {
    public void Configure(EntityTypeBuilder<Order> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Orders");
      builder.Property(x => x.Id);
      builder.Property(x => x.Description);
      builder.Property(x => x.OrderTypeId);
      builder.HasRowVersion();
      builder.Property(x => x.CustomerId);
      builder.Property(x => x.Orderer);
      builder.Property(x => x.DocumentNumber);
      builder.Property(x => x.DocumentType);
      builder.Property(x => x.IsDelete);
      builder.Property(x => x.TotalAmount);
      builder.HasOne(x => x.OrderType).WithMany(x => x.Orders).HasForeignKey(x => x.OrderTypeId);
      builder.HasOne(x => x.Customer).WithMany(x => x.Orders).HasForeignKey(x => x.CustomerId);
    }
  }
}
