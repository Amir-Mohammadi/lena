using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffBasePriceMap : IEntityTypeConfiguration<StuffBasePrice>
  {
    public void Configure(EntityTypeBuilder<StuffBasePrice> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_StuffBasePrice");
      builder.Property(x => x.Id);
      builder.Property(x => x.StuffBasePriceType);
      builder.Property(x => x.MainPrice);
      builder.Property(x => x.PurchaseOrderId);
      builder.HasOne(x => x.PurchaseOrder).WithOne(x => x.StuffBasePrice).HasForeignKey<StuffBasePrice>(x => x.PurchaseOrderId);
    }
  }
}