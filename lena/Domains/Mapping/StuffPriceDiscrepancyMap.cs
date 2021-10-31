using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffPriceDiscrepancyMap : IEntityTypeConfiguration<StuffPriceDiscrepancy>
  {
    public void Configure(EntityTypeBuilder<StuffPriceDiscrepancy> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StuffPriceDiscrepancies");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.PurchaseOrderId);
      builder.Property(x => x.PurchaseOrderPrice);
      builder.Property(x => x.PurchaseOrderCurrencyId);
      builder.Property(x => x.PurchaseOrderQty);
      builder.Property(x => x.CurrentStuffBasePrice);
      builder.Property(x => x.CurrentStuffBasePriceCurrencyId);
      builder.Property(x => x.Description);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.ConfirmationDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.ConfirmerUserId);
      builder.Property(x => x.ConfirmationDescription);
      builder.Property(x => x.Status);
      builder.HasOne(x => x.PurchaseOrder).WithMany(x => x.StuffPriceDiscrepancies).HasForeignKey(x => x.PurchaseOrderId);
      builder.HasOne(x => x.Currency).WithMany(x => x.StuffPriceDiscrepancies).HasForeignKey(x => x.CurrentStuffBasePriceCurrencyId);
      builder.HasOne(x => x.User).WithMany(x => x.StuffPriceDiscrepancies).HasForeignKey(x => x.ConfirmerUserId);
    }
  }
}