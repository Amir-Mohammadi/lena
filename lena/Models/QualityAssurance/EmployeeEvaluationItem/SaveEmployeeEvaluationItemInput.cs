using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationItem
{
  public class SaveEmployeeEvaluationItemInput
  {
    public int EvaluationCategoryId { get; set; }
    public string Description { get; set; }
    public SaveEmployeeEvaluationItemDetailInput[] SaveEmployeeEvaluationItemDetailInputs { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
