using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationPeriodItem
{
  public class AddEmployeeEvaluationPeriodItemInput
  {
    public int EvaluationCategoryId { get; set; }
    public int EmployeeEvaluationPeriodId { get; set; }
    public int Coefficient { get; set; }
  }
}
