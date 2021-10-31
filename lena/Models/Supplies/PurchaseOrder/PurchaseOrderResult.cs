using System;
using lena.Domains.Enums;
using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrder
{
  public class PurchaseOrderResult
  {
    public int Id { get; set; }
    public int CargoItemId { get; set; }
    public StuffType StuffType { get; set; }
    public RiskLevelStatus RiskLevelStatus { get; set; }
    public string LatestRiskTitle { get; set; }
    public DateTime? LatestRiskCreateDateTime { get; set; }
    public int LadingItemId { get; set; }
    public string OrderInvoiceNum { get; set; }
    public int? FinancialTransacionBatchId { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime PurchaseOrderDateTime { get; set; }
    public DateTime PurchaseOrderPreparingDateTime { get; set; }
    public int? ProviderId { get; set; }
    public string ProviderName { get; set; }
    public string ProviderCode { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double? StuffGrossWeight { get; set; }
    public double? Price { get; set; }
    public double? TotalPrice { get; set; }
    public int? CurrencuyId { get; set; }
    public string CurrencyTitle { get; set; }
    public string CurrencySign { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double ConversionRatio { get; set; }
    public DateTime Deadline { get; set; }
    public string Description { get; set; }
    public string Code { get; set; }
    public double RemainedQty { get; set; }
    public PurchaseOrderStatus PurchaseOrderStatus { get; set; }
    public int StuffCategoryId { get; set; }
    public string StuffCategoryName { get; set; }
    public string EmployeeFullName { get; set; }
    public int? SupplierId { get; set; }
    public string SupplierFullName { get; set; }
    public PurchaseOrderType PurchaseOrderType { get; set; }
    public double CargoedQty { get; set; }
    public double ReceiptedQty { get; set; }
    public double? AllocatedAmount { get; set; }
    public double? RemainingAmount { get; set; }
    public double QualityControlPassedQty { get; set; }
    public double QualityControlFailedQty { get; set; }
    public IEnumerable<string> PurchaseRequestDescriptionArray { get; set; }
    public int? PurchaseOrderStepDetailId { get; set; }
    public DateTime? PurchaseOrderStepChangeTime { get; set; }
    public string PurchaseOrderStepChangeUserFullName { get; set; }
    public int? PurchaseOrderStepId { get; set; }
    public string PurchaseOrderStepDetailDescription { get; set; }
    public string PurchaseOrderStepName { get; set; }
    public string PurchaseRequsetDescription
    {
      get
      {
        return PurchaseRequestDescriptionArray != null
            ? string.Join(" ، ", PurchaseRequestDescriptionArray)
            : "";
      }
    }
    public string LatestBaseEntityDocumentDescription { get; set; }
    public DateTime? LatestBaseEntityDocumentDateTime { get; set; }
    public string PlanCode { get; set; }
    public int? PriceConfirmerId { get; set; }
    public string PriceConfirmerFullName { get; set; }
    public ConfirmationStatus? PriceConfirmationStatus { get; set; }
    public string PriceConfirmDescription { get; set; }
    public bool IsArchived { get; set; }
    public string PurchaseOrderGroupCode { get; set; }
    public int? PurchaseOrderGroupId { get; set; }
    public int PurchaseOrderId { get; set; }
    public byte[] RowVersion { get; set; }
    public DateTime? MaxEstimateDateTime { get; set; }
    public DateTime? MinDeadlinePurchaseRequest { get; set; }
    public IEnumerable<CargoItemStatus> CargoItemStatus { get; set; }
    public IEnumerable<LadingItemStatus> LadingItemStatus { get; set; }
    public IEnumerable<BankOrderStatus> BankOrderStatus { get; set; }
    public int DecimalDigitCount { get; set; }
    public int CurrencyDecimalDigitCount { get; set; }
    public FinanceAllocationStatus FinanceAllocationStatus { get; set; }
  }
}
