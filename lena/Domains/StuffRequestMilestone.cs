using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffRequestMilestone : BaseEntity, IEntity
  {
    protected internal StuffRequestMilestone()
    {
      this.StuffRequestMilestoneDetails = new HashSet<StuffRequestMilestoneDetail>();
    }
    public DateTime DueDate { get; set; }
    public bool IsClosed { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<StuffRequestMilestoneDetail> StuffRequestMilestoneDetails { get; set; }
  }
}