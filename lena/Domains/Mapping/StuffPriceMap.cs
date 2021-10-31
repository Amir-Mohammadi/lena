using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffPriceMap : IEntityTypeConfiguration<StuffPrice>
  {
    public void Configure(EntityTypeBuilder<StuffPrice> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_StuffPrice");
      builder.Property(x => x.Id);
      builder.Property(x => x.Price);
      builder.Property(x => x.Type);
      builder.Property(x => x.Status);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.CurrencyId);
      builder.Property(x => x.ConfirmUserId);
      builder.Property(x => x.ConfirmDate).HasColumnType("smalldatetime");
      builder.HasOne(x => x.Stuff).WithMany(x => x.StuffPrices).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Currency).WithMany(x => x.StuffPrices).HasForeignKey(x => x.CurrencyId);
      builder.HasOne(x => x.ConfirmUser).WithMany(x => x.StuffPrices).HasForeignKey(x => x.ConfirmUserId);
    }
  }
}
