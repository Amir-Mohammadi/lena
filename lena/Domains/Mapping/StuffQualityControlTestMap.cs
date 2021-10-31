using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffQualityControlTestMap : IEntityTypeConfiguration<StuffQualityControlTest>
  {
    public void Configure(EntityTypeBuilder<StuffQualityControlTest> builder)
    {
      builder.HasKey(x => new
      {
        x.StuffId,
        x.QualityControlTestId
      });
      builder.ToTable("StuffQualityControlTests");
      builder.Property(x => x.StuffId);
      builder.Property(x => x.QualityControlTestId);
      builder.Property(x => x.DocumentId);
      builder.Property(x => x.QualityControlTestConditionTestConditionId);
      builder.Property(x => x.QualityControlTestConditionQualityControlTestId);
      builder.Property(x => x.QualityControlTestEquipmentTestEquipmentId);
      builder.Property(x => x.QualityControlTestEquipmentQualityControlTestId);
      builder.Property(x => x.QualityControlTestOperationTestOperationId);
      builder.Property(x => x.QualityControlTestOperationQualityControlTestId);
      builder.Property(x => x.QualityControlTestImportanceDegreeTestImportanceDegreeId);
      builder.Property(x => x.QualityControlTestImportanceDegreeQualityControlTestId);
      builder.Property(x => x.MeasurementMethod);
      builder.Property(x => x.Frequency);
      builder.Property(x => x.CorrectiveReaction);
      builder.HasRowVersion();
      builder.HasOne(x => x.Stuff).WithMany(x => x.StuffQualityControlTests).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.QualityControlTest).WithMany(x => x.StuffQualityControlTests).HasForeignKey(x => x.QualityControlTestId);
      builder.HasOne(x => x.QualityControlTestCondition).WithMany(x => x.StuffQualityControlTests).HasForeignKey(x => new { x.QualityControlTestConditionTestConditionId, x.QualityControlTestConditionQualityControlTestId });
      builder.HasOne(x => x.QualityControlTestEquipment).WithMany(x => x.StuffQualityControlTests).HasForeignKey(x => new { x.QualityControlTestEquipmentTestEquipmentId, x.QualityControlTestEquipmentQualityControlTestId });
      builder.HasOne(x => x.QualityControlTestOperation).WithMany(x => x.StuffQualityControlTests).HasForeignKey(x => new { x.QualityControlTestOperationTestOperationId, x.QualityControlTestOperationQualityControlTestId });
      builder.HasOne(x => x.QualityControlTestImportanceDegree).WithMany(x => x.StuffQualityControlTests).HasForeignKey(x => new { x.QualityControlTestImportanceDegreeTestImportanceDegreeId, x.QualityControlTestImportanceDegreeQualityControlTestId });
    }
  }
}
