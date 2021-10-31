using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluation
{
  public class GetEmployeeEvaluationPrepareInput
  {
    public int EmployeeId { get; set; }
    public int EmployeeEvaluationPeriodId { get; set; }
    public int EvaluationCategoryId { get; set; }
  }
}
