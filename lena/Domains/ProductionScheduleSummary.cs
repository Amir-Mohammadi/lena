using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionScheduleSummary : IEntity
  {
    protected internal ProductionScheduleSummary()
    {
    }
    public int Id { get; set; }
    public double ProducedQty { get; set; }
    public int ProductionScheduleId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionSchedule ProductionSchedule { get; set; }
  }
}