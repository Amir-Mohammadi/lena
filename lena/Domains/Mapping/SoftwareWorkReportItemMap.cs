using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class SoftwareWorkReportItemMap : IEntityTypeConfiguration<SoftwareWorkReportItem>
  {
    public void Configure(EntityTypeBuilder<SoftwareWorkReportItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("SoftwareWorkReportItems");
      builder.Property(x => x.Id);
      builder.Property(x => x.SoftwareWorkReportId);
      builder.Property(x => x.Spent);
      builder.Property(x => x.Issue).IsRequired();
      builder.Property(x => x.Estimated);
      builder.HasRowVersion();
      builder.HasOne(x => x.SoftwareWorkReport).WithMany(x => x.SoftwareWorkReportItems).HasForeignKey(x => x.SoftwareWorkReportId);
    }
  }
}
