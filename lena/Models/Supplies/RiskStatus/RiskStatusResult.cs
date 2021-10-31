using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.RiskStatus
{
  public class RiskStatusResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string EmployeeName { get; set; }
    public DateTime DateTime { get; set; }
    public RiskLevelStatus RiskLevel { get; set; }
  }
}
