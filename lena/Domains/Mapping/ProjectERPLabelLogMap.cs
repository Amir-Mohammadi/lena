using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectERPLabelLogMap : IEntityTypeConfiguration<ProjectERPLabelLog>
  {
    public void Configure(EntityTypeBuilder<ProjectERPLabelLog> builder)
    {
      builder.HasKey(x => new
      {
        x.ProjectERPId,
        x.ProjectERPLabelId
      });
      builder.ToTable("ProjectERPLabelLogs");
      builder.Property(x => x.ProjectERPId);
      builder.Property(x => x.ProjectERPLabelId);
      builder.HasRowVersion();
      builder.HasOne(x => x.ProjectERP).WithMany(x => x.ProjectERPLabelLogs).HasForeignKey(x => x.ProjectERPId);
      builder.HasOne(x => x.ProjectERPLabel).WithMany(x => x.ProjectERPLabelLogs).HasForeignKey(x => x.ProjectERPLabelId);
    }
  }
}