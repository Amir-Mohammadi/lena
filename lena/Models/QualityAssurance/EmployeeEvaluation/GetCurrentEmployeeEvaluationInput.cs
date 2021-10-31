using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluation
{
  public class GetCurrentEmployeeEvaluationInput
  {
    public int EmployeeId { get; set; }
    public int EmployeeEvaluationPeriodId { get; set; }
  }
}
