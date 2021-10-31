using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionRequest : BaseEntity, IEntity
  {
    protected internal ProductionRequest()
    {
      this.ProductionPlans = new HashSet<ProductionPlan>();
    }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public DateTime DeadlineDate { get; set; }
    public int CheckOrderItemId { get; set; }
    public ProductionRequestStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual ICollection<ProductionPlan> ProductionPlans { get; set; }
    public virtual CheckOrderItem CheckOrderItem { get; set; }
    public virtual ProductionRequestSummary ProductionRequestSummary { get; set; }
  }
}
