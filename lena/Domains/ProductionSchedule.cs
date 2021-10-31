using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionSchedule : BaseEntity, IEntity
  {
    protected internal ProductionSchedule()
    {
      this.ProductionOrders = new HashSet<ProductionOrder>();
    }
    public int ProductionPlanDetailId { get; set; }
    public double Qty { get; set; }
    public bool ApplySwitchTime { get; set; }
    public int SwitchTime { get; set; }
    public bool PlanningWithoutMachineLimit { get; set; }
    public int OperatorCount { get; set; }
    public ProductionScheduleStatus Status { get; set; }
    public bool IsPublished { get; set; }
    public int WorkPlanStepId { get; set; }
    public int CalendarEventId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual CalendarEvent CalendarEvent { get; set; }
    public virtual ICollection<ProductionOrder> ProductionOrders { get; set; }
    public virtual ProductionPlanDetail ProductionPlanDetail { get; set; }
    public virtual WorkPlanStep WorkPlanStep { get; set; }
    public virtual ProductionScheduleSummary ProductionScheduleSummary { get; set; }
  }
}