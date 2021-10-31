using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinancialDocumentBeginningMap : IEntityTypeConfiguration<FinancialDocumentBeginning>
  {
    public void Configure(EntityTypeBuilder<FinancialDocumentBeginning> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinancialDocumentBeginnings");
      builder.Property(x => x.Id);
      builder.Property(x => x.FinancialDocumentId);
      builder.HasRowVersion();
      builder.Property(x => x.FinancialTransactionLevel);
      builder.HasOne(x => x.FinancialDocument).WithOne(x => x.FinancialDocumentBeginning).HasForeignKey<FinancialDocumentBeginning>(x => x.FinancialDocumentId);
    }
  }
}