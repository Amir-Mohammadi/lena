using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionScheduleMap : IEntityTypeConfiguration<ProductionSchedule>
  {
    public void Configure(EntityTypeBuilder<ProductionSchedule> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ProductionSchedule");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProductionPlanDetailId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.ApplySwitchTime);
      builder.Property(x => x.SwitchTime);
      builder.Property(x => x.PlanningWithoutMachineLimit);
      builder.Property(x => x.OperatorCount);
      builder.Property(x => x.Status);
      builder.Property(x => x.IsPublished);
      builder.Property(x => x.WorkPlanStepId);
      builder.Property(x => x.CalendarEventId);
      builder.HasOne(x => x.ProductionPlanDetail).WithMany(x => x.ProductionSchedules).HasForeignKey(x => x.ProductionPlanDetailId);
      builder.HasOne(x => x.WorkPlanStep).WithMany(x => x.ProductionSchedules).HasForeignKey(x => x.WorkPlanStepId);
      builder.HasOne(x => x.ProductionScheduleSummary).WithOne(x => x.ProductionSchedule).HasForeignKey<ProductionScheduleSummary>(x => x.ProductionScheduleId);
    }
  }
}