using lena.Models.QualityAssurance.EvaluationCategoryItem;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EvaluationCategory
{
  public class AddEvaluationCategoryInput
  {
    public string Title { get; set; }
    public short? DepartmentId { get; set; }
    public AddEvaluationCategoryItemInput[] AddEvaluationCategoryItemInput { get; set; }

  }
}
