using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.BillOfMaterialPriceHistory
{
  public class AddBillOfMaterialPriceHistoryDetailInput
  {
    public int Id { get; set; }
    public int BillOfMaerialPriceHistoryId { get; set; }
    public int StuffId { get; set; }
    public long? BasePriceId { get; set; }
    public int? Version { get; set; }
    public byte UnitId { get; set; }
    public double Amount { get; set; }
    public double? BaseFeeByOwnCurrency { get; set; }
    //public string BasePriceCode { get; set; }
    public double? BaseFee { get; set; }
    public StuffPriceStatus? StuffPriceStatus { get; set; }
    public DateTime? StuffPriceDateTime { get; set; }
    public int? StuffPriceCurrencyId { get; set; }
    public double? BaseCustomsFee { get; set; }
    public double? BaseTransportFee { get; set; }
    public double? BaseTotalFee { get; set; }
    public DateTime? BaseFeeDateTime { get; set; }
    public int? BaseCurrencyId { get; set; }
    public double? BasePrice { get; set; }
    public double Factor { get; set; }
    public double AveragePurchaseFee { get; set; }
    public double? LastPurchaseFee { get; set; }
    public double? LastPurchaseFeeInSourceCurrency { get; set; }
    public int? LastPurchaseFeeSourceCurrencyId { get; set; }
    public DateTime? LastPurchaseFeeDateTime { get; set; }
    public double AverageEstimatedFee { get; set; }
    public double? LastEstimatedFee { get; set; }
    public double? LastEstimatedFeeInSourceCurrency { get; set; }
    public int? LastEstimatedFeeSourceCurrencyId { get; set; }
    public DateTime? LastEstimateDateTime { get; set; }
  }
}
