using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffRequestMilestoneDetailSummary : IEntity
  {
    protected internal StuffRequestMilestoneDetailSummary()
    {
    }
    public int Id { get; set; }
    public double OrderedQty { get; set; }
    public double CargoedQty { get; set; }
    public double ReciptedQty { get; set; }
    public int StuffRequestMilestoneDetailId { get; set; }
    public double QualityControlPassedQty { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StuffRequestMilestoneDetail StuffRequestMilestoneDetail { get; set; }
  }
}