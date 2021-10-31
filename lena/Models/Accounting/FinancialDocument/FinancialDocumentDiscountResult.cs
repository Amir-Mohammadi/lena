using System.Collections.Generic;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class FinancialDocumentDiscountResult
  {
    public int? Id { get; set; }
    public DiscountType? DiscountType { get; set; }

    public IEnumerable<PurchaseOrderDiscountResult> PurchaseOrderDiscounts { get; set; }

    public byte[] RowVersion { get; set; }
  }
}
