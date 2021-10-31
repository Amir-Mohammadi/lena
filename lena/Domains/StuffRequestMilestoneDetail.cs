using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffRequestMilestoneDetail : BaseEntity, IEntity
  {
    protected internal StuffRequestMilestoneDetail()
    {
    }
    public int StuffRequestMilestoneId { get; set; }
    public double Qty { get; set; }
    public int StuffId { get; set; }
    public byte UnitId { get; set; }
    public StuffRequestMilestoneDetailStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StuffRequestMilestone StuffRequestMilestone { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual StuffRequestMilestoneDetailSummary StuffRequestMilestoneDetailSummary { get; set; }
  }
}