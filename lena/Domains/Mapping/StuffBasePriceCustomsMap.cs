using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffBasePriceCustomsMap : IEntityTypeConfiguration<StuffBasePriceCustoms>
  {
    public void Configure(EntityTypeBuilder<StuffBasePriceCustoms> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StuffBasePriceCustoms");
      builder.Property(x => x.Id);
      builder.Property(x => x.Type);
      builder.Property(x => x.Tariff);
      builder.Property(x => x.Percent);
      builder.Property(x => x.HowToBuyRatio);
      builder.Property(x => x.Price);
      builder.Property(x => x.HowToBuyId);
      builder.Property(x => x.Weight);
      builder.Property(x => x.CurrencyId);
      builder.Property(x => x.StuffBasePriceId);
      builder.HasRowVersion();
      builder.HasOne(x => x.HowToBuy).WithMany(x => x.StuffBasePriceCustoms).HasForeignKey(x => x.HowToBuyId);
      builder.HasOne(x => x.Currency).WithMany(x => x.StuffBasePriceCustoms).HasForeignKey(x => x.CurrencyId);
      builder.HasOne(x => x.StuffBasePrice).WithOne(x => x.StuffBasePriceCustoms).HasForeignKey<StuffBasePriceCustoms>(x => x.StuffBasePriceId);
    }
  }
}