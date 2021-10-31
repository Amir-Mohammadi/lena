using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PriceAnnunciationItemMap : IEntityTypeConfiguration<PriceAnnunciationItem>
  {
    public void Configure(EntityTypeBuilder<PriceAnnunciationItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PriceAnnunciationItems");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Price);
      builder.Property(x => x.PriceAnnunciationId);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.Count);
      builder.Property(x => x.CurrencyId);
      builder.Property(x => x.ConfirmerUserId);
      builder.Property(x => x.Status);
      builder.Property(x => x.Description);
      builder.HasOne(x => x.PriceAnnunciation).WithMany(x => x.PriceAnnunciationItems).HasForeignKey(x => x.PriceAnnunciationId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.PriceAnnunciationItems).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Currency).WithMany(x => x.PriceAnnunciationItems).HasForeignKey(x => x.CurrencyId);
      builder.HasOne(x => x.ConfirmerUser).WithMany(x => x.PriceAnnunciationItems).HasForeignKey(x => x.ConfirmerUserId);
    }
  }
}
