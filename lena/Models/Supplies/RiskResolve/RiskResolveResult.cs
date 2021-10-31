using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.RiskResolve
{
  public class RiskResolveResult
  {
    public int Id { get; set; }
    public int RiskId { get; set; }
    public string CorrectiveAction { get; set; }
    public int CreatorUserId { get; set; }
    public string CreatorEmployeeName { get; set; }
    public RiskLevelStatus? RiskLevelStatus { get; set; }
    public DateTime DateTime { get; set; }
    public RiskResolveStatus Status { get; set; }
    public int? ReviewerUserId { get; set; }
    public string ReviewerEmployeeName { get; set; }
    public DateTime? RevieweDateTime { get; set; }
    public string ReviewDescription { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
