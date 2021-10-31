using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.StuffPriceDiscrepancy
{
  public class StuffPriceDiscrepancyResult
  {

    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public double? PurchaseOrderPrice { get; set; }
    public int? PurchaseOrderCurrencyId { get; set; }
    public string PurchaseOrderCurrencyTitle { get; set; }
    public double? PurchaseOrderQty { get; set; }
    public int? UnitId { get; set; }
    public string UnitName { get; set; }
    public double? CurrentStuffBasePrice { get; set; }
    public string CurrentStuffBasePriceCurrencyTitle { get; set; }
    public int? CurrentStuffBasePriceCurrencyId { get; set; }
    public string Description { get; set; }
    public DateTime? DateTime { get; set; }
    public DateTime? ConfirmationDateTime { get; set; }
    public int? ConfirmerUserId { get; set; }
    public string ConfirmerUserFullName { get; set; }
    public string ConfirmationDescription { get; set; }
    public StuffPriceDiscrepancyStatus? Status { get; set; }
    public int? ProviderId { get; set; }
    public string ProviderName { get; set; }
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public int StuffCategoryId { get; set; }
    public string StuffCategoryName { get; set; }
    public string PurchaseOrderEmployeeFullName { get; set; }
    public int? SupplierId { get; set; }
    public string SupplierFullName { get; set; }
    public int? PurchaseOrderPriceConfirmerId { get; set; }
    public string PurchaseOrderPriceConfirmerFullName { get; set; }
    public ConfirmationStatus? PurchaseOrderPriceConfirmationStatus { get; set; }
    public string PurchaseOrderPriceConfirmDescription { get; set; }

  }
}
