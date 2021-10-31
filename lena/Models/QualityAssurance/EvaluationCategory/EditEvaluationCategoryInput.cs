using lena.Models.QualityAssurance.EvaluationCategoryItem;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EvaluationCategory
{
  public class EditEvaluationCategoryInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public short? DepartmentId { get; set; }
    public AddEvaluationCategoryItemInput[] AddEvaluationCategoryItemInput { get; set; }
    public EditEvaluationCategoryItemInput[] EditEvaluationCategoryItemInput { get; set; }
    public int[] EvaluationCategoryItemDeteltedIds { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
