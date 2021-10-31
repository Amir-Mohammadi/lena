using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionPlanSummary : IEntity
  {
    protected internal ProductionPlanSummary()
    {
    }
    public int Id { get; set; }
    public double ProducedQty { get; set; }
    public double ScheduledQty { get; set; }
    public int ProductionPlanId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionPlan ProductionPlan { get; set; }
  }
}
