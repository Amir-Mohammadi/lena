using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ScrumTaskMap : IEntityTypeConfiguration<ScrumTask>
  {
    public void Configure(EntityTypeBuilder<ScrumTask> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntities_ScrumTask");
      builder.Property(x => x.Id);
      builder.Property(x => x.ScrumTaskTypeId);
      builder.Property(x => x.UserId);
      builder.Property(x => x.SpentTime);
      builder.Property(x => x.RemainedTime);
      builder.Property(x => x.ScrumBackLogId);
      builder.Property(x => x.ScrumTaskStep);
      builder.HasOne(x => x.User).WithMany(x => x.ScrumTasks).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.ScrumBackLog).WithMany(x => x.ScrumTasks).HasForeignKey(x => x.ScrumBackLogId);
      builder.HasOne(x => x.ScrumTaskType).WithMany(x => x.ScrumTasks).HasForeignKey(x => x.ScrumTaskTypeId);
    }
  }
}