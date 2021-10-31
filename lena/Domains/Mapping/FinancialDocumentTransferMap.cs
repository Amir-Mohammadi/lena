using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinancialDocumentTransferMap : IEntityTypeConfiguration<FinancialDocumentTransfer>
  {
    public void Configure(EntityTypeBuilder<FinancialDocumentTransfer> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinancialDocumentTransfers");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.ToFinancialAccountId);
      builder.Property(x => x.ToDebitAmount);
      builder.Property(x => x.FinancialDocumentId);
      builder.HasOne(x => x.FinancialDocument).WithOne(x => x.FinancialDocumentTransfer).HasForeignKey<FinancialDocumentTransfer>(x => x.FinancialDocumentId);

      builder.HasOne(x => x.ToFinancialAccount).WithMany(x => x.FinancialDocumentTransfers).HasForeignKey(x => x.ToFinancialAccountId);
    }
  }
}