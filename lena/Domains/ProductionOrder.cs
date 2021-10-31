using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionOrder : BaseEntity, IEntity
  {
    protected internal ProductionOrder()
    {
      this.Productions = new HashSet<Production>();
      this.ProductionOperators = new HashSet<ProductionOperator>();
      this.ProductionMaterialRequests = new HashSet<ProductionMaterialRequest>();
      this.EquivalentStuffUsages = new HashSet<EquivalentStuffUsage>();
      this.ProductionPerformanceInfoes = new HashSet<ProductionPerformanceInfo>();
      this.SerialFailedOperations = new HashSet<SerialFailedOperation>();
      this.StuffSerials = new HashSet<StuffSerial>();
    }
    public Nullable<int> ProductionScheduleId { get; set; }
    public double Qty { get; set; }
    public int WorkPlanStepId { get; set; }
    public byte UnitId { get; set; }
    public int SupervisorEmployeeId { get; set; }
    public ProductionOrderStatus Status { get; set; }
    public double ToleranceQty { get; set; }
    public int? CalendarEventId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<Production> Productions { get; set; }
    public virtual ProductionSchedule ProductionSchedule { get; set; }
    public virtual WorkPlanStep WorkPlanStep { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual ICollection<ProductionOperator> ProductionOperators { get; set; }
    public virtual CalendarEvent CalendarEvent { get; set; }
    public virtual ICollection<ProductionMaterialRequest> ProductionMaterialRequests { get; set; }
    public virtual ICollection<EquivalentStuffUsage> EquivalentStuffUsages { get; set; }
    public virtual ProductionOrderSummary ProductionOrderSummary { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual ICollection<ProductionPerformanceInfo> ProductionPerformanceInfoes { get; set; }
    public virtual ICollection<SerialFailedOperation> SerialFailedOperations { get; set; }
    public virtual ICollection<ProductionMaterialRequestDetail> ProductionMaterialRequestDetails { get; set; }
    public virtual ICollection<StuffSerial> StuffSerials { get; set; }

  }
}
