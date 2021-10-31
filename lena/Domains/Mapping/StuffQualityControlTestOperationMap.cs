using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffQualityControlTestOperationMap : IEntityTypeConfiguration<StuffQualityControlTestOperation>
  {
    public void Configure(EntityTypeBuilder<StuffQualityControlTestOperation> builder)
    {
      builder.HasKey(x => new
      {
        x.StuffId,
        x.QualityControlTestId,
        x.QualityControlOperationTestOperationId,
        x.QualityControlTestOperationQualityControlTestId
      });
      builder.ToTable("StuffQualityControlTestOperations");
      builder.Property(x => x.StuffId);
      builder.Property(x => x.QualityControlTestId);
      builder.Property(x => x.QualityControlOperationTestOperationId);
      builder.Property(x => x.QualityControlTestOperationQualityControlTestId);
      builder.HasRowVersion();
      builder.HasOne(x => x.StuffQualityControlTest).WithMany(x => x.StuffQualityControlTestOperations).HasForeignKey(x => new { x.StuffId, x.QualityControlTestId });
      builder.HasOne(x => x.QualityControlTestOperation).WithMany(x => x.StuffQualityControlTestOperations).HasForeignKey(x => new { x.QualityControlOperationTestOperationId, x.QualityControlTestOperationQualityControlTestId });
    }
  }
}
