using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectERPTaskDependencyMap : IEntityTypeConfiguration<ProjectERPTaskDependency>
  {
    public void Configure(EntityTypeBuilder<ProjectERPTaskDependency> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProjectERPTaskDependencies");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProjectERPTaskId);
      builder.Property(x => x.PredecessorProjectERPTaskId);
      builder.Property(x => x.DependencyType);
      builder.Property(x => x.LagMinutues);
      builder.HasRowVersion();
      builder.HasOne(x => x.ProjectERPTask).WithMany(x => x.ProjectERPTaskDependencies).HasForeignKey(x => x.ProjectERPTaskId);
      builder.HasOne(x => x.PredecessorProjectERPTask).WithMany(x => x.PredecessorProjectERPTaskDependency).HasForeignKey(x => x.PredecessorProjectERPTaskId);
    }
  }
}
