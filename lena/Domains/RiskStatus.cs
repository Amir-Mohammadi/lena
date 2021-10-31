using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class RiskStatus : IEntity
  {
    protected internal RiskStatus()
    {
    }
    public int Id { get; set; }
    public int RiskId { get; set; }
    public OccurrenceSeverityStatus OccurrenceSeverityStatus { get; set; }
    public OccurrenceProbabilityStatus OccurrenceProbabilityStatus { get; set; }
    public int UserId { get; set; }
    public DateTime DateTime { get; set; }
    public int? RiskResolveId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual Risk Risk { get; set; }
    public virtual RiskParameter RiskParameter { get; set; }
    public virtual RiskResolve RiskResolve { get; set; }
  }
}