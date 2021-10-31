using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BillOfMaterialDetailMap : IEntityTypeConfiguration<BillOfMaterialDetail>
  {
    public void Configure(EntityTypeBuilder<BillOfMaterialDetail> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BillOfMaterialDetails");
      builder.Property(x => x.Id);
      builder.Property(x => x.Index);
      builder.Property(x => x.Value);
      builder.Property(x => x.Reservable);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.ForQty);
      builder.Property(x => x.IsPackingMaterial);
      builder.Property(x => x.SemiProductBillOfMaterialVersion);
      builder.Property(x => x.BillOfMaterialDetailType);
      builder.HasRowVersion();
      builder.Property(x => x.BillOfMaterialVersion);
      builder.Property(x => x.BillOfMaterialStuffId);
      builder.Property(x => x.Description);
      builder.HasOne(x => x.Unit).WithMany(x => x.BillOfMaterialDetails).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.BillOfMaterialDetails).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.BillOfMaterial).WithMany(x => x.BillOfMaterialDetails).HasForeignKey(x => new
      {
        x.BillOfMaterialVersion,
        x.BillOfMaterialStuffId
      });
      builder.HasOne(x => x.SemiProductBillOfMaterial).WithMany(x => x.UsedInBillOfMaterialDetails).HasForeignKey(x => new { x.SemiProductBillOfMaterialVersion, x.StuffId });
    }
  }
}
