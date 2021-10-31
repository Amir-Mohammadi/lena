using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinancialDocumentDiscountMap : IEntityTypeConfiguration<FinancialDocumentDiscount>
  {
    public void Configure(EntityTypeBuilder<FinancialDocumentDiscount> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinancialDocumentDiscounts");
      builder.Property(x => x.Id);
      builder.Property(x => x.DiscountType);
      builder.Property(x => x.FinancialDocumentId);
      builder.HasRowVersion();
      builder.HasOne(x => x.FinancialDocument).WithOne(x => x.FinancialDocumentDiscount).HasForeignKey<FinancialDocumentDiscount>(x => x.FinancialDocumentId);
    }
  }
}