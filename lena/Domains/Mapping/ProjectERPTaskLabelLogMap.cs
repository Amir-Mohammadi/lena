using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectERPTaskLabelLogMap : IEntityTypeConfiguration<ProjectERPTaskLabelLog>
  {
    public void Configure(EntityTypeBuilder<ProjectERPTaskLabelLog> builder)
    {
      builder.HasKey(x => new
      {
        x.ProjectERPTaskId,
        x.ProjectERPLabelId
      });
      builder.ToTable("ProjectERPTaskLabelLogs");
      builder.Property(x => x.ProjectERPLabelId);
      builder.Property(x => x.ProjectERPTaskId);
      builder.HasRowVersion();
      builder.HasOne(x => x.ProjectERPLabel).WithMany(x => x.ProjectERPTaskLabelLogs).HasForeignKey(x => x.ProjectERPLabelId);
      builder.HasOne(x => x.ProjectERPTask).WithMany(x => x.ProjectERPTaskLabelLogs).HasForeignKey(x => x.ProjectERPTaskId);
    }
  }
}
