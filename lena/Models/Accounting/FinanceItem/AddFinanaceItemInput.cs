using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceItem
{
  public class AddPurchaseOrderFinanaceItemInput
  {
    public int purchaseOrderId { get; set; }
    public int CooperatorId { get; set; }
    public FinanceItemDetailInput[] AddFinanaceItemDetailInputs { get; set; }
  }
}
