using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WorkPlanStep : IEntity
  {
    protected internal WorkPlanStep()
    {
      this.ProductionOrders = new HashSet<ProductionOrder>();
      this.OperationSequences = new HashSet<OperationSequence>();
      this.ProductionSchedules = new HashSet<ProductionSchedule>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public int ProductionLineId { get; set; }
    public long SwitchTime { get; set; }
    public long InitialTime { get; set; }
    public long BatchTime { get; set; }
    public double BatchCount { get; set; }
    public int WorkPlanId { get; set; }
    public bool NeedToQualityControl { get; set; }
    public bool IsActive { get; set; }
    public bool WorkPlanIsPublished { get; set; }
    public bool PlanningWithoutMachineLimit { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<short> ProductWarehouseId { get; set; }
    public Nullable<short> ConsumeWarehouseId { get; set; }
    public int ProductionStepId { get; set; }
    public virtual WorkPlan WorkPlan { get; set; }
    public virtual ICollection<ProductionOrder> ProductionOrders { get; set; }
    public virtual ICollection<OperationSequence> OperationSequences { get; set; }
    public virtual ICollection<ProductionSchedule> ProductionSchedules { get; set; }
    public virtual ProductionLineProductionStep ProductionLineProductionStep { get; set; }
    public virtual ProductionLine ProductionLine { get; set; }
    public virtual Warehouse ProductWarehouse { get; set; }
    public virtual Warehouse ConsumeWarehouse { get; set; }
    public virtual ProductionStep ProductionStep { get; set; }
  }
}