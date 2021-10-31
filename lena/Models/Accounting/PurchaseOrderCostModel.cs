using lena.Models.Accounting.FinancialDocument;

using lena.Domains.Enums;
namespace lena.Models.Accounting
{
  public class PurchaseOrderCostModel : AddPurchaseOrderCostInput
  {
    public int PurchaseOrderCostId { get; set; }
    public double PurchaseOrderItemWeight { get; set; }
  }
}
