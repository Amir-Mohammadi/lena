using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionOrderMap : IEntityTypeConfiguration<ProductionOrder>
  {
    public void Configure(EntityTypeBuilder<ProductionOrder> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ProductionOrder");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProductionScheduleId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.WorkPlanStepId);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.Status);
      builder.Property(x => x.ToleranceQty);
      builder.Property(x => x.CalendarEventId);
      builder.HasOne(x => x.ProductionSchedule).WithMany(x => x.ProductionOrders).HasForeignKey(x => x.ProductionScheduleId);
      builder.HasOne(x => x.WorkPlanStep).WithMany(x => x.ProductionOrders).HasForeignKey(x => x.WorkPlanStepId);
      builder.HasOne(x => x.Unit).WithMany(x => x.ProductionOrders).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.CalendarEvent).WithOne(x => x.ProductionOrder).HasForeignKey<ProductionOrder>(x => x.CalendarEventId);
      builder.HasOne(x => x.ProductionOrderSummary).WithOne(x => x.ProductionOrder).HasForeignKey<ProductionOrderSummary>(x => x.ProductionOrderId);
      builder.HasOne(x => x.Employee).WithMany(x => x.ProductionOrders).HasForeignKey(x => x.SupervisorEmployeeId);
    }
  }
}