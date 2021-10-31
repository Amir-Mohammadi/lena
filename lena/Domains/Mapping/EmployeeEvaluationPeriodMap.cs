using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EmployeeEvaluationPeriodMap : IEntityTypeConfiguration<EmployeeEvaluationPeriod>
  {
    public void Configure(EntityTypeBuilder<EmployeeEvaluationPeriod> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("EmployeeEvaluationPeriods");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.FromDateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.ToDateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.CreatedDateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.Status).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.EmployeeEvaluationPeriods).HasForeignKey(x => x.UserId);
    }
  }
}
