using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{
  public class IndicatorRejectedPurchaseResult
  {
    public int Id { get; set; }
    public int StoreReceiptId { get; set; }
    public string StoreReceiptCode { get; set; }
    public DateTime? ReceiptDateTime { get; set; } // تاریخ رسید
    public DateTime InboundCargoDateTime { get; set; } // تاریخ ورود به شرکت
    public DateTime StoreReceiptDateTime { get; set; }  // تاریخ ورود به انبار
    public int? CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public double StoreReceiptAmount { get; set; }
    public int StuffId { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public string StuffNoun { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double? QualityControlPassedQty { get; set; }
    public double? QualityControlConsumedQty { get; set; }
    public double? QualityControlFailedQty { get; set; }
    public StoreReceiptType StoreReceiptType { get; set; }
    public int? CargoItemId { get; set; }
    public string CargoItemCode { get; set; }
    public int? PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public double PurchaseOrderPrice { get; set; }
    public int? CurrencyId { get; set; }
    public string CurrencySign { get; set; }
    public string CurrencyTitle { get; set; }
    public double TotalPrice { get; set; }
    public double QualityControlPassedPrice { get; set; }
    public double QualityControlRejectedPrice { get; set; }
    public string StuffPurchaseCategoryName { get; set; }
    public int? StuffPurchaseCategoryId { get; set; }

  }
}
