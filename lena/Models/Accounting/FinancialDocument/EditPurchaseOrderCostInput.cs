using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class EditPurchaseOrderCostInput
  {
    public double Amount { get; set; }
    public int? PurchaseOrderGroupId { get; set; }
    public int PurchaseOrderItemId { get; set; }
  }
}
