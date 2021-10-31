using lena.Domains.Enums;
using lena.Models.Accounting.FinancialDocument;
using lena.Models.ApplicationBase.BaseEntityDocument;
using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrder
{
  public class AddPurchaseOrdersInput
  {
    public AddPurchaseOrderInput[] AddPurchaseOrders { get; set; }
    public AddFinancialDocumentInput FinancialDocumentCost { get; set; }
    public AddFinancialDocumentInput FinancialDocumentDiscount { get; set; }
    public AddBaseEntityDocumentInput Document { get; set; }
    public PurchaseOrderType PurchaseOrderType { get; set; }
  }
}