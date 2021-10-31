using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.PayRequest
{
  public class PayRequestResult
  {
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public int? UserId { get; set; }
    public int? ProviderId { get; set; }
    public string ProviderName { get; set; }
    public string EmployeeFullName { get; set; }
    public int QualityControlId { get; set; }
    public string QualityControlCode { get; set; }
    public string Description { get; set; }
    public int? CargoId { get; set; }
    public string CargoCode { get; set; }
    public string CargoItemCode { get; set; }
    public int? CargoItemId { get; set; }
    public string LadingCode { get; set; }
    public int? LadingId { get; set; }
    public int? PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public double Qty { get; set; }
    public int? UnitId { get; set; }
    public string UnitName { get; set; }
    public double? UnitPrice { get; set; }
    public double? TotalPrice { get; set; }
    public double PayedAmount { get; set; }
    public double? DiscountedTotalPrice { get; set; }
    public byte? CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public PayRequestStatus Status { get; set; }
    public ReceiptStatus? ReceiptStatus { get; set; }
    public int? ReceiptId { get; set; }
    public int? FinancialTransactionBatchId { get; set; }
    public int? StoreReceiptId { get; set; }
    public string StoreReceiptCode { get; set; }
    public string FinancialAccountCode { get; set; }
    public Guid? DocumentId { get; set; }
    public Guid? QualityControlDocumentId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
