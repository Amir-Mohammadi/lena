using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QualityControlConfirmationTestMap : IEntityTypeConfiguration<QualityControlConfirmationTest>
  {
    public void Configure(EntityTypeBuilder<QualityControlConfirmationTest> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("QualityControlConfirmationTests");
      builder.Property(x => x.Id);
      builder.Property(x => x.UserId);
      builder.Property(x => x.Status);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.StuffId);
      builder.Property(x => x.QualityControlTestId);
      builder.Property(x => x.TestConditionId);
      builder.Property(x => x.QualityControlConfirmationId);
      builder.Property(x => x.Description);
      builder.Property(x => x.AQLAmount);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.QualityControlConfirmationTests).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.TestCondition).WithMany(x => x.QualityControlConfirmationTests).HasForeignKey(x => x.TestConditionId);
      builder.HasOne(x => x.QualityControlConfirmation).WithMany(x => x.QualityControlConfirmationTests).HasForeignKey(x => x.QualityControlConfirmationId);
      builder.HasOne(x => x.StuffQualityControlTest).WithMany(x => x.QualityControlConfirmationTests).HasForeignKey(x => new
      {
        x.StuffId,
        x.QualityControlTestId
      });
    }
  }
}
