using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EmployeeEvaluationPeriodItemMap : IEntityTypeConfiguration<EmployeeEvaluationPeriodItem>
  {
    public void Configure(EntityTypeBuilder<EmployeeEvaluationPeriodItem> builder)
    {
      builder.HasKey(x => new
      {
        x.EmployeeEvaluationPeriodId,
        x.EvaluationCategoryId
      });
      builder.ToTable("EmployeeEvaluationPeriodItems");
      builder.Property(x => x.EmployeeEvaluationPeriodId);
      builder.Property(x => x.EvaluationCategoryId);
      builder.Property(x => x.Coefficient).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.EmployeeEvaluationPeriod).WithMany(x => x.EmployeeEvaluationPeriodItems).HasForeignKey(x => x.EmployeeEvaluationPeriodId);
      builder.HasOne(x => x.EvaluationCategory).WithMany(x => x.EmployeeEvaluationPeriodItems).HasForeignKey(x => x.EvaluationCategoryId);
    }
  }
}
