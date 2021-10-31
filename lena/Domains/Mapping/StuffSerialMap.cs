using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffSerialMap : IEntityTypeConfiguration<StuffSerial>
  {
    public void Configure(EntityTypeBuilder<StuffSerial> builder)
    {
      builder.HasKey(x => new { x.Code, x.StuffId });
      builder.ToTable("StuffSerials");
      builder.Property(x => x.Code);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.BatchNo);
      builder.Property(x => x.SerialProfileCode);
      builder.Property(x => x.Order);
      builder.Property(x => x.Serial).IsRequired().HasMaxLength(50);
      builder.Property(x => x.InitQty);
      builder.Property(x => x.PartitionedQty);
      builder.Property(x => x.InitUnitId);
      builder.Property(x => x.PartitionStuffSerialId);
      builder.Property(x => x.IsPacking);
      builder.Property(x => x.Status);
      builder.HasRowVersion();
      builder.Property(x => x.BillOfMaterialVersion);
      builder.Property(x => x.QualityControlDescription);
      builder.Property(x => x.LastModified);
      builder.Property(x => x.LastModifiedUserId);
      builder.Property(x => x.CRC);
      builder.Property(x => x.UnitRialPrice);
      builder.Property(x => x.ProductionOrderId);
      builder.Property(x => x.WarehouseEnterTime).HasColumnType("smalldatetime");
      builder.HasOne(x => x.SerialProfile).WithMany(x => x.StuffSerials).HasForeignKey(x => new { x.SerialProfileCode, x.StuffId });
      builder.HasOne(x => x.Stuff).WithMany(x => x.StuffSerials).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.InitUnit).WithMany(x => x.StuffSerials).HasForeignKey(x => x.InitUnitId);
      builder.HasOne(x => x.PartitionStuffSerial).WithMany(x => x.ChildStuffSerials).HasForeignKey(x => x.PartitionStuffSerialId);
      builder.HasOne(x => x.BillOfMaterial).WithMany(x => x.StuffSerials).HasForeignKey(x => new { x.BillOfMaterialVersion, x.StuffId });
      builder.HasOne(x => x.LastUserModified).WithMany(x => x.StuffSerials).HasForeignKey(x => x.LastModifiedUserId);
      //builder.HasOne(x => x.LinkSerial).WithOne(x => x.StuffSerial).HasForeignKey<("StuffSerial>(x => x.LinkedSerial);
      builder.HasOne(x => x.ProductionOrder).WithMany(x => x.StuffSerials).HasForeignKey(x => x.ProductionOrderId);
      builder.HasOne(x => x.IssueConfirmerUser).WithMany().HasForeignKey(x => x.IssueConfirmerUserId);
      builder.HasOne(x => x.IssueUser).WithMany().HasForeignKey(x => x.IssueUserId);
    }
  }
}