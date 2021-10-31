using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionRequestSummary : IEntity
  {
    protected internal ProductionRequestSummary()
    {
    }
    public int Id { get; set; }
    public double PlannedQty { get; set; }
    public double ScheduledQty { get; set; }
    public double ProducedQty { get; set; }
    public int ProductionRequestId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionRequest ProductionRequest { get; set; }
  }
}