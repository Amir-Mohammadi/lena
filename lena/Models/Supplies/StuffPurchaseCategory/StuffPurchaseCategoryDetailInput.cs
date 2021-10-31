using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffPurchaseCategory
{
  public class StuffPurchaseCategoryDetailInput
  {
    public int Id { get; set; }
    public int StuffPurchaseCategoryId { get; set; }
    public int ApplicatorUserGroupId { get; set; }
    public int ApplicatorConfirmerUserGroupId { get; set; }
    public int QualityControlUserGroupId { get; set; }
    public int QualityControlDepartmentId { get; set; }
    public int RequestConfirmerUserGroupId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
