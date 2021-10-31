using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EmployeeEvaluationItemMap : IEntityTypeConfiguration<EmployeeEvaluationItem>
  {
    public void Configure(EntityTypeBuilder<EmployeeEvaluationItem> builder)
    {
      builder.HasKey(x => new
      {
        x.EmployeeEvaluationId,
        x.EvaluationCategoryId
      });
      builder.ToTable("EmployeeEvaluationItems");
      builder.Property(x => x.EmployeeEvaluationId);
      builder.Property(x => x.EvaluationCategoryId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.Status).IsRequired();
      builder.Property(x => x.Description).IsRequired(false);
      builder.Property(x => x.PermanentDateTime).HasColumnType("smalldatetime").IsRequired(false);
      builder.HasRowVersion();
      builder.HasOne(x => x.EmployeeEvaluation).WithMany(x => x.EmployeeEvaluationItems).HasForeignKey(x => x.EmployeeEvaluationId);
      builder.HasOne(x => x.EvaluationCategory).WithMany(x => x.EmployeeEvaluationItems).HasForeignKey(x => x.EvaluationCategoryId);
      builder.HasOne(x => x.User).WithMany(x => x.EmployeeEvaluationItems).HasForeignKey(x => x.UserId);
    }
  }
}
