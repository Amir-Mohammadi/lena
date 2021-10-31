using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CurrencyRateMap : IEntityTypeConfiguration<CurrencyRate>
  {
    public void Configure(EntityTypeBuilder<CurrencyRate> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("CurrencyRates");
      builder.Property(x => x.Id);
      builder.Property(x => x.FromDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Commission);
      builder.Property(x => x.Rate);
      builder.Property(x => x.FromCurrencyId);
      builder.Property(x => x.ToCurrencyId);
      builder.Property(x => x.ExchangeId);
      builder.HasRowVersion();
      builder.HasOne(x => x.FromCurrency).WithMany(x => x.ToCurrencyRates).HasForeignKey(x => x.FromCurrencyId);
      builder.HasOne(x => x.ToCurrency).WithMany(x => x.FromCurrencyRates).HasForeignKey(x => x.ToCurrencyId);
      builder.HasOne(x => x.Exchange).WithMany(x => x.CurrencyRates).HasForeignKey(x => x.ExchangeId);
    }
  }
}
