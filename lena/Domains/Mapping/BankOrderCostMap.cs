using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BankOrderCostMap : IEntityTypeConfiguration<BankOrderCost>
  {
    public void Configure(EntityTypeBuilder<BankOrderCost> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BankOrderCosts");
      builder.Property(x => x.Id);
      builder.Property(x => x.FinancialDocumentCostId);
      builder.Property(x => x.Amount);
      builder.Property(x => x.BankOrderId);
      builder.HasRowVersion();
      builder.HasOne(x => x.FinancialDocumentCost).WithMany(x => x.BankOrderCosts).HasForeignKey(x => x.FinancialDocumentCostId);
      builder.HasOne(x => x.BankOrder).WithMany(x => x.BankOrderCosts).HasForeignKey(x => x.BankOrderId);
    }
  }
}
