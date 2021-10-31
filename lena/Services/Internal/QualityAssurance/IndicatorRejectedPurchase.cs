using System;
using System.Linq;
using lena.Services.Core;
using lena.Services.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models;
using lena.Models.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {
    #region Get IndicatorRejectedPurchaseFinancialPercentage  
    // محاسبه مالی خرید های مردودی
    public IndicatorRejectedPurchaseFinancialPercentageResult GetIndicatorRejectedPurchaseFinancialPercentage(
    TValue<DateTime> fromDate = null,
    TValue<DateTime> toDate = null)
    {

      #region Gets IndicatorRejectedPurchases

      var indicatorRejectedPurchasesResult = App.Internals.QualityAssurance.GetIndicatorRejectedPurchases(
          fromDate: fromDate,
          toDate: toDate);
      #endregion

      var currencies = App.Internals.ApplicationBase.GetCurrencys();
      var currencyIdIRR = currencies.Where(m => m.IsMain).FirstOrDefault().Id;
      var dateTimeNow = DateTime.Now.ToUniversalTime();

      var result = new IndicatorRejectedPurchaseFinancialPercentageResult();
      var groupResult = from indicator in indicatorRejectedPurchasesResult
                        group indicator by indicator.CurrencyId into g
                        select new
                        {
                          CurrencyId = g.Key.Value,
                          SumQualityControlRejectedPrice = ((double?)g.Where(m => m.QualityControlFailedQty > 0).Sum(v => v.QualityControlRejectedPrice)) ?? 0,
                          SumQualityControlPassedPrice = ((double?)g.Where(m => m.QualityControlPassedQty > 0).Sum(v => v.QualityControlPassedPrice)) ?? 0
                        };

      #region Covert To IRR

      foreach (var item in groupResult)
      {
        if (item.CurrencyId == currencyIdIRR)
        {
          result.SumQualityControlPassedPriceIRR = item.SumQualityControlPassedPrice;
          result.SumQualityControlRejectedPriceIRR = item.SumQualityControlRejectedPrice;
        }
        else
        {
          var currencyRatesOnDate = App.Internals.ApplicationBase.GetCurrencyRateOnDate(
                    dateTime: dateTimeNow,
                    fromCurrencyId: item.CurrencyId,
                    toCurrencyId: currencyIdIRR);

          result.SumQualityControlPassedPriceIRR += item.SumQualityControlPassedPrice * currencyRatesOnDate;
          result.SumQualityControlRejectedPriceIRR += item.SumQualityControlRejectedPrice * currencyRatesOnDate;
        }
      }
      #endregion

      #region Calculate Indicator Rejected Purchase Financial Percentage
      result.TotalQualityControlPriceIRR = result.SumQualityControlPassedPriceIRR + result.SumQualityControlRejectedPriceIRR;

      if (result.TotalQualityControlPriceIRR == 0)
        throw new System.Exception($"Division by zero error in calculating of Indicator Rejected Purchases Financial Percentage");

      var qualityControlFaildPricePercent = (result.SumQualityControlRejectedPriceIRR / result.TotalQualityControlPriceIRR) * 100;

      var indicatorRejectedPurchaseResult = new IndicatorRejectedPurchaseFinancialPercentageResult
      {
        Amount = qualityControlFaildPricePercent
      };
      #endregion
      return indicatorRejectedPurchaseResult;
    }
    #endregion

    #region Get IndicatorRejectedPurchaseQtyPercentage  
    // محاسبه تعداد خرید های مردودی
    public IndicatorRejectedPurchaseQtyPercentageResult GetIndicatorRejectedPurchaseQtyPercentage(
    TValue<DateTime> fromDate = null,
    TValue<DateTime> toDate = null)
    {

      #region Gets IndicatorRejectedPurchases

      var indicatorRejectedPurchasesResult = App.Internals.QualityAssurance.GetIndicatorRejectedPurchases(
          fromDate: fromDate,
          toDate: toDate);
      #endregion

      #region Calculate Indicator Rejected Purchase Qty Percentage
      var result = new IndicatorRejectedPurchaseQtyPercentageResult();
      var groupResult = from indicator in indicatorRejectedPurchasesResult
                        group indicator by indicator.CurrencyId into g
                        select new
                        {
                          CurrencyId = g.Key.Value,
                          SumQualityControlRejectedQty = ((double?)g.Count(v => v.QualityControlFailedQty > 0)) ?? 0,
                          SumQualityControlPassedQty = ((double?)g.Count(v => v.QualityControlPassedQty > 0)) ?? 0
                        };

      foreach (var item in groupResult)
      {
        result.SumQualityControlPassedQty += item.SumQualityControlPassedQty;
        result.SumQualityControlRejectedQty += item.SumQualityControlRejectedQty;

      }
      result.TotalQualityControlQty = result.SumQualityControlPassedQty + result.SumQualityControlRejectedQty;
      if (result.TotalQualityControlQty == 0)
        throw new System.Exception($"Division by zero error in calculating of Indicator Rejected Purchases Qty Percentage");

      var qualityControlFaildPricePercent = (result.SumQualityControlRejectedQty / result.TotalQualityControlQty) * 100;

      var indicatorRejectedPurchaseResult = new IndicatorRejectedPurchaseQtyPercentageResult
      {
        Amount = qualityControlFaildPricePercent
      };
      #endregion
      return indicatorRejectedPurchaseResult;
    }
    #endregion

    #region Gets IndicatorRejectedPurchases
    public IQueryable<IndicatorRejectedPurchaseResult> GetIndicatorRejectedPurchases(
    TValue<double> conversionRatio = null,
    TValue<DateTime> fromDate = null,
    TValue<DateTime> toDate = null)
    {

      #region Gets Store Receipt Result
      var storeReceipts = App.Internals.WarehouseManagement.GetStoreReceipts(e => e,
          fromDate: fromDate,
          toDate: toDate,
          isDelete: false);
      var storeReceiptResult = App.Internals.WarehouseManagement.ToStoreReceiptResult(storeReceipts);
      #endregion
      var purchaseOrders = App.Internals.Supplies.GetPurchaseOrders(e => e);

      var query = from storeReceipt in storeReceiptResult
                  join purchaseOrder in purchaseOrders on storeReceipt.PurchaseOrderId equals purchaseOrder.Id
                  select new IndicatorRejectedPurchaseResult()
                  {
                    StoreReceiptId = storeReceipt.Id,
                    StoreReceiptCode = storeReceipt.Code,
                    ReceiptDateTime = storeReceipt.ReceiptDateTime, // تاریخ رسید
                    InboundCargoDateTime = storeReceipt.InboundCargoDateTime, // تاریخ ورود به شرکت
                    StoreReceiptDateTime = storeReceipt.DateTime, // تاریخ ورود به انبار
                    CooperatorId = storeReceipt.CooperatorId,
                    CooperatorName = storeReceipt.CooperatorName,
                    StoreReceiptAmount = storeReceipt.Amount,
                    StuffId = storeReceipt.StuffId,
                    StuffName = storeReceipt.StuffName,
                    StuffCode = storeReceipt.StuffCode,
                    StuffNoun = storeReceipt.StuffNoun,
                    UnitId = storeReceipt.UnitId,
                    UnitName = storeReceipt.UnitName,
                    QualityControlPassedQty = storeReceipt.QualityControlPassedQty,
                    QualityControlConsumedQty = storeReceipt.QualityControlConsumedQty,
                    QualityControlFailedQty = storeReceipt.QualityControlFailedQty,
                    StoreReceiptType = storeReceipt.StoreReceiptType,
                    CargoItemId = storeReceipt.CargoItemId,
                    CargoItemCode = storeReceipt.CargoItemCode,
                    PurchaseOrderId = storeReceipt.PurchaseOrderId,
                    PurchaseOrderCode = storeReceipt.PurchaseOrderCode,
                    WarehouseId = storeReceipt.WarehouseId,
                    WarehouseName = storeReceipt.WarehouseName,
                    CurrencyId = purchaseOrder.CurrencyId,
                    CurrencySign = purchaseOrder.Currency.Sign,
                    CurrencyTitle = purchaseOrder.Currency.Title,
                    PurchaseOrderPrice = purchaseOrder.Price ?? 0,
                    QualityControlPassedPrice = storeReceipt.QualityControlPassedQty * purchaseOrder.Price ?? 0,
                    QualityControlRejectedPrice = storeReceipt.QualityControlFailedQty * purchaseOrder.Price ?? 0,
                    TotalPrice = (storeReceipt.QualityControlPassedQty * purchaseOrder.Price ?? 0) + (storeReceipt.QualityControlFailedQty * purchaseOrder.Price ?? 0),
                    StuffPurchaseCategoryName = purchaseOrder.Stuff.StuffPurchaseCategory.Title,
                    StuffPurchaseCategoryId = purchaseOrder.Stuff.StuffPurchaseCategoryId

                  };

      return query;
    }
    #endregion

    #region Search
    public IQueryable<IndicatorRejectedPurchaseResult> SearchIndicatorRejectedPurchase(
       IQueryable<IndicatorRejectedPurchaseResult> query,
       string searchText
     )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.StoreReceiptCode.Contains(searchText)
                select item;
      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<IndicatorRejectedPurchaseResult> SortIndicatorRejectedPurchaseResult(IQueryable<IndicatorRejectedPurchaseResult> query,
        SortInput<IndicatorRejectedPurchaseSortType> sort)
    {
      switch (sort.SortType)
      {
        case IndicatorRejectedPurchaseSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }

}
