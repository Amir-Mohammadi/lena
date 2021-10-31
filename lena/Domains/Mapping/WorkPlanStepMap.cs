using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WorkPlanStepMap : IEntityTypeConfiguration<WorkPlanStep>
  {
    public void Configure(EntityTypeBuilder<WorkPlanStep> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("WorkPlanSteps");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title);
      builder.Property(x => x.ProductionLineId);
      builder.Property(x => x.SwitchTime);
      builder.Property(x => x.InitialTime);
      builder.Property(x => x.BatchTime);
      builder.Property(x => x.BatchCount);
      builder.Property(x => x.WorkPlanId);
      builder.Property(x => x.NeedToQualityControl);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.PlanningWithoutMachineLimit);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.Property(x => x.ProductWarehouseId);
      builder.Property(x => x.ConsumeWarehouseId);
      builder.Property(x => x.ProductionStepId);
      builder.HasOne(x => x.WorkPlan).WithMany(x => x.WorkPlanSteps).HasForeignKey(x => x.WorkPlanId);
      builder.HasOne(x => x.ProductionLineProductionStep).WithMany(x => x.WorkPlanSteps).HasForeignKey(x => new
      {
        x.ProductionLineId,
        x.ProductionStepId
      });
      builder.HasOne(x => x.ProductionLine).WithMany(x => x.WorkPlanSteps).HasForeignKey(x => x.ProductionLineId);
      builder.HasOne(x => x.ProductWarehouse).WithMany(x => x.ProductWorkPlanSteps).HasForeignKey(x => x.ProductWarehouseId);
      builder.HasOne(x => x.ConsumeWarehouse).WithMany(x => x.ConsumeWorkPlanSteps).HasForeignKey(x => x.ConsumeWarehouseId);
      builder.HasOne(x => x.ProductionStep).WithMany(x => x.WorkPlanSteps).HasForeignKey(x => x.ProductionStepId);
    }
  }
}