using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffPurchaseCategory
{
  public class StuffPurchaseCategoryResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int StuffDefinitionUserGroupId { get; set; }
    public string StuffDefinitionUserGroupName { get; set; }
    public int StuffDefinitionConfirmerUserGroupId { get; set; }
    public string StuffDefinitionConfirmerUserGroupName { get; set; }
    public int QualityControlUserGroupId { get; set; }
    public string QualityControlUserGroupName { get; set; }
    public int QualityControlDepartmentId { get; set; }
    public string QualityControlDepartmentName { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
