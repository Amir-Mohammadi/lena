using lena.Models.QualityAssurance.EmployeeEvaluationItem;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluation
{
  public class AddEmployeeEvaluationEmployeeInput
  {
    public int EmployeeId { get; set; }
    public int EmployeeEvaluationPeriodId { get; set; }
    public SaveEmployeeEvaluationItemInput SaveEmployeeEvaluationItemInput { get; set; }
  }
}
