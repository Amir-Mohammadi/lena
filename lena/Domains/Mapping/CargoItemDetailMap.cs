using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CargoItemDetailMap : IEntityTypeConfiguration<CargoItemDetail>
  {
    public void Configure(EntityTypeBuilder<CargoItemDetail> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_CargoItemDetail");
      builder.Property(x => x.Id);
      builder.Property(x => x.CargoItemId);
      builder.Property(x => x.PurchaseOrderDetailId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.HasOne(x => x.CargoItem).WithMany(x => x.CargoItemDetails).HasForeignKey(x => x.CargoItemId);
      builder.HasOne(x => x.PurchaseOrderDetail).WithMany(x => x.CargoItemDetails).HasForeignKey(x => x.PurchaseOrderDetailId);
      builder.HasOne(x => x.Unit).WithMany(x => x.CargoItemDetails).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.CargoItemDetailSummary).WithOne(x => x.CargoItemDetail).HasForeignKey<CargoItemDetailSummary>(x => x.CargoItemDetailId);
    }
  }
}