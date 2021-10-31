using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationItemDetail
{
  public class EmployeeEvaluationItemDetailResult
  {
    public int EmployeeEvaluationId { get; set; }
    public string EvaluationCategoryTitle { get; set; }
    public int EvaluationCategoryId { get; set; }
    public int EvaluationCategoryItemId { get; set; }
    public string EvaluationQuestion { get; set; }
    public string EmployeeEvaluationItemDescription { get; set; }
    public DateTime EmployeeEvaluationItemDateTime { get; set; }
    public string EmployeeEvaluationItemEmployeeFullName { get; set; }
    public int Coefficient { get; set; }
    public Score EvaluationScore { get; set; }
  }
}
