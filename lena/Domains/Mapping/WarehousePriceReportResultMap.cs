using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WarehousePriceReportResultMap : IEntityTypeConfiguration<WarehousePriceReportResult>
  {
    public void Configure(EntityTypeBuilder<WarehousePriceReportResult> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("WarehousePriceReportResults");
      builder.Property(x => x.WarehouseId);
      builder.Property(x => x.WarehouseName).IsRequired();
      builder.Property(x => x.StuffId);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.StuffCategoryId);
      builder.Property(x => x.StuffCategoryName).IsRequired();
      builder.Property(x => x.StuffCode).IsRequired();
      builder.Property(x => x.StuffName).IsRequired();
      builder.Property(x => x.TotalAmount);
      builder.Property(x => x.AvailableAmount);
      builder.Property(x => x.BlockedAmount);
      builder.Property(x => x.QualityControlAmount);
      builder.Property(x => x.WasteAmount);
      builder.Property(x => x.SerialBufferAmount);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.UnitName).IsRequired();
      builder.Property(x => x.Serial);
      builder.Property(x => x.StuffLastTransactionDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.LastStuffPrice);
      builder.Property(x => x.TotalAmountPrice);
      builder.Property(x => x.CurrencyId);
      builder.Property(x => x.CurrencyCode);
      builder.Property(x => x.CurrencyTitle);
      builder.Property(x => x.CurrencySign);
      builder.Property(x => x.CurrencyDecimalDigitCount);
      builder.Property(x => x.LastStuffPriceDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
    }
  }
}
