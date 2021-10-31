using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WarehouseIssueItemMap : IEntityTypeConfiguration<WarehouseIssueItem>
  {
    public void Configure(EntityTypeBuilder<WarehouseIssueItem> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_WarehouseIssueItem");
      builder.Property(x => x.Id);
      builder.Property(x => x.WarehouseIssueId);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.BillOfMaterialVersion);
      builder.Property(x => x.Amount);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.TransactionLevel);
      builder.Property(x => x.AssetCode);
      builder.HasOne(x => x.WarehouseIssue).WithMany(x => x.WarehouseIssueItems).HasForeignKey(x => x.WarehouseIssueId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.WarehouseIssueItems).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.BillOfMaterial).WithMany(x => x.WarehouseIssueItems).HasForeignKey(x => new
      {
        x.BillOfMaterialVersion,
        x.StuffId
      });
      builder.HasOne(x => x.Unit).WithMany(x => x.WarehouseIssueItems).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.StuffSerial).WithMany(x => x.WarehouseIssueItems).HasForeignKey(x => new { x.StuffSerialCode, x.StuffId });
    }
  }
}
