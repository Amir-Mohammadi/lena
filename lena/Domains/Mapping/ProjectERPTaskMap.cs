using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectERPTaskMap : IEntityTypeConfiguration<ProjectERPTask>
  {
    public void Configure(EntityTypeBuilder<ProjectERPTask> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProjectERPTaskes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title);
      builder.Property(x => x.Output);
      builder.Property(x => x.Status);
      builder.Property(x => x.StartDateTime).HasColumnType("smalldatetime").IsRequired(false).HasColumnType("smalldatetime");
      builder.Property(x => x.DueDateTime).HasColumnType("smalldatetime").IsRequired(false).HasColumnType("smalldatetime");
      builder.Property(x => x.EstimateTime);
      builder.Property(x => x.DurationMinute).IsRequired(false);
      builder.Property(x => x.ProgressPercentage).IsRequired(false);
      builder.Property(x => x.Priority);
      builder.Property(x => x.ProjectERPId);
      builder.Property(x => x.AssigneeEmployeeId).IsRequired(false);
      builder.Property(x => x.ProjectERPTaskCategoryId);
      builder.Property(x => x.CreateDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.CreatorUser).WithMany(x => x.ProjectERPTasks).HasForeignKey(x => x.CreatorUserId);
      builder.HasOne(x => x.AssigneeEmployee).WithMany(x => x.ProjectERPTasks).HasForeignKey(x => x.AssigneeEmployeeId);
      builder.HasOne(x => x.ProjectERP).WithMany(x => x.ProjectERPTasks).HasForeignKey(x => x.ProjectERPId);
      builder.HasOne(x => x.ProjectERPTaskCategory).WithMany(x => x.ProjectERPTasks).HasForeignKey(x => x.ProjectERPTaskCategoryId);
    }
  }
}