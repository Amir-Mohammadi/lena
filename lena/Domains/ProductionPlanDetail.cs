using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionPlanDetail : BaseEntity, IEntity
  {
    protected internal ProductionPlanDetail()
    {
      this.ProductionSchedules = new HashSet<ProductionSchedule>();
      this.EquivalentStuffUsages = new HashSet<EquivalentStuffUsage>();
    }
    public int ProductionPlanId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public int ProductionPlanDetailLevelId { get; set; }
    public ProductionPlanDetailStatus Status { get; set; }
    public short BillOfMaterialVersion { get; set; }
    public int BillOfMaterialStuffId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionPlan ProductionPlan { get; set; }
    public virtual ICollection<ProductionSchedule> ProductionSchedules { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual ICollection<EquivalentStuffUsage> EquivalentStuffUsages { get; set; }
    public virtual ProductionPlanDetailLevel ProductionPlanDetailLevel { get; set; }
    public virtual ProductionPlanDetailSummary ProductionPlanDetailSummary { get; set; }
    public virtual BillOfMaterial BillOfMaterial { get; set; }
  }
}
