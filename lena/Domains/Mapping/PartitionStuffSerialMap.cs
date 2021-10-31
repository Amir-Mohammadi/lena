using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PartitionStuffSerialMap : IEntityTypeConfiguration<PartitionStuffSerial>
  {
    public void Configure(EntityTypeBuilder<PartitionStuffSerial> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_PartitionStuffSerial");
      builder.Property(x => x.Id);
      builder.Property(x => x.MainStuffSerialCode);
      builder.Property(x => x.MainStuffSerialStuffId);
      builder.Property(x => x.WarehouseId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.BoxCount);
      builder.Property(x => x.QtyPerBox);
      builder.HasOne(x => x.MainStuffSerial).WithMany(x => x.PartitionStuffSerials).HasForeignKey(x => new
      {
        x.MainStuffSerialCode,
        x.MainStuffSerialStuffId
      });
      builder.HasOne(x => x.Warehouse).WithMany(x => x.PartitionStuffSerials).HasForeignKey(x => x.WarehouseId);
      builder.HasOne(x => x.Unit).WithMany(x => x.PartitionStuffSerials).HasForeignKey(x => x.UnitId);
    }
  }
}
