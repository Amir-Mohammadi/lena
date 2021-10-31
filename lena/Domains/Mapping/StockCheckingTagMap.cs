using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StockCheckingTagMap : IEntityTypeConfiguration<StockCheckingTag>
  {
    public void Configure(EntityTypeBuilder<StockCheckingTag> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StockCheckingTags");
      builder.Property(x => x.Id);
      builder.Property(x => x.Number);
      builder.Property(x => x.StockCheckingId);
      builder.Property(x => x.WarehouseId);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.TagTypeId);
      builder.Property(x => x.Amount);
      builder.Property(x => x.UnitId);
      builder.HasRowVersion();
      builder.HasOne(x => x.StockCheckingWarehouse).WithMany(x => x.StockCheckingTags).HasForeignKey(x => new
      {
        x.StockCheckingId,
        x.WarehouseId
      });
      builder.HasOne(x => x.Stuff).WithMany(x => x.StockCheckingTags).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.StuffSerial).WithMany(x => x.StockCheckingTags).HasForeignKey(x => new { x.StuffSerialCode, x.StuffId });
      builder.HasOne(x => x.TagType).WithMany(x => x.StockCheckingTags).HasForeignKey(x => x.TagTypeId);
      builder.HasOne(x => x.Unit).WithMany(x => x.StockCheckingTags).HasForeignKey(x => x.UnitId);
    }
  }
}
