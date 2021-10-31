using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffQualityControlTestImportanceDegreeMap : IEntityTypeConfiguration<StuffQualityControlTestImportanceDegree>
  {
    public void Configure(EntityTypeBuilder<StuffQualityControlTestImportanceDegree> builder)
    {
      builder.HasKey(x => new
      {
        x.StuffId,
        x.QualityControlTestId,
        x.QualityControlImportanceDegreeTestImportanceDegreeId,
        x.QualityControlTestImportanceDegreeQualityControlTestId
      });
      builder.ToTable("StuffQualityControlTestImportanceDegrees");
      builder.Property(x => x.StuffId);
      builder.Property(x => x.QualityControlTestId);
      builder.Property(x => x.QualityControlImportanceDegreeTestImportanceDegreeId);
      builder.Property(x => x.QualityControlTestImportanceDegreeQualityControlTestId);
      builder.HasRowVersion();
      builder.HasOne(x => x.StuffQualityControlTest).WithMany(x => x.StuffQualityControlTestImportanceDegrees).HasForeignKey(x => new { x.StuffId, x.QualityControlTestId });
      builder.HasOne(x => x.QualityControlTestImportanceDegree).WithMany(x => x.StuffQualityControlTestImportanceDegrees).HasForeignKey(x => new { x.QualityControlImportanceDegreeTestImportanceDegreeId, x.QualityControlTestImportanceDegreeQualityControlTestId });
    }
  }
}
