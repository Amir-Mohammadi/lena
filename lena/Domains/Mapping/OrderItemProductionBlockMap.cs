using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OrderItemProductionBlockMap : IEntityTypeConfiguration<OrderItemProductionBlock>
  {
    public void Configure(EntityTypeBuilder<OrderItemProductionBlock> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_OrderItemProductionBlock");
      builder.Property(x => x.Id);
    }
  }
}
