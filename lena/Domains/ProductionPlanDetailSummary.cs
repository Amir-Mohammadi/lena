using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionPlanDetailSummary : IEntity
  {
    protected internal ProductionPlanDetailSummary()
    {
    }
    public int Id { get; set; }
    public double ProducedQty { get; set; }
    public double ScheduledQty { get; set; }
    public int ProductionPlanDetailId { get; internal set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionPlanDetail ProductionPlanDetail { get; set; }
  }
}
