using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationItem
{
  public class SaveEmployeeEvaluationItemDetailInput
  {
    public int EvaluationCategoryItemId { get; set; }
    public Score Score { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
