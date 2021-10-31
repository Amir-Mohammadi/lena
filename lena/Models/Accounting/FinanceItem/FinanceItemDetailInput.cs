using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceItem
{
  public class FinanceItemDetailInput
  {
    public int? Id { get; set; }
    public int? PurchaseOrderId { get; set; }
    public int? ExpenseFinancialDocumentId { get; set; }
    public string Description { get; set; }
    public int CooperatorId { get; set; }
    public double RequestedAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentKind? PaymentKind { get; set; }
    public DateTime RequestedDueDateTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
