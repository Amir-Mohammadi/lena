using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionOrderSummary : IEntity
  {
    protected internal ProductionOrderSummary()
    {
    }
    public int Id { get; set; }
    public double ProducedQty { get; set; }
    public double InProductionQty { get; set; }
    public string Description { get; set; }
    public int ProductionOrderId { get; internal set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionOrder ProductionOrder { get; set; }
  }
}
