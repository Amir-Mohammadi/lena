using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Risk
{
  public class RiskResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public RiskLevelStatus RiskLevelStatus { get; set; }
    public OccurrenceProbabilityStatus OccurrenceProbabilityStatus { get; set; }
    public OccurrenceSeverityStatus OccurrenceSeverityStatus { get; set; }
    public int RiskFactor { get; set; }
    public int RiskCreatorUserId { get; set; }
    public string RiskCreatorEmployeeName { get; set; }
    public DateTime RiskCreatedDateTime { get; set; }
    public string CorrectiveAction { get; set; }
    public int? RiskResolveCreatorUserId { get; set; }
    public DateTime? RiskResolveDateTime { get; set; }
    public int? RiskResolveId { get; set; }
    public RiskResolveStatus? RiskResolveStatus { get; set; }
    public int? ReviewerUserId { get; set; }
    public string ReviewerEmployeeName { get; set; }
    public DateTime? RevieweDateTime { get; set; }
    public string RiskResolveCreatorEmployeeName { get; set; }
    public string ReviewDescription { get; set; }
    public byte[] RiskRowVersion { get; set; }
    public byte[] RiskResolveRowVersion { get; set; }
  }
}
