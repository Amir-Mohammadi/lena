using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinancialDocumentCorrectionMap : IEntityTypeConfiguration<FinancialDocumentCorrection>
  {
    public void Configure(EntityTypeBuilder<FinancialDocumentCorrection> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinancialDocumentCorrections");
      builder.Property(x => x.Id);
      builder.Property(x => x.FinancialDocumentId);
      builder.HasRowVersion();
      builder.Property(x => x.IsActive);
      builder.Property(x => x.FinancialTransactionLevel);
      builder.HasOne(x => x.FinancialDocument).WithOne(x => x.FinancialDocumentCorrection).HasForeignKey<FinancialDocumentCorrection>(x => x.FinancialDocumentId);
    }
  }
}