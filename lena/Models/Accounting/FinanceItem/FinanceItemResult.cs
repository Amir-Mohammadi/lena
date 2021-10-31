using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceItem
{
  public class FinanceItemResult
  {
    public int Id { get; set; }
    public int? PurchaseOrderId { get; set; }
    public int? FinanceId { get; set; }

    public int? ExpenseFinancialDocumentId { get; set; }
    public string FinanceCode { get; set; }
    public int? CurrencyId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public string Description { get; set; }
    public double RequestedAmount { get; set; }
    public DateTime RequestedDateTime { get; set; }
    public DateTime? RequestedDueDateTime { get; set; }
    public double? AllocatedAmount { get; set; }
    public ProviderType? ProviderType { get; set; }

    public short? LeadTime { get; set; }
    public DateTime? DeadLineDateTime { get; set; }
    public DateTime? AcceptedDueDateTime { get; set; }
    public FinanceItemConfirmationStatus FinanceItemConfirmationStatus { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime? ReceivedDateTime { get; set; }
    public string ReceivedEmployeeName { get; set; }
    public PaymentMethod? AcceptedPaymentMethod { get; set; }
    public PaymentKind? PaymentKind { get; set; }
    public FinanceType FinanceType { get; set; }
    public string CurrencyTitle { get; set; }
    public int CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public string CooperatorCode { get; set; }
    public string FinancialDescription { get; set; }
    public string ChequeNumber { get; set; }
    public double TotalPrice { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
