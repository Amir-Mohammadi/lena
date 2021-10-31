using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QualityControlTestEquipmentMap : IEntityTypeConfiguration<QualityControlTestEquipment>
  {
    public void Configure(EntityTypeBuilder<QualityControlTestEquipment> builder)
    {
      builder.HasKey(x => new
      {
        x.TestEquipmentId,
        x.QualityControlTestId
      });
      builder.ToTable("QualityControlTestEquipments");
      builder.Property(x => x.TestEquipmentId);
      builder.Property(x => x.QualityControlTestId);
      builder.HasRowVersion();
      builder.HasOne(x => x.QualityControlTest).WithMany(x => x.QualityControlTestEquipments).HasForeignKey(x => x.QualityControlTestId);
      builder.HasOne(x => x.TestEquipment).WithMany(x => x.QualityControlTestEquipments).HasForeignKey(x => x.TestEquipmentId);
    }
  }
}
