using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationPeriodItem
{
  public class EditEmployeeEvaluationPeriodItemInput
  {
    public int EmployeeEvaluationPeriodId { get; set; }
    public int EvaluationCategoryId { get; set; }
    public int Coefficient { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
