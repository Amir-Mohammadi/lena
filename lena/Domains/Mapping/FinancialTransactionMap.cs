using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinancialTransactionMap : IEntityTypeConfiguration<FinancialTransaction>
  {
    public void Configure(EntityTypeBuilder<FinancialTransaction> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinancialTransactions");
      builder.Property(x => x.Id);
      builder.Property(x => x.FinancialTransactionBatchId);
      builder.Property(x => x.FinancialTransactionTypeId);
      builder.Property(x => x.FinancialAccountId);
      builder.Property(x => x.EffectDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Amount);
      builder.Property(x => x.Description);
      builder.Property(x => x.ReferenceFinancialTransactionId);
      builder.HasRowVersion();
      builder.Property(x => x.IsDelete);
      builder.Property(x => x.IsPermanent);
      builder.HasOne(x => x.FinancialTransactionType).WithMany(x => x.FinancialTransactions).HasForeignKey(x => x.FinancialTransactionTypeId);
      builder.HasOne(x => x.FinancialAccount).WithMany(x => x.FinancialTransactions).HasForeignKey(x => x.FinancialAccountId);
      builder.HasOne(x => x.FinancialTransactionBatch).WithMany(x => x.FinancialTransactions).HasForeignKey(x => x.FinancialTransactionBatchId);
      builder.HasOne(x => x.ReferenceFinancialTransaction).WithMany(x => x.ReferencedFinancialTransactions).HasForeignKey(x => x.ReferenceFinancialTransactionId);
    }
  }
}