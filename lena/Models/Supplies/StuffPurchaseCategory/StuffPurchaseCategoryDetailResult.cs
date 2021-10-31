using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffPurchaseCategory
{
  public class StuffPurchaseCategoryDetailResult
  {
    public int Id { get; set; }
    public int StuffPurchaseCategoryId { get; set; }
    public string StuffPurchaseCategoryName { get; set; }
    public int ApplicatorUserGroupId { get; set; }
    public string ApplicatorUserGroupName { get; set; }
    public int ApplicatorConfirmerUserGroupId { get; set; }
    public string ApplicatorConfirmerUserGroupName { get; set; }
    public int? RequestConfirmerUserGroupId { get; set; }
    public string RequestConfirmerUserGroupName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
