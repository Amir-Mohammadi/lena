using lena.Models.QualityAssurance.EmployeeEvaluationItem;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluation
{
  public class EditEmployeeEvaluationEmployeeInput
  {
    public int EmployeeEvaluationId { get; set; }
    public SaveEmployeeEvaluationItemInput SaveEmployeeEvaluationItemInput { get; set; }
  }
}
