using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EmployeeWorkReportItemMap : IEntityTypeConfiguration<EmployeeWorkReportItem>
  {
    public void Configure(EntityTypeBuilder<EmployeeWorkReportItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("EmployeeWorkReportItems");
      builder.Property(x => x.Id);
      builder.Property(x => x.EmployeeWorkReportId).IsRequired();
      builder.Property(x => x.FromTime).IsRequired();
      builder.Property(x => x.ToTime).IsRequired();
      builder.Property(x => x.Operation).IsRequired();
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.Description).IsRequired(false);
      builder.Property(x => x.Operation).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.EmployeeWorkReport).WithMany(x => x.EmployeeWorkReportItems).HasForeignKey(x => x.EmployeeWorkReportId);
    }
  }
}