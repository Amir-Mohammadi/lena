using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationPeriod
{
  public class EmployeeEvaluationPeriodResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string EmployeeFullName { get; set; }
    public EmployeeEvaluationPeriodStatus Status { get; set; }
    public bool? IsActive { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
