using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ReportVersionMap : IEntityTypeConfiguration<ReportVersion>
  {
    public void Configure(EntityTypeBuilder<ReportVersion> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ReportVersions");
      builder.Property(x => x.Id);
      builder.Property(x => x.ApiUrl).IsRequired();
      builder.Property(x => x.ReportContent).IsRequired();
      builder.Property(x => x.IsPublished);
      builder.Property(x => x.CreationTime).HasColumnType("smalldatetime");
      builder.Property(x => x.CreatorUserId);
      builder.HasRowVersion();
      builder.Property(x => x.ReportId);
      builder.Property(x => x.CultureName);
      builder.Property(x => x.IsForExport);
      builder.Property(x => x.ExportFormat);
      builder.Property(x => x.IsBarcodeTemplate);
      builder.HasOne(x => x.CreatorUser).WithMany(x => x.ReportVersions).HasForeignKey(x => x.CreatorUserId);
      builder.HasOne(x => x.Report).WithMany(x => x.ReportVersions).HasForeignKey(x => x.ReportId);
    }
  }
}
