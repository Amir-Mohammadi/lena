using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinancialDocumentCostMap : IEntityTypeConfiguration<FinancialDocumentCost>
  {
    public void Configure(EntityTypeBuilder<FinancialDocumentCost> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinancialDocumentCosts");
      builder.Property(x => x.Id);
      builder.Property(x => x.CostType);
      builder.Property(x => x.CargoWeight);
      builder.Property(x => x.LadingWeight);
      builder.Property(x => x.PurchaseOrderWeight);
      builder.Property(x => x.KotazhTransPort).IsRequired(false);
      builder.Property(x => x.EntranceRightsCost).IsRequired(false);
      builder.Property(x => x.FinancialDocumentId);
      builder.HasRowVersion();
      builder.HasOne(x => x.FinancialDocument).WithOne(x => x.FinancialDocumentCost).HasForeignKey<FinancialDocumentCost>(x => x.FinancialDocumentId);
    }
  }
}