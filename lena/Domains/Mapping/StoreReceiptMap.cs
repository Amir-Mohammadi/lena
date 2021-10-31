using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StoreReceiptMap : IEntityTypeConfiguration<StoreReceipt>
  {
    public void Configure(EntityTypeBuilder<StoreReceipt> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_StoreReceipt");
      builder.Property(x => x.Id);
      builder.Property(x => x.CooperatorId);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.BillOfMaterialVersion);
      builder.Property(x => x.Amount);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.ReceiptId);
      builder.Property(x => x.InboundCargoId);
      builder.Property(x => x.WarehouseId);
      builder.Property(x => x.StoreReceiptType);
      builder.Property(x => x.StuffNeedToQualityControl);
      builder.Property(x => x.CurrentPurchasePriceId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.StoreReceipts).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Unit).WithMany(x => x.StoreReceipts).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.Cooperator).WithMany(x => x.StoreReceipts).HasForeignKey(x => x.CooperatorId);
      builder.HasOne(x => x.StoreReceiptSerialProfile).WithOne(x => x.StoreReceipt).HasForeignKey<StoreReceiptSerialProfile>(x => x.StoreReceiptId);
      builder.HasOne(x => x.BillOfMaterial).WithMany(x => x.StoreReceipts).HasForeignKey(x => new { x.BillOfMaterialVersion, x.StuffId });
      builder.HasOne(x => x.Receipt).WithMany(x => x.StoreReceipts).HasForeignKey(x => x.ReceiptId);
      builder.HasOne(x => x.CurrentPurchasePrice).WithOne(x => x.ActiveForStoreReceipt).HasForeignKey<StoreReceipt>(x => x.CurrentPurchasePriceId);
      builder.HasOne(x => x.InboundCargo).WithMany(x => x.StoreReceipts).HasForeignKey(x => x.InboundCargoId);
      builder.HasOne(x => x.Warehouse).WithMany(x => x.StoreReceipts).HasForeignKey(x => x.WarehouseId);
      builder.HasOne(x => x.StoreReceiptSummary).WithOne(x => x.StoreReceipt).HasForeignKey<StoreReceiptSummary>(x => x.StoreReceiptId);
    }
  }
}