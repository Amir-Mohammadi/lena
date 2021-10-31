using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseOrderCostMap : IEntityTypeConfiguration<PurchaseOrderCost>
  {
    public void Configure(EntityTypeBuilder<PurchaseOrderCost> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PurchaseOrderCosts");
      builder.Property(x => x.Id);
      builder.Property(x => x.Amount);
      builder.HasRowVersion();
      builder.Property(x => x.PurchaseOrderGroupId);
      builder.Property(x => x.PurchaseOrderId);
      builder.Property(x => x.FinancialDocumentCostId);
      builder.HasOne(x => x.PurchaseOrderGroup).WithMany(x => x.PurchaseOrderCosts).HasForeignKey(x => x.PurchaseOrderGroupId);
      builder.HasOne(x => x.PurchaseOrder).WithMany(x => x.PurchaseOrderCosts).HasForeignKey(x => x.PurchaseOrderId);
      builder.HasOne(x => x.FinancialDocumentCost).WithMany(x => x.PurchaseOrderCosts).HasForeignKey(x => x.FinancialDocumentCostId);
    }
  }
}
