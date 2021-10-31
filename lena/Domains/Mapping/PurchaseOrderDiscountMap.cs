using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseOrderDiscountMap : IEntityTypeConfiguration<PurchaseOrderDiscount>
  {
    public void Configure(EntityTypeBuilder<PurchaseOrderDiscount> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PurchaseOrderDiscounts");
      builder.Property(x => x.Id);
      builder.Property(x => x.Amount);
      builder.HasRowVersion();
      builder.Property(x => x.FinancialDocumentDiscountId);
      builder.Property(x => x.PurchaseOrderGroupId);
      builder.Property(x => x.PurchaseOrderId);
      builder.HasOne(x => x.FinancialDocumentDiscount).WithMany(x => x.PurchaseOrderDiscounts).HasForeignKey(x => x.FinancialDocumentDiscountId);
      builder.HasOne(x => x.PurchaseOrderGroup).WithMany(x => x.PurchaseOrderDiscounts).HasForeignKey(x => x.PurchaseOrderGroupId);
      builder.HasOne(x => x.PurchaseOrder).WithMany(x => x.PurchaseOrderDiscounts).HasForeignKey(x => x.PurchaseOrderId);
    }
  }
}
