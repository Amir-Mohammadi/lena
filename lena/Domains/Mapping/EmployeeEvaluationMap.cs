using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EmployeeEvaluationMap : IEntityTypeConfiguration<EmployeeEvaluation>
  {
    public void Configure(EntityTypeBuilder<EmployeeEvaluation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("EmployeeEvaluations");
      builder.Property(x => x.Id);
      builder.Property(x => x.EmployeeId).IsRequired();
      builder.Property(x => x.EmployeeEvaluationPeriodId).IsRequired();
      builder.Property(x => x.CreatedUserId).IsRequired();
      builder.Property(x => x.CreatedDateTime).HasColumnType("smalldatetime").IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.Employee).WithMany(x => x.EmployeeEvaluations).HasForeignKey(x => x.EmployeeId);
      builder.HasOne(x => x.User).WithMany(x => x.EmployeeEvaluations).HasForeignKey(x => x.CreatedUserId);
      builder.HasOne(x => x.EmployeeEvaluationPeriod).WithMany(x => x.EmployeeEvaluations).HasForeignKey(x => x.EmployeeEvaluationPeriodId);
    }
  }
}
