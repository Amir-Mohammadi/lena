using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QualityControlTestImportanceDegreeMap : IEntityTypeConfiguration<QualityControlTestImportanceDegree>
  {
    public void Configure(EntityTypeBuilder<QualityControlTestImportanceDegree> builder)
    {
      builder.HasKey(x => new
      {
        x.TestImportanceDegreeId,
        x.QualityControlTestId
      });
      builder.ToTable("QualityControlTestImportanceDegrees");
      builder.Property(x => x.TestImportanceDegreeId);
      builder.Property(x => x.QualityControlTestId);
      builder.HasRowVersion();
      builder.HasOne(x => x.QualityControlTest).WithMany(x => x.QualityControlTestImportanceDegrees).HasForeignKey(x => x.QualityControlTestId);
      builder.HasOne(x => x.TestImportanceDegree).WithMany(x => x.QualityControlTestImportanceDegrees).HasForeignKey(x => x.TestImportanceDegreeId);
    }
  }
}
