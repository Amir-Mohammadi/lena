using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class SoftwareWorkReportMap : IEntityTypeConfiguration<SoftwareWorkReport>
  {
    public void Configure(EntityTypeBuilder<SoftwareWorkReport> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("SoftwareWorkReports");
      builder.Property(x => x.Id);
      builder.Property(x => x.EmployeeId);
      builder.Property(x => x.ReportDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.CreatedDateTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
      builder.HasOne(x => x.Employee).WithMany(x => x.SoftwareWorkReports).HasForeignKey(x => x.EmployeeId);
    }
  }
}
