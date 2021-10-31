using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffPrice
{
  public class StuffPriceResult
  {
    public long Id { get; set; }

    public StuffPriceType PriceType { get; set; }

    public int StuffId { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public DateTime DateTime { get; set; }
    public StuffPriceStatus Status { get; set; }
    public double Price { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
    public double? PurchaseOrderQty { get; set; }
    public int? PurchaseOrderProviderId { get; set; }
    public string PurchaseOrderProviderName { get; set; }
    public string ConfirmStuffPriceEmployeeFullName { get; set; }
    public DateTime? ConfirmStuffPriceDate { get; set; }
    public DateTime? LastPurchaseOrderDate { get; set; }
    public double? LastPurchaseOrderPrice { get; set; }
    public string LastPurchaseOrderCurrencyTitle { get; set; }
    public double? QualityControlFailedQty { get; set; }
    public double? QualityControlPassedQty { get; set; }
    public StuffPriceQualityControlStatus StuffPriceQualityControlStatus { get; set; }
    public double? ReceiptedQty { get; set; }
    public int? PurchaseOrderId { get; set; }

    public StuffBasePriceCustomsType? PriceCustomsType { get; set; } // نحوه محاسبه گمرک
    public double? PriceCustomsTariff { get; set; } // تعرفه گمرک
    public double? PriceCustomsPercent { get; set; } // درصد گمرک
    public double? PriceCustomsHowToBuyRatio { get; set; } // ضریب حمل گمرک
    public double? PriceCustomsPrice { get; set; } // فی گمرک
    public double? PriceCustomsWeight { get; set; } // وزن گمرکی
    public double? PriceCustomsCurrenyId { get; set; } // ارز گمرک
    public string PriceCustomsCurrenyTitle { get; set; } // عنوان ارز گمرک

    public StuffBasePriceTransportType? PriceTransportsType { get; set; } // نحوه محاسبه حمل
    public StuffBasePriceTransportComputeType? PriceTransportsComputeType { get; set; }  // حمل بر اساس
    public double? PriceTransportsComputePercent { get; set; } // درصد حمل
    public double? PriceTransportsComputePrice { get; set; } // قیمت حمل
    public string Description { get; set; }
    public string PurchaseOrderCode { get; set; }
  }
}