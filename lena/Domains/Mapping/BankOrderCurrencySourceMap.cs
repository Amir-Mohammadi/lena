using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BankOrderCurrencySourceMap : IEntityTypeConfiguration<BankOrderCurrencySource>
  {
    public void Configure(EntityTypeBuilder<BankOrderCurrencySource> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BankOrderCurrencySources");
      builder.Property(x => x.Id);
      builder.Property(x => x.HasFinancialDocumentBankOrder);
      builder.Property(x => x.BankOrderId);
      builder.Property(x => x.FOB);
      builder.Property(x => x.TransferCost);
      builder.Property(x => x.BoxCount);
      builder.Property(x => x.SataCode);
      builder.Property(x => x.ActualWeight);
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.LadingId);
      builder.HasRowVersion();
      builder.HasOne(x => x.BankOrder).WithMany(x => x.BankOrderCurrencySources).HasForeignKey(x => x.BankOrderId);
    }
  }
}