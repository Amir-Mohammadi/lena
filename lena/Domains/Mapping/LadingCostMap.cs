using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class LadingCostMap : IEntityTypeConfiguration<LadingCost>
  {
    public void Configure(EntityTypeBuilder<LadingCost> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("LadingCosts");
      builder.Property(x => x.Id);
      builder.Property(x => x.FinancialDocumentCostId);
      builder.Property(x => x.Amount);
      builder.Property(x => x.LadingId);
      builder.Property(x => x.LadingItemId);
      builder.HasRowVersion();
      builder.HasOne(x => x.FinancialDocumentCost).WithMany(x => x.LadingCosts).HasForeignKey(x => x.FinancialDocumentCostId);
      builder.HasOne(x => x.Lading).WithMany(x => x.LadingCosts).HasForeignKey(x => x.LadingId);
      builder.HasOne(x => x.LadingItem).WithMany(x => x.LadingCosts).HasForeignKey(x => x.LadingItemId);
    }
  }
}
