using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QualityControlTestOperationMap : IEntityTypeConfiguration<QualityControlTestOperation>
  {
    public void Configure(EntityTypeBuilder<QualityControlTestOperation> builder)
    {
      builder.HasKey(x => new
      {
        x.TestOperationId,
        x.QualityControlTestId
      });
      builder.ToTable("QualityControlTestOperations");
      builder.Property(x => x.TestOperationId);
      builder.Property(x => x.QualityControlTestId);
      builder.HasRowVersion();
      builder.HasOne(x => x.QualityControlTest).WithMany(x => x.QualityControlTestOperations).HasForeignKey(x => x.QualityControlTestId);
      builder.HasOne(x => x.TestOperation).WithMany(x => x.QualityControlTestOperations).HasForeignKey(x => x.TestOperationId);
    }
  }
}
