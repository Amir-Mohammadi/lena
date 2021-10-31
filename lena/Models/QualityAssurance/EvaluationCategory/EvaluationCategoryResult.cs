using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EvaluationCategory
{
  public class EvaluationCategoryResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public int? DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
