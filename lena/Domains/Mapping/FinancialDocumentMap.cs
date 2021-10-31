using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinancialDocumentMap : IEntityTypeConfiguration<FinancialDocument>
  {
    public void Configure(EntityTypeBuilder<FinancialDocument> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_FinancialDocument");
      builder.Property(x => x.Id);
      builder.Property(x => x.Type);
      builder.Property(x => x.FinancialAccountId);
      builder.Property(x => x.DocumentId);
      builder.Property(x => x.DocumentDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.DebitAmount);
      builder.Property(x => x.CreditAmount);
      builder.HasOne(x => x.FinancialAccount).WithMany(x => x.FinancialDocuments).HasForeignKey(x => x.FinancialAccountId);

      builder.HasOne(x => x.Finance).WithMany(x => x.FinancialDocuments).HasForeignKey(x => x.FinanceId);
      //builder.HasOne(x => x.).WithOne(x => x.BankOrderIssue).HasForeignKey<("BaseEntities_FinancialDocument>(x => x.BanKOrderIssueId);
    }
  }
}
