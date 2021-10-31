using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationItem
{
  public class EmployeeEvaluationItemResult
  {
    public int EmployeeEvaluationId { get; set; }
    public int EvaluationCategoryId { get; set; }
    public string Description { get; set; }
    public EmployeeEvaluationStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
