using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffPurchaseCategory
{
  public class AddStuffPurchaseCategoryDetailInput
  {
    public int StuffPurchaseCategoryId { get; set; }
    public int ApplicatorUserGroupId { get; set; }
    public int ApplicatorConfirmerUserGroupId { get; set; }
    public int RequestConfirmerUserGroupId { get; set; }
  }
}
