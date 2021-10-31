using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EvaluationCategoryItem
{
  public class EditEvaluationCategoryItemInput
  {
    public int Id { get; set; }
    public string Question { get; set; }
    public bool IsArchive { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
