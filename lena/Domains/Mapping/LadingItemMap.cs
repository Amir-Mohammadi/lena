using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class LadingItemMap : IEntityTypeConfiguration<LadingItem>
  {
    public void Configure(EntityTypeBuilder<LadingItem> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_LadingItem");
      builder.Property(x => x.Id);
      builder.Property(x => x.Qty);
      builder.Property(x => x.LadingId);
      builder.Property(x => x.CargoItemId);
      builder.Property(x => x.Status);
      builder.HasOne(x => x.Lading).WithMany(x => x.LadingItems).HasForeignKey(x => x.LadingId);
      builder.HasOne(x => x.CargoItem).WithMany(x => x.LadingItems).HasForeignKey(x => x.CargoItemId);
      builder.HasOne(x => x.LadingItemSummary).WithOne(x => x.LadingItem).HasForeignKey<LadingItemSummary>(x => x.LadingItemId);
    }
  }
}
