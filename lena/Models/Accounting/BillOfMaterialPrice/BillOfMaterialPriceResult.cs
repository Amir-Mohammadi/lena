using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using lena.Domains.Enums;
using lena.Domains.Enums;
namespace lena.Models.Accounting.BillOfMaterialPrice
{
  public class BillOfMaterialPriceResult
  {
    public int StuffId { get; set; }
    public int StuffCategoryId { get; set; }
    public string StuffCategoryName { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public long? BasePriceId { get; set; }
    public StuffType StuffType { get; set; }
    public int? Version { get; set; }
    public int? UnitId { get; set; }
    public string UnitName { get; set; }
    public double Amount { get; set; }
    public double? BaseFeeByOwnCurrency { get; set; }
    public int BaseCurrencyDecimalDigitCount { get; set; }
    public byte[] BasePriceRowVersion { get; set; }
    public string BasePriceCode { get; set; }
    public double? BaseFee { get; set; }
    public StuffPriceStatus? StuffPriceStatus { get; set; }
    public DateTime? StuffPriceDateTime { get; set; }
    public int? StuffPriceCurrencyId { get; set; }
    public string StuffPriceCurrencyTitle { get; set; }
    public double? BaseCustomsFee { get; set; }
    public double? BaseTransportFee { get; set; }
    public double? BaseTotalFee { get; set; }
    public DateTime? BaseFeeDateTime { get; set; }
    public string BaseCurrencyTitle { get; set; }
    public string BaseCurrencySign { get; set; }
    public int? BaseCurrencyId { get; set; }
    public double? BasePrice { get; set; }
    [JsonIgnore]
    public double Factor { get; set; }
    public double AveragePurchaseFee { get; set; } // فی متوسط خرید
    public double AveragePurchasePrice => AveragePurchaseFee * Factor; // قیمت متوسط خرید
    public double? LastPurchaseFee { get; set; } // فی آخرین خرید
    public double? LastPurchaseFeeInSourceCurrency { get; set; } // فی آخرین خرید به ارز اصلی
    public double? LastPurchasePriceInSourceCurrency => LastPurchaseFeeInSourceCurrency * Factor; // قیمت آخرین خرید به ارز اصلی
    public int? LastPurchaseFeeSourceCurrencyId { get; set; }
    public int? LastPurchaseFeeSourceCurrencyDecimalDigitCount { get; set; }
    public string LastPurchaseFeeSourceCurrencyTitle { get; set; }
    public double? LastPurchasePrice => LastPurchaseFee * Factor; // قیمت آخرین خرید
    public DateTime? LastPurchaseFeeDateTime { get; set; } // تاریخ آخرین خرید
    public double AverageEstimatedFee { get; set; } // فی متوسط تخمینی
    public double AverageEstimatedPrice => AverageEstimatedFee * Factor; // قیمت متوسط تخمینی
    public double? LastEstimatedFee { get; set; } // فی آخرین تخمینی
    public double? LastEstimatedFeeInSourceCurrency { get; set; } // فی آخرین تخمینی به ارز اصلی
    public double? LastEstimatedPriceInSourceCurrency => LastEstimatedFeeInSourceCurrency * Factor; // قیمت آخرین تخمینی به ارز اصلی
    public int? LastEstimatedFeeSourceCurrencyId { get; set; }
    public int? LastEstimatedFeeSourceCurrencyDecimalDigitCount { get; set; }
    public string LastEstimatedFeeSourceCurrencyTitle { get; set; }
    public double? LastEstimatedPrice => LastEstimatedFee * Factor; // قیمت آخرین تخمینی
    public DateTime? LastEstimateDateTime { get; set; } // تاریخ آخرین تخمینی
    public List<BillOfMaterialPriceResult> InnersPrices { get; set; }
    public bool NotHasAnyActiveAndPublishedVersion { get; set; }
  }
}