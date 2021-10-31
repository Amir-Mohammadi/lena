using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffQualityControlTestConditionMap : IEntityTypeConfiguration<StuffQualityControlTestCondition>
  {
    public void Configure(EntityTypeBuilder<StuffQualityControlTestCondition> builder)
    {
      builder.HasKey(x => new
      {
        x.StuffId,
        x.QualityControlTestId,
        x.QualityControlConditionTestConditionId,
        x.QualityControlTestConditionQualityControlTestId
      });
      builder.ToTable("StuffQualityControlTestConditions");
      builder.Property(x => x.StuffId);
      builder.Property(x => x.QualityControlTestId);
      builder.Property(x => x.QualityControlConditionTestConditionId);
      builder.Property(x => x.QualityControlTestConditionQualityControlTestId);
      builder.Property(x => x.QualityControlTestUnitId);
      builder.Property(x => x.Min);
      builder.Property(x => x.Max);
      builder.Property(x => x.Description);
      builder.Property(x => x.ToleranceType);
      builder.Property(x => x.AcceptanceLimit);
      builder.HasRowVersion();
      builder.HasOne(x => x.QualityControlTestUnit).WithMany(x => x.StuffQualityControlTestConditions).HasForeignKey(x => x.QualityControlTestUnitId);
      builder.HasOne(x => x.StuffQualityControlTest).WithMany(x => x.StuffQualityControlTestConditions).HasForeignKey(x => new { x.StuffId, x.QualityControlTestId });
      builder.HasOne(x => x.QualityControlTestCondition).WithMany(x => x.StuffQualityControlTestConditions).HasForeignKey(x => new { x.QualityControlConditionTestConditionId, x.QualityControlTestConditionQualityControlTestId });
    }
  }
}
