using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BankOrderIssueMap : IEntityTypeConfiguration<BankOrderIssue>
  {
    public void Configure(EntityTypeBuilder<BankOrderIssue> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BankOrderIssues");
      builder.Property(x => x.Id);
      builder.Property(x => x.IsDelete);
      builder.Property(x => x.FinancialDocumentId);
      builder.Property(x => x.Number);
      builder.Property(x => x.BankOrderIssueTypeId).IsRequired();
      builder.Property(x => x.AllocationId).IsRequired();
      builder.Property(x => x.NetAmountPaid).IsRequired();
      builder.Property(x => x.ConvertRate).IsRequired();
      builder.Property(x => x.CurrencyFee).IsRequired();
      builder.Property(x => x.RialFee).IsRequired();
      builder.Property(x => x.DailyUSDRate).IsRequired();
      builder.Property(x => x.FinishedCurrencyRate).IsRequired();
      builder.Property(x => x.DailyExchangeRateUSD).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.BankOrderIssueType).WithMany(x => x.BankOrderIssues).HasForeignKey(x => x.BankOrderIssueTypeId);
      builder.HasOne(x => x.Allocation).WithMany(x => x.BankOrderIssues).HasForeignKey(x => x.AllocationId);
      builder.HasOne(x => x.FinancialDocument).WithMany(x => x.BankOrderIssues).HasForeignKey(x => x.FinancialDocumentId);
    }
  }
}