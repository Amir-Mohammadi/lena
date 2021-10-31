using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CargoItemMap : IEntityTypeConfiguration<CargoItem>
  {
    public void Configure(EntityTypeBuilder<CargoItem> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_CargoItem");
      builder.Property(x => x.Id);
      builder.Property(x => x.CargoId);
      builder.Property(x => x.PurchaseOrderId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.Status);
      builder.Property(x => x.LadingId);
      builder.Property(x => x.HowToBuyId);
      builder.Property(x => x.EstimateDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.CargoItemDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.IsArchived);
      builder.Property(x => x.ForwarderId);
      builder.Property(x => x.ForwarderDocumentId).IsRequired(false);
      builder.Property(x => x.BuyingProcess);
      builder.Property(x => x.LatestRiskId);
      builder.HasOne(x => x.PurchaseOrder).WithMany(x => x.CargoItems).HasForeignKey(x => x.PurchaseOrderId);
      builder.HasOne(x => x.Cargo).WithMany(x => x.CargoItems).HasForeignKey(x => x.CargoId);
      builder.HasOne(x => x.Unit).WithMany(x => x.CargoItems).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.CargoItemSummary).WithOne(x => x.CargoItem).HasForeignKey<CargoItemSummary>(x => x.CargoItemId);
      builder.HasOne(x => x.HowToBuy).WithMany(x => x.CargoItems).HasForeignKey(x => x.HowToBuyId);
      builder.HasOne(x => x.Forwarder).WithMany(x => x.CargoItems).HasForeignKey(x => x.ForwarderId);
      builder.HasOne(x => x.LatestRisk).WithOne().HasForeignKey<CargoItem>(x => x.LatestRiskId);
    }
  }
}