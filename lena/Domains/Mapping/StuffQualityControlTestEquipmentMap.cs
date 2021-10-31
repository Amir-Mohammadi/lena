using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffQualityControlTestEquipmentMap : IEntityTypeConfiguration<StuffQualityControlTestEquipment>
  {
    public void Configure(EntityTypeBuilder<StuffQualityControlTestEquipment> builder)
    {
      builder.HasKey(x => new
      {
        x.StuffId,
        x.QualityControlTestId,
        x.QualityControlEquipmentTestEquipmentId,
        x.QualityControlTestEquipmentQualityControlTestId
      });
      builder.ToTable("StuffQualityControlTestEquipments");
      builder.Property(x => x.StuffId);
      builder.Property(x => x.QualityControlTestId);
      builder.Property(x => x.QualityControlEquipmentTestEquipmentId);
      builder.Property(x => x.QualityControlTestEquipmentQualityControlTestId);
      builder.HasRowVersion();
      builder.HasOne(x => x.StuffQualityControlTest).WithMany(x => x.StuffQualityControlTestEquipments).HasForeignKey(x => new { x.StuffId, x.QualityControlTestId });
      builder.HasOne(x => x.QualityControlTestEquipment).WithMany(x => x.StuffQualityControlTestEquipments).HasForeignKey(x => new { x.QualityControlEquipmentTestEquipmentId, x.QualityControlTestEquipmentQualityControlTestId });
    }
  }
}
