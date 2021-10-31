using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EmployeeWorkReportMap : IEntityTypeConfiguration<EmployeeWorkReport>
  {
    public void Configure(EntityTypeBuilder<EmployeeWorkReport> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("EmployeeWorkReports");
      builder.Property(x => x.Id);
      builder.Property(x => x.UserId);
      builder.Property(x => x.ProjectERPTaskId);
      builder.Property(x => x.EmployeeId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.ReportDateTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.EmployeeWorkReports).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.Employee).WithMany(x => x.EmployeeWorkReports).HasForeignKey(x => x.EmployeeId);
      builder.HasOne(x => x.ProjectERPTask).WithMany(x => x.EmployeeWorkReports).HasForeignKey(x => x.ProjectERPTaskId);
    }
  }
}
