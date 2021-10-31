using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluation
{
  public class EmployeeEvaluationPrepareResult
  {
    public int? EmployeeEvalutionId { get; set; }
    public int EvaluationCateogryItemId { get; set; }
    public string Question { get; set; }
    public string EmployeeEvaluationDescription { get; set; }
    public Score? Score { get; set; }
    public byte[] EmployeeEvaluationRowVersion { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
