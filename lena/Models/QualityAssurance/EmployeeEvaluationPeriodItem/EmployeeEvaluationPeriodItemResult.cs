using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationPeriodItem
{
  public class EmployeeEvaluationPeriodItemResult
  {
    public int EmployeeEvaluationPeriodId { get; set; }
    public int EvaluationCategoryId { get; set; }
    public string EmployeeEvaluationPeriodTitle { get; set; }
    public string EvaluationCategoryTitle { get; set; }
    public int Coefficient { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
