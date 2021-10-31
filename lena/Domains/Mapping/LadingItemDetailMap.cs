using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class LadingItemDetailMap : IEntityTypeConfiguration<LadingItemDetail>
  {
    public void Configure(EntityTypeBuilder<LadingItemDetail> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_LadingItemDetail");
      builder.Property(x => x.Id);
      builder.Property(x => x.Qty);
      builder.Property(x => x.LadingItemId);
      builder.Property(x => x.CargoItemDetailId);
      builder.HasOne(x => x.LadingItem).WithMany(x => x.LadingItemDetails).HasForeignKey(x => x.LadingItemId);
      builder.HasOne(x => x.CargoItemDetail).WithMany(x => x.LadingItemDetails).HasForeignKey(x => x.CargoItemDetailId);
      builder.HasOne(x => x.LadingItemDetailSummary).WithOne(x => x.LadingItemDetail).HasForeignKey<LadingItemDetailSummary>(x => x.LadingItemDetailId);
    }
  }
}