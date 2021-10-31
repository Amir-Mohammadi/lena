using lena.Domains.Enums;
namespace lena.Models.StuffCategory
{
  public class StuffCategoryComboResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentStuffCategoryId { get; set; }
    public string ParentStuffCategoryName { get; set; }
  }
}
