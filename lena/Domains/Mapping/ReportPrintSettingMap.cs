using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ReportPrintSettingMap : IEntityTypeConfiguration<ReportPrintSetting>
  {
    public void Configure(EntityTypeBuilder<ReportPrintSetting> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ReportPrintSettings");
      builder.Property(x => x.Id);
      builder.Property(x => x.ReportId);
      builder.Property(x => x.PrinterId);
      builder.Property(x => x.CreationTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
      builder.Property(x => x.UserId);
      builder.Property(x => x.ShowPreview);
      builder.Property(x => x.ShowPrintDialog);
      builder.Property(x => x.NumberOfCopies);
    }
  }
}
