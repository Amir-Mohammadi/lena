using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WarehouseInventoryResultMap : IEntityTypeConfiguration<WarehouseInventoryResult>
  {
    public void Configure(EntityTypeBuilder<WarehouseInventoryResult> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("WarehouseInventoryResults");
      builder.Property(x => x.Id);
      builder.Property(x => x.WarehouseId);
      builder.Property(x => x.WarehouseName).IsRequired();
      builder.Property(x => x.StuffId);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.StuffCategoryId);
      builder.Property(x => x.StuffCategoryName).IsRequired();
      builder.Property(x => x.StuffCode).IsRequired();
      builder.Property(x => x.StuffName).IsRequired();
      builder.Property(x => x.BillOfMaterialVersion);
      builder.Property(x => x.BillOfMaterialTitle).IsRequired();
      builder.Property(x => x.UnitRialPrice);
      builder.Property(x => x.TotalAmount);
      builder.Property(x => x.AvailableAmount);
      builder.Property(x => x.BlockedAmount);
      builder.Property(x => x.QualityControlAmount);
      builder.Property(x => x.WasteAmount);
      builder.Property(x => x.SerialBufferAmount);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.UnitName).IsRequired();
      builder.Property(x => x.Serial).IsRequired();
      builder.Property(x => x.SerialStatus);
      builder.Property(x => x.DecimalDigitCount);
      builder.Property(x => x.SerialProfileDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.StuffType);
      builder.Property(x => x.StockPlaceCodes).IsRequired();
      builder.Property(x => x.StockPlaceTitles).IsRequired();
      builder.Property(x => x.QualityControlDescription);
      builder.HasRowVersion();
      builder.Property(x => x.SerialProfileCode);
    }
  }
}
