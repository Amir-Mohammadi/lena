using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class AddFinancialDocumentDiscountInput
  {
    public DiscountType Type { get; set; }
    public AddPurchaseOrderDiscountInput[] PurchaseOrderDiscounts { get; set; }
  }
}
