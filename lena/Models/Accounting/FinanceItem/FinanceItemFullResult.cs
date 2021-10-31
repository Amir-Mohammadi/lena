using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceItem
{
  public class FinanceItemFullResult
  {
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }
    public string Description { get; set; }
    public double RequestedAmount { get; set; }
    public DateTime RequestedDateTime { get; set; }
    public double? AllocatedAmount { get; set; }
    public DateTime? AcceptedDateTime { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyCode { get; set; }
    public string CurrencyTitle { get; set; }
    public string CurrencySign { get; set; }
    public byte CurrencyDecimalDigitCount { get; set; }
    public bool? Confirmed { get; set; }
    public FinanceItemConfirmationStatus FinanceItemConfirmationStatus { get; set; }
    public DateTime DateTime { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int ProviderId { get; set; }
    public string ProviderName { get; set; }
    public string ProviderCode { get; set; }
    public string PurchaseOrderCode { get; set; }
    public double Price { get; set; }
    public byte[] RowVersion { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentKind PaymentKind { get; set; }
  }
}
