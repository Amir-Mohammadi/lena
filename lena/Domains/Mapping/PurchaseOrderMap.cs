using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseOrderMap : IEntityTypeConfiguration<PurchaseOrder>
  {
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_PurchaseOrder");
      builder.Property(x => x.Id);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.ProviderId);
      builder.Property(x => x.Price);
      builder.Property(x => x.CurrencyId);
      builder.Property(x => x.Deadline);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.SupplierId);
      builder.Property(x => x.PurchaseOrderType);
      builder.Property(x => x.Status);
      builder.Property(x => x.PurchaseOrderDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.PurchaseOrderGroupId);
      builder.Property(x => x.IsArchived);
      builder.Property(x => x.PurchaseOrderPreparingDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.PurchaseOrderStepDetailId);
      builder.Property(x => x.OrderInvoiceNum);
      builder.Property(x => x.LatestRiskId);
      builder.HasOne(x => x.Currency).WithMany(x => x.PurchaseOrders).HasForeignKey(x => x.CurrencyId);
      builder.HasOne(x => x.Unit).WithMany(x => x.PurchaseOrders).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.PurchaseOrders).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.StuffProvider).WithMany(x => x.PurchaseOrders).HasForeignKey(x => new { x.StuffId, x.ProviderId });
      builder.HasOne(x => x.Supplier).WithMany(x => x.PurchaseOrders).HasForeignKey(x => x.SupplierId);
      builder.HasOne(x => x.Provider).WithMany(x => x.PurchaseOrders).HasForeignKey(x => x.ProviderId);
      builder.HasOne(x => x.PurchaseOrderGroup).WithMany(x => x.PurchaseOrders).HasForeignKey(x => x.PurchaseOrderGroupId);
      builder.HasOne(x => x.PurchaseOrderStepDetail).WithMany(x => x.PurchaseOrders).HasForeignKey(x => x.PurchaseOrderStepDetailId);
      builder.HasOne(x => x.LatestRisk).WithOne().HasForeignKey<PurchaseOrder>(x => x.LatestRiskId);
    }
  }
}