using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class NewShoppingDetailMap : IEntityTypeConfiguration<NewShoppingDetail>
  {
    public void Configure(EntityTypeBuilder<NewShoppingDetail> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_NewShoppingDetail");
      builder.Property(x => x.Id);
      builder.Property(x => x.NewShoppingId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.LadingItemDetailId);
      builder.HasOne(x => x.NewShopping).WithMany(x => x.NewShoppingDetails).HasForeignKey(x => x.NewShoppingId);
      builder.HasOne(x => x.NewShoppingDetailSummary).WithOne(x => x.NewShoppingDetail).HasForeignKey<NewShoppingDetailSummary>(x => x.NewShoppingDetailId);
      builder.HasOne(x => x.Unit).WithMany(x => x.NewShoppingDetails).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.LadingItemDetail).WithMany(x => x.NewShoppingDetails).HasForeignKey(x => x.LadingItemDetailId);
    }
  }
}