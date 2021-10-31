using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QualityControlConfirmationTestItemMap : IEntityTypeConfiguration<QualityControlConfirmationTestItem>
  {
    public void Configure(EntityTypeBuilder<QualityControlConfirmationTestItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("QualityControlConfirmationTestItems");
      builder.Property(x => x.Id);
      builder.Property(x => x.SampleName);
      builder.Property(x => x.QualityControlConfirmationTestId);
      builder.Property(x => x.ObtainAmount);
      builder.Property(x => x.MaxObtainAmount);
      builder.Property(x => x.MinObtainAmount);
      builder.Property(x => x.TesterUserId);
      builder.HasRowVersion();
      builder.HasOne(x => x.QualityControlConfirmationTest).WithMany(x => x.QualityControlConfirmationTestItems).HasForeignKey(x => x.QualityControlConfirmationTestId);
      builder.HasOne(x => x.TesterUser).WithMany(x => x.QualityControlConfirmationTestItems).HasForeignKey(x => x.TesterUserId);
    }
  }
}
