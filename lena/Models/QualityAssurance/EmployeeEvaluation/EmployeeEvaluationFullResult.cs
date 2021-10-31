using lena.Domains.Enums;
using System;
using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluation
{
  public class EmployeeEvaluationFullResult
  {
    public int? Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public string DepartmentName { get; set; }
    public string EmployeeFullName { get; set; }
    public int? OrganizationPostId { get; set; }
    public string OrganizationPostTitle { get; set; }
    public int? EmployeeEvaluationPeriodId { get; set; }
    public string EmployeeEvaluationPeriodTitle { get; set; }
    public EmployeeEvaluationStatus? Status { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public DateTime? FinalDateTime { get; set; }
    public IEnumerable<EmployeeEvaluationGroupItem> EmployeeEvaluationGroupItems { get; set; }
    public EmployeeEvaluationPeriodStatus? EmployeeEvaluationPeriodStatus { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
