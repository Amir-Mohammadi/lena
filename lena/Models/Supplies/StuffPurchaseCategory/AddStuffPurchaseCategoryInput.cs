using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffPurchaseCategory
{
  public class AddStuffPurchaseCategoryInput
  {
    public AddStuffPurchaseCategoryInput()
    {

    }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int StuffDefinitionUserGroupId { get; set; }
    public int StuffDefinitionConfirmerUserGroupId { get; set; }

    public AddStuffPurchaseCategoryDetailInput[] Details { get; set; }
    public int QualityControlUserGroupId { get; set; }
    public short QualityControlDepartmentId { get; set; }
  }
}
