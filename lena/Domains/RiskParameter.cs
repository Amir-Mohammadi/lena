using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class RiskParameter : IEntity
  {
    protected internal RiskParameter()
    {
      this.RiskStatuses = new HashSet<RiskStatus>();
    }
    public OccurrenceSeverityStatus OccurrenceSeverityStatus { get; set; }
    public OccurrenceProbabilityStatus OccurrenceProbabilityStatus { get; set; }
    public RiskLevelStatus RiskLevelStatus { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<RiskStatus> RiskStatuses { get; set; }
  }
}