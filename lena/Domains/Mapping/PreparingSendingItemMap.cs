using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PreparingSendingItemMap : IEntityTypeConfiguration<PreparingSendingItem>
  {
    public void Configure(EntityTypeBuilder<PreparingSendingItem> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_PreparingSendingItem");
      builder.Property(x => x.Id);
      builder.Property(x => x.PreparingSendingId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.StuffSerialCode);
      builder.HasOne(x => x.PreparingSending).WithMany(x => x.PreparingSendingItems).HasForeignKey(x => x.PreparingSendingId);
      builder.HasOne(x => x.Unit).WithMany(x => x.PreparingSendingItems).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.PreparingSendingItems).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.StuffSerial).WithMany(x => x.PreparingSendingItems).HasForeignKey(x => new
      {
        x.StuffSerialCode,
        x.StuffId
      });
    }
  }
}
