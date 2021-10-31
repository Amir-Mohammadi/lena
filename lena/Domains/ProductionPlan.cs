using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionPlan : BaseEntity, IEntity
  {
    protected internal ProductionPlan()
    {
      this.ProductionPlanDetails = new HashSet<ProductionPlanDetail>();
    }
    public Nullable<int> ProductionRequestId { get; set; }
    public DateTime EstimatedDate { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public ProductionPlanStatus Status { get; set; }
    public bool IsTemporary { get; set; }
    public short BillOfMaterialVersion { get; set; }
    public int BillOfMaterialStuffId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionRequest ProductionRequest { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual ICollection<ProductionPlanDetail> ProductionPlanDetails { get; set; }
    public virtual ProductionPlanSummary ProductionPlanSummary { get; set; }
    public virtual BillOfMaterial BillOfMaterial { get; set; }
  }
}
