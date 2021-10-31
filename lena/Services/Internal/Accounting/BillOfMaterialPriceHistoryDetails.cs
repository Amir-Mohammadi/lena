using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
//using Microsoft.Ajax.Utilities;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Accounting.BillOfMaterialPriceHistory;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    public BillOfMaterialPriceHistoryDetail GetBillOfMaterialPriceHistoryDetail(int id)
    {

      var entity = GetBillOfMaterialPriceHistoryDetails(selector: e => e,
                id: id).FirstOrDefault();

      if (entity == null)
        throw new RecordNotFoundException(id, typeof(BillOfMaterialPriceHistoryDetail));
      return entity;
    }

    public IQueryable<TResult> GetBillOfMaterialPriceHistoryDetails<TResult>(
        Expression<Func<BillOfMaterialPriceHistoryDetail, TResult>> selector,
        TValue<int> id = null,
       TValue<int> billOfMaterialPriceHistoryId = null,
       TValue<int> userId = null,
       TValue<DateTime> dateTime = null,
       TValue<int> stuffId = null,
       TValue<int> version = null
       )
    {

      var query = repository.GetQuery<BillOfMaterialPriceHistoryDetail>();

      if (id != null)
        query = query.Where(i => i.Id == id);

      if (billOfMaterialPriceHistoryId != null)
        query = query.Where(i => i.BillOfMaerialPriceHistoryId == billOfMaterialPriceHistoryId);

      if (userId != null)
        query = query.Where(i => i.BillOfMaterialPriceHistory.UserId == userId);

      if (dateTime != null)
        query = query.Where(i => i.BillOfMaterialPriceHistory.DateTime == dateTime);

      if (stuffId != null)
        query = query.Where(i => i.BillOfMaterialPriceHistory.StuffId == stuffId);

      if (version != null)
        query = query.Where(i => i.BillOfMaterialPriceHistory.Version == version);

      return query.Select(selector);
    }


    public void AddBillOfMaterialPriceHistoryDetail(
        int billOfMaerialPriceHistoryId,
        int stuffId,
        byte unitId,
        long? basePriceId,
        int? version,
        double amount,
        double? baseFeeByOwnCurrency,
        double? baseFee,
        StuffPriceStatus? stuffPriceStatus,
        DateTime? stuffPriceDateTime,
        int? stuffPriceCurrencyId,
        double? baseCustomsFee,
        double? baseTransportFee,
        double? baseTotalFee,
        DateTime? baseFeeDateTime,
        int? baseCurrencyId,
        double? basePrice,
        double factor,
        double averagePurchaseFee,
        double? lastPurchaseFee,
        double? lastPurchaseFeeInSourceCurrency,
        int? lastPurchaseFeeSourceCurrencyId,
        DateTime? lastPurchaseFeeDateTime,
        double averageEstimatedFee,
        double? lastEstimatedFee,
        double? lastEstimatedFeeInSourceCurrency,
        int? lastEstimatedFeeSourceCurrencyId,
        DateTime? lastEstimateDateTime

        )
    {

      var entity = repository.Create<BillOfMaterialPriceHistoryDetail>();
      entity.BillOfMaerialPriceHistoryId = billOfMaerialPriceHistoryId;
      entity.StuffId = stuffId;
      entity.UnitId = unitId;
      entity.BasePriceId = basePriceId;
      entity.Version = version;
      entity.Amount = amount;
      entity.BaseFeeByOwnCurrency = baseFeeByOwnCurrency;
      entity.BaseFee = baseFee;
      entity.StuffPriceStatus = stuffPriceStatus;
      entity.StuffPriceDateTime = stuffPriceDateTime;
      entity.StuffPriceCurrencyId = stuffPriceCurrencyId;
      entity.BaseCustomsFee = baseCustomsFee;
      entity.BaseTransportFee = baseTransportFee;
      entity.BaseTotalFee = baseTotalFee;
      entity.BaseFeeDateTime = baseFeeDateTime;
      entity.BaseCurrencyId = baseCurrencyId;
      entity.BasePrice = basePrice;
      entity.Factor = factor;
      entity.AveragePurchaseFee = averagePurchaseFee;
      entity.LastPurchaseFee = lastPurchaseFee;
      entity.LastPurchaseFeeInSourceCurrency = lastPurchaseFeeInSourceCurrency;
      entity.LastPurchaseFeeSourceCurrencyId = lastPurchaseFeeSourceCurrencyId;
      entity.LastPurchaseFeeDateTime = lastPurchaseFeeDateTime;
      entity.AverageEstimatedFee = averageEstimatedFee;
      entity.LastEstimatedFee = lastEstimatedFee;
      entity.LastEstimatedFeeInSourceCurrency = lastEstimatedFeeInSourceCurrency;
      entity.LastEstimatedFeeSourceCurrencyId = lastEstimatedFeeSourceCurrencyId;
      entity.LastEstimateDateTime = lastEstimateDateTime;


      repository.Add(entity);
    }

    public void DeleteBillOfMaterialPriceHistoryDetail(BillOfMaterialPriceHistoryDetail detail, int id)
    {

      var entity = detail == null ? GetBillOfMaterialPriceHistoryDetail(id) : detail;

      repository.Delete(entity);
    }

    public IQueryable<BillOfMaterialPriceHistoryDetailResult> ToBillOfMaterialPriceHistoryDetailResult(
        IQueryable<BillOfMaterialPriceHistoryDetail> detailQuery
        )
    {


      var stuffQuery = App.Internals.SaleManagement.GetStuffs(e => new { StuffId = e.Id, StuffCode = e.Code, StuffName = e.Name, StuffNone = e.Noun, e.StuffType });
      var unitQuery = App.Internals.ApplicationBase.GetUnits(e => new { UnitId = e.Id, UnitName = e.Name });
      var currencyQuery = App.Internals.ApplicationBase.GetCurrencys();

      var query = (from detail in detailQuery
                   join stuff in stuffQuery on detail.StuffId equals stuff.StuffId
                   join unit in unitQuery on detail.UnitId equals unit.UnitId
                   join stuffPriceCurrency in currencyQuery on detail.StuffPriceCurrencyId equals stuffPriceCurrency.Id into stuffPriceCurrencyLeft
                   from stuffPriceCurrency in stuffPriceCurrencyLeft.DefaultIfEmpty()
                   join baseCurrency in currencyQuery on detail.BaseCurrencyId equals baseCurrency.Id into baseCurrencyLeft
                   from baseCurrency in baseCurrencyLeft.DefaultIfEmpty()
                   join lastPurchaseFeeSourceCurrency in currencyQuery on detail.LastPurchaseFeeSourceCurrencyId equals lastPurchaseFeeSourceCurrency.Id into lastPurchaseFeeSourceCurrencyLeft
                   from lastPurchaseFeeSourceCurrency in lastPurchaseFeeSourceCurrencyLeft.DefaultIfEmpty()
                   join lastEstimatedFeeSourceCurrency in currencyQuery on detail.LastEstimatedFeeSourceCurrencyId equals lastEstimatedFeeSourceCurrency.Id into lastEstimatedFeeSourceCurrencyLeft
                   from lastEstimatedFeeSourceCurrency in lastEstimatedFeeSourceCurrencyLeft.DefaultIfEmpty()
                   select new BillOfMaterialPriceHistoryDetailResult()
                   {
                     Id = detail.Id,
                     StuffId = stuff.StuffId,
                     StuffCode = stuff.StuffCode,
                     StuffName = stuff.StuffName,
                     StuffNoun = stuff.StuffNone,
                     StuffType = stuff.StuffType,
                     BasePriceId = detail.BasePriceId,
                     Version = detail.Version,
                     Amount = detail.Amount,
                     UnitId = detail.UnitId,
                     UnitName = unit.UnitName,
                     BaseFeeByOwnCurrency = detail.BaseFeeByOwnCurrency,
                     BaseCurrencyDecimalDigitCount = baseCurrency.DecimalDigitCount,
                     BasePriceCode = "",
                     BaseFee = detail.BaseFee,
                     StuffPriceStatus = detail.StuffPriceStatus,
                     StuffPriceDateTime = detail.StuffPriceDateTime,
                     StuffPriceCurrencyId = detail.StuffPriceCurrencyId,
                     StuffPriceCurrencyTitle = stuffPriceCurrency.Title,
                     BaseCustomsFee = detail.BaseCustomsFee,
                     BaseTransportFee = detail.BaseTransportFee,
                     BaseTotalFee = detail.BaseTotalFee,
                     BaseFeeDateTime = detail.BaseFeeDateTime,
                     BaseCurrencyTitle = baseCurrency.Title,
                     BaseCurrencySign = baseCurrency.Sign,
                     BaseCurrencyId = baseCurrency.Id,
                     BasePrice = detail.BasePrice,
                     Factor = detail.Factor,
                     AveragePurchaseFee = detail.AveragePurchaseFee,
                     LastPurchaseFee = detail.LastPurchaseFee,
                     LastPurchaseFeeInSourceCurrency = detail.LastPurchaseFeeInSourceCurrency,
                     LastPurchaseFeeSourceCurrencyId = detail.LastPurchaseFeeSourceCurrencyId,
                     LastPurchaseFeeSourceCurrencyDecimalDigitCount = lastPurchaseFeeSourceCurrency.DecimalDigitCount,
                     LastPurchaseFeeSourceCurrencyTitle = lastPurchaseFeeSourceCurrency.Title,
                     LastPurchaseFeeDateTime = detail.LastPurchaseFeeDateTime,
                     AverageEstimatedFee = detail.AverageEstimatedFee,
                     LastEstimatedFee = detail.LastEstimatedFee,
                     LastEstimatedFeeInSourceCurrency = detail.LastEstimatedFeeInSourceCurrency,
                     LastEstimatedFeeSourceCurrencyId = detail.LastEstimatedFeeSourceCurrencyId,
                     LastEstimatedFeeSourceCurrencyDecimalDigitCount = lastEstimatedFeeSourceCurrency.DecimalDigitCount,
                     LastEstimatedFeeSourceCurrencyTitle = lastEstimatedFeeSourceCurrency.Title,
                     LastEstimateDateTime = detail.LastEstimateDateTime,
                     BillOfMaterialPriceHistoryRowVersion = detail.BillOfMaterialPriceHistory.RowVersion,
                     BillOfMaterialPriceHistoryDetailRowVersion = detail.RowVersion

                   });


      return query;
    }

  }
}
