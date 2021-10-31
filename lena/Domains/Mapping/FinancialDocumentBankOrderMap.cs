using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinancialDocumentBankOrderMap : IEntityTypeConfiguration<FinancialDocumentBankOrder>
  {
    public void Configure(EntityTypeBuilder<FinancialDocumentBankOrder> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinancialDocumentBankOrders");
      builder.Property(x => x.Id);
      builder.Property(x => x.BankOrderId);
      builder.Property(x => x.BankOrderAmount);
      builder.Property(x => x.FOB);
      builder.Property(x => x.TransferCost);
      builder.Property(x => x.FinancialDocumentId);
      builder.HasRowVersion();
      builder.HasOne(x => x.FinancialDocument).WithOne(x => x.FinancialDocumentBankOrder).HasForeignKey<FinancialDocumentBankOrder>(x => x.FinancialDocumentId);
      builder.HasOne(x => x.BankOrder).WithMany(x => x.FinancialDocumentBankOrders).HasForeignKey(x => x.BankOrderId);
    }
  }
}