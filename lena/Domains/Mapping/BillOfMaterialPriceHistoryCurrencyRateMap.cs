using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BillOfMaterialPriceHistoryCurrencyRateMap : IEntityTypeConfiguration<BillOfMaterialPriceHistoryCurrencyRate>
  {
    public void Configure(EntityTypeBuilder<BillOfMaterialPriceHistoryCurrencyRate> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BillOfMaterialPriceHistoryCurrencyRates");
      builder.HasRowVersion();
      builder.Property(x => x.Rate).IsRequired();
      builder.HasOne(x => x.BillOfMaterialHistory).WithMany(x => x.BillOfMaterialPriceHistoryCurrencyRates).HasForeignKey(x => x.BillOfMaterialPriceHistoryId);
      builder.HasOne(x => x.FromCurrency).WithMany(x => x.BillOfMaterialPriceHistoryFromCurrencyRates).HasForeignKey(x => x.FromCurrencyId);
      builder.HasOne(x => x.ToCurrency).WithMany(x => x.BillOfMaterialPriceHistoryToCurrencyRates).HasForeignKey(x => x.ToCurrencyId);
    }
  }
}