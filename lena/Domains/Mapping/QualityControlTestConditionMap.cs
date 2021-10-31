using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QualityControlTestConditionMap : IEntityTypeConfiguration<QualityControlTestCondition>
  {
    public void Configure(EntityTypeBuilder<QualityControlTestCondition> builder)
    {
      builder.HasKey(x => new
      {
        x.TestConditionId,
        x.QualityControlTestId
      });
      builder.ToTable("QualityControlTestConditions");
      builder.Property(x => x.TestConditionId);
      builder.Property(x => x.QualityControlTestId);
      builder.HasRowVersion();
      builder.HasOne(x => x.QualityControlTest).WithMany(x => x.QualityControlTestConditions).HasForeignKey(x => x.QualityControlTestId);
      builder.HasOne(x => x.TestCondition).WithMany(x => x.QualityControlTestConditions).HasForeignKey(x => x.TestConditionId);
    }
  }
}
