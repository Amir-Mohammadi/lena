using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OrderTypeMap : IEntityTypeConfiguration<OrderType>
  {
    public void Configure(EntityTypeBuilder<OrderType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("OrderTypes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.HasRowVersion();
    }
  }
}
