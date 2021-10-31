using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluation
{
  public class EmployeeEvaluationGroupItem
  {
    public int EmployeeEvaluationId { get; set; }
    public int EvaluationCategoryId { get; set; }
    public string EvaluationCategoryTitle { get; set; }
    public double TotalScore { get; set; }
    public int Coefficient { get; set; }
    public double TotalPercentage { get; set; }
    public EmployeeEvaluationStatus Status { get; set; }
    public DateTime? PermanentDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
