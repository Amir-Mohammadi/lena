using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffPurchaseCategory
{
  public class EditStuffPurchaseCategoryInput
  {
    public EditStuffPurchaseCategoryInput()
    {
      NewDetails = new StuffPurchaseCategoryDetailInput[] { };
      DeletedDetails = new StuffPurchaseCategoryDetailInput[] { };
      EditedDetails = new StuffPurchaseCategoryDetailInput[] { };
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int StuffDefinitionUserGroupId { get; set; }
    public int StuffDefinitionConfirmerUserGroupId { get; set; }
    public byte[] RowVersion { get; set; }

    public StuffPurchaseCategoryDetailInput[] NewDetails { get; set; }
    public StuffPurchaseCategoryDetailInput[] DeletedDetails { get; set; }
    public StuffPurchaseCategoryDetailInput[] EditedDetails { get; set; }
    public int QualityControlUserGroupId { get; set; }
    public short QualityControlDepartmentId { get; set; }
  }
}
