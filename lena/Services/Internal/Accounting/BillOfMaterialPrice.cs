using System;
using System.Collections.Generic;
using System.Linq;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Accounting.BillOfMaterialPrice;
using lena.Models.ApplicationBase.CurrencyRate;
using lena.Models.Supplies.StuffPrice;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    public BillOfMaterialPriceTotalResult ToBillOfMaterialPriceResult(
        int stuffId,
        short? version,
        int currencyId,
        CurrencyRateValue[] currencyRateValues,
        bool calculatePurchasePriceByOwnCurrencyRate = false,
        BillOfMaterialPriceListDisplayType displayType = BillOfMaterialPriceListDisplayType.Hierarchical)
    {
      var result = new BillOfMaterialPriceTotalResult();
      switch (displayType)
      {
        case BillOfMaterialPriceListDisplayType.Hierarchical:
          {
            result.BillOfMaterialPrice = ToBillOfMaterialPriceHierarchicalResult(
                      stuffId: stuffId,
                      version: version,
                      currencyId: currencyId,
                      currencyRateValues: currencyRateValues,
                      calculatePurchasePriceByOwnCurrencyRate: calculatePurchasePriceByOwnCurrencyRate,
                      factor: 1);
          }
          break;
        case BillOfMaterialPriceListDisplayType.NonHierarchical:
          {
            result.BillOfMaterialPrice = ToBillOfMaterialPriceNonHierarchicalResult(
                      stuffId: stuffId,
                      version: version,
                      currencyId: currencyId,
                      currencyRateValues: currencyRateValues,
                      calculatePurchasePriceByOwnCurrencyRate: calculatePurchasePriceByOwnCurrencyRate,
                      factor: 1);
          }
          break;
        default:
          {
            throw new ArgumentOutOfRangeException();
          }
      }
      result.Summary = CalculateSummary(result.BillOfMaterialPrice, currencyId);
      return result;
    }
    protected BillOfMaterialPriceSummary CalculateSummary(BillOfMaterialPriceResult billOfMaterialPrice, int currencyId)
    {
      var summary = new BillOfMaterialPriceSummary();
      CalculateSummary(
          billOfMaterialPrices: billOfMaterialPrice.InnersPrices,
          summary: summary,
          factor: 1,
          currencyId: currencyId);
      return summary;
    }
    protected void CalculateSummary(
        List<BillOfMaterialPriceResult> billOfMaterialPrices,
        BillOfMaterialPriceSummary summary,
        double factor,
        int currencyId)
    {
      foreach (var billOfMaterialPriceResult in billOfMaterialPrices)
      {
        if (billOfMaterialPriceResult.InnersPrices.Any())
        {
          CalculateSummary(
              billOfMaterialPrices: billOfMaterialPriceResult.InnersPrices,
              summary: summary,
              factor: factor * billOfMaterialPriceResult.Amount,
              currencyId: currencyId);
        }
        else
        {
          summary.CustomsPrice += (billOfMaterialPriceResult.BaseCustomsFee ?? 0) * factor *
                                  billOfMaterialPriceResult.Amount;
          summary.TransportPrice += (billOfMaterialPriceResult.BaseTransportFee ?? 0) * factor *
                                  billOfMaterialPriceResult.Amount;
          if (billOfMaterialPriceResult.BaseCurrencyId != null)
          {
            double convertedBasePrice = (billOfMaterialPriceResult.BaseFee ?? 0) * factor * billOfMaterialPriceResult.Amount;
            var currencyPrice = summary.CurrencyPriceList.FirstOrDefault(r => r.BaseCurrencyId == billOfMaterialPriceResult.BaseCurrencyId);
            if (currencyPrice == null)
            {
              currencyPrice = new CurrencyPrice();
              currencyPrice.BaseCurrencyId = billOfMaterialPriceResult.BaseCurrencyId ?? 0;
              currencyPrice.BaseCurrencyTitle = billOfMaterialPriceResult.BaseCurrencyTitle;
              currencyPrice.BaseCurrencySign = billOfMaterialPriceResult.BaseCurrencySign;
              currencyPrice.BaseCurrencyDecimalDigitCount = billOfMaterialPriceResult.BaseCurrencyDecimalDigitCount;
              summary.CurrencyPriceList.Add(currencyPrice);
            }
            currencyPrice.ConvertedPrice += convertedBasePrice;
            currencyPrice.BasePrice += (billOfMaterialPriceResult.BaseFeeByOwnCurrency ?? 0) * billOfMaterialPriceResult.Amount;
          }
        }
      }
    }
    protected BillOfMaterialPriceResult ToBillOfMaterialPriceHierarchicalResult(
        int stuffId,
        short? version,
        int currencyId,
        CurrencyRateValue[] currencyRateValues,
        bool calculatePurchasePriceByOwnCurrencyRate,
        double factor = 1)
    {
      #region Get Stuff
      var stuff = App.Internals.SaleManagement.GetStuff(id: stuffId);
      #endregion
      if (stuff.StuffType != StuffType.General && stuff.StuffType != StuffType.RawMaterial)
      {
        #region Bill Of Material
        BillOfMaterial billOfMaterial;
        if (version.HasValue)
          billOfMaterial = App.Internals.Planning.GetBillOfMaterial(
                        stuffId: stuffId,
                        version: version.Value);
        else
          billOfMaterial = App.Internals.Planning.GetPublishedBillOfMaterial(stuffId: stuffId, ignoreNotFoundException: true);
        #endregion
        #region Inner Prices
        var innerPricesQuery = App.Internals.Planning.GetBillOfMaterialDetails(
                billOfMaterialStuffId: billOfMaterial.StuffId,
                billOfMaterialVersion: billOfMaterial.Version);
        var queryList = innerPricesQuery.ToList();
        var innerPrices = queryList
                  .Select(i => ToBillOfMaterialPriceHierarchicalResult(
                          stuffId: i.StuffId,
                          version: i.SemiProductBillOfMaterialVersion,
                          currencyId: currencyId,
                          currencyRateValues: currencyRateValues,
                          calculatePurchasePriceByOwnCurrencyRate: calculatePurchasePriceByOwnCurrencyRate,
                          factor: (i.Value / i.ForQty) * i.Unit.ConversionRatio)
                      )
                  .ToList();
        var mainUnit = App.Internals.ApplicationBase.GetUnits(
                      selector: App.Internals.ApplicationBase.ToUnitResult,
                      unitTypeId: billOfMaterial.Unit.UnitTypeId,
                      isMainUnit: true)
                  .FirstOrDefault();
        #endregion
        #region Result
        var result = new BillOfMaterialPriceResult
        {
          InnersPrices = innerPrices,
          Version = billOfMaterial.Version,
          StuffId = billOfMaterial.StuffId,
          StuffCategoryId = billOfMaterial.Stuff.StuffCategoryId,
          StuffCategoryName = billOfMaterial.Stuff.StuffCategory.Name,
          StuffCode = billOfMaterial.Stuff.Code,
          StuffType = billOfMaterial.Stuff.StuffType,
          Amount = factor,
          Factor = factor,
          StuffName = billOfMaterial.Stuff.Name,
          StuffNoun = billOfMaterial.Stuff.Noun,
          UnitId = mainUnit?.Id,
          UnitName = mainUnit?.Name,
          BaseCurrencyTitle = "",
          BaseCurrencySign = "",
          BaseFee = innerPrices.Sum(u => (u.BaseFee ?? 0) * u.Amount),
          StuffPriceStatus = StuffPriceStatus.None,
          BaseTotalFee = innerPrices.Sum(u => (u.BaseTotalFee ?? 0) * u.Amount),
          BaseCustomsFee = innerPrices.Sum(u => (u.BaseCustomsFee ?? 0) * u.Amount),
          BaseTransportFee = innerPrices.Sum(u => (u.BaseTransportFee ?? 0) * u.Amount),
          BaseCurrencyId = null,
          BasePrice = innerPrices.Sum(u => u.BasePrice ?? 0) * factor,
          NotHasAnyActiveAndPublishedVersion = billOfMaterial.NotHasAnyActiveAndPublishedVersion
        };
        return result;
        #endregion
      }
      else
      {
        #region Get Unit
        var unit = App.Internals.ApplicationBase.GetUnits(
                selector: e => e,
                unitTypeId: stuff.UnitTypeId,
                isMainUnit: true)
            .FirstOrDefault();
        #endregion
        #region Calculate Price
        var baseFee = App.Internals.Supplies.GetStuffBasePrices(
                selector: e => e,
                stuffId: stuffId,
                isCurrent: true)
            .ToList()
            .Select(u => new
            {
              Price = App.Internals.Supplies.CalculatePrice(u, currencyId, currencyRateValues)
                    ,
              OwnPrice = u.Price,
              DateTime = u.DateTime,
              Currency = u.Currency,
              RowVersion = u.RowVersion,
              Status = u.Status,
              Id = u.Id
            })
            .LastOrDefault();
        var numberOfAverageCount = App.Internals.ApplicationSetting.GetNumberOfPricesForAveraging();
        var lastPurchaseFees = App.Internals.Accounting.GetPurchasePrices(
                      selector: e => e,
                      stuffId: stuffId,
                      isDelete: false)
                  .OrderByDescending(u => u.DateTime)
                  .Take(numberOfAverageCount)
                  .ToList()
                  .Select(
                      u => new
                      {
                        Price = calculatePurchasePriceByOwnCurrencyRate
                              ? new StuffCalculationPriceResult
                              {
                                Price = u.Price * u.CurrencyRate
                              }
                              : App.Internals.Supplies.CalculatePrice(u, currencyId, currencyRateValues)
                                  ,
                        SourceCurrencyPrice = new StuffCalculationPriceResult
                        {
                          Price = u.Price,
                        },
                        SourceCurrencyId = u.CurrencyId,
                        SourceCurrencyTitle = u.Currency.Title,
                        SourceCurrencyDecimalDigitCount = u.Currency.DecimalDigitCount,
                        DateTime = u.DateTime
                      }
                  );
        var lastPurchaseFee = lastPurchaseFees.OrderByDescending(u => u.DateTime).LastOrDefault();
        var lastEstimatedFees = App.Internals.Supplies.GetEstimatedPurchasePrices(
                      selector: e => e,
                      stuffId: stuffId)
                  .OrderByDescending(u => u.DateTime)
                  .Take(numberOfAverageCount)
                  .ToList()
                  .Select(
                      u => new
                      {
                        Price = App.Internals.Supplies.CalculatePrice(u, currencyId, currencyRateValues)
                                  ,
                        SourceCurrencyPrice = new StuffCalculationPriceResult
                        {
                          Price = u.Price,
                        },
                        SourceCurrencyId = u.CurrencyId,
                        SourceCurrencyTitle = u.Currency.Title,
                        SourceCurrencyDecimalDigitCount = u.Currency.DecimalDigitCount,
                        DateTime = u.DateTime
                      }
                  );
        var lastEstimatedFee = lastEstimatedFees.OrderByDescending(u => u.DateTime).LastOrDefault();
        #endregion
        #region Result
        var result = new BillOfMaterialPriceResult
        {
          Version = null,
          StuffId = stuff.Id,
          StuffCategoryId = stuff.StuffCategoryId,
          StuffCategoryName = stuff.StuffCategory.Name,
          StuffCode = stuff.Code,
          Amount = factor,
          Factor = factor,
          StuffName = stuff.Name,
          StuffNoun = stuff.Noun,
          UnitId = unit?.Id,
          UnitName = unit?.Name,
          StuffType = stuff.StuffType,
          BaseCurrencyTitle = baseFee?.Currency.Title,
          BaseCurrencySign = baseFee?.Currency.Sign,
          BaseCurrencyId = baseFee?.Currency.Id,
          BaseFee = baseFee?.Price.Price,
          StuffPriceStatus = baseFee?.Status,
          StuffPriceCurrencyId = baseFee?.Currency?.Id,
          StuffPriceCurrencyTitle = baseFee?.Currency?.Title,
          StuffPriceDateTime = baseFee?.DateTime,
          BaseCustomsFee = baseFee?.Price.Customs,
          BaseTransportFee = baseFee?.Price.Transport,
          BaseTotalFee = baseFee?.Price.Total,
          BaseFeeDateTime = baseFee?.DateTime,
          BasePrice = baseFee?.Price.Total * factor,
          AveragePurchaseFee = lastPurchaseFees.Select(u => u.Price.Total).DefaultIfEmpty(0).Sum(),
          AverageEstimatedFee = lastEstimatedFees.Select(u => u.Price.Total).DefaultIfEmpty(0).Sum(),
          LastPurchaseFee = lastPurchaseFee?.Price.Total,
          LastPurchaseFeeInSourceCurrency = lastPurchaseFee?.SourceCurrencyPrice.Total,
          LastPurchaseFeeSourceCurrencyId = lastPurchaseFee?.SourceCurrencyId,
          LastPurchaseFeeSourceCurrencyTitle = lastPurchaseFee?.SourceCurrencyTitle,
          LastPurchaseFeeSourceCurrencyDecimalDigitCount = lastPurchaseFee?.SourceCurrencyDecimalDigitCount,
          LastPurchaseFeeDateTime = lastPurchaseFee?.DateTime,
          LastEstimatedFee = lastEstimatedFee?.Price.Total,
          LastEstimatedFeeInSourceCurrency = lastEstimatedFee?.SourceCurrencyPrice.Total,
          LastEstimatedFeeSourceCurrencyId = lastEstimatedFee?.SourceCurrencyId,
          LastEstimatedFeeSourceCurrencyTitle = lastEstimatedFee?.SourceCurrencyTitle,
          LastEstimatedFeeSourceCurrencyDecimalDigitCount = lastEstimatedFee?.SourceCurrencyDecimalDigitCount,
          LastEstimateDateTime = lastEstimatedFee?.DateTime,
          InnersPrices = new List<BillOfMaterialPriceResult>(),
          BaseFeeByOwnCurrency = baseFee?.OwnPrice,
          BaseCurrencyDecimalDigitCount = baseFee?.Currency?.DecimalDigitCount ?? 0,
          BasePriceId = baseFee?.Id,
          BasePriceRowVersion = baseFee?.RowVersion,
          BasePriceCode = ""
        };
        return result;
        #endregion
      }
    }
    protected BillOfMaterialPriceResult ToBillOfMaterialPriceNonHierarchicalResult(
        int stuffId,
        short? version,
        int currencyId,
        CurrencyRateValue[] currencyRateValues,
        bool calculatePurchasePriceByOwnCurrencyRate,
        double factor = 1)
    {
      if (version.HasValue)
      {
        #region Bill Of Material
        var billOfMaterial = App.Internals.Planning.GetBillOfMaterial(
                stuffId: stuffId,
                version: version.Value);
        #endregion
        #region Inner Prices
        var innersPricesQuery = App.Internals.Planning.GetBillOfMaterialDetails(
                billOfMaterialStuffId: stuffId,
                billOfMaterialVersion: version);
        var innerPrices = new List<BillOfMaterialPriceResult>();
        innersPricesQuery
                  .ToList()
                  .Select(i => GetBillOfMaterialPriceNonHierarchicalInnerResult(
                      stuffId: i.StuffId,
                      version: i.SemiProductBillOfMaterialVersion,
                      currencyId: currencyId,
                      currencyRateValues: currencyRateValues,
                      calculatePurchasePriceByOwnCurrencyRate: calculatePurchasePriceByOwnCurrencyRate,
                      factor: (i.Value / i.ForQty) * factor * i.Unit.ConversionRatio
                  ))
              .ToList()
              .ForEach(i => innerPrices.AddRange(i));
        var mainUnit = App.Internals.ApplicationBase.GetUnits(
                  selector: App.Internals.ApplicationBase.ToUnitResult,
                  unitTypeId: billOfMaterial.Unit.UnitTypeId,
                  isMainUnit: true)
              .FirstOrDefault();
        #endregion
        #region Result
        var result = new BillOfMaterialPriceResult
        {
          InnersPrices = innerPrices,
          Version = billOfMaterial.Version,
          StuffId = billOfMaterial.StuffId,
          StuffCategoryId = billOfMaterial.Stuff.StuffCategoryId,
          StuffCategoryName = billOfMaterial.Stuff.StuffCategory.Name,
          StuffCode = billOfMaterial.Stuff.Code,
          StuffType = billOfMaterial.Stuff.StuffType,
          Amount = factor,
          Factor = factor,
          StuffName = billOfMaterial.Stuff.Name,
          StuffNoun = billOfMaterial.Stuff.Noun,
          UnitId = mainUnit?.Id,
          UnitName = mainUnit?.Name,
          StuffPriceStatus = StuffPriceStatus.None,
          BaseFee = innerPrices.Sum(u => (u.BaseFee ?? 0) * u.Amount),
          BaseTotalFee = innerPrices.Sum(u => (u.BaseTotalFee ?? 0) * u.Amount),
          BaseCustomsFee = innerPrices.Sum(u => (u.BaseCustomsFee ?? 0) * u.Amount),
          BaseTransportFee = innerPrices.Sum(u => (u.BaseTransportFee ?? 0) * u.Amount),
          BasePrice = innerPrices.Sum(u => u.BasePrice ?? 0) * factor,
          //AveragePurchaseFee = innerPrices.Sum(u => u.AveragePurchaseFee),
          //AverageEstimatedFee = innerPrices.Sum(u => u.AverageEstimatedFee),
          //LastPurchaseFee = innerPrices.Sum(u => u.LastPurchaseFee ?? 0),
          //LastEstimatedFee = innerPrices.Sum(u => u.LastEstimatedFee ?? 0),
          BaseCurrencyTitle = "",
          BaseCurrencySign = "",
          BaseFeeByOwnCurrency = null,
          BaseFeeDateTime = null,
          BasePriceCode = "",
          BasePriceRowVersion = null,
          LastPurchaseFeeDateTime = null,
          LastEstimateDateTime = null
        };
        return result;
        #endregion
      }
      else
      {
        #region Stuff
        var stuff = App.Internals.SaleManagement.GetStuff(id: stuffId);
        var unit = App.Internals.ApplicationBase.GetUnits(
                      selector: e => e,
                      unitTypeId: stuff.UnitTypeId,
                      isMainUnit: true)
                  .FirstOrDefault();
        #endregion
        #region Calculate Price
        var baseFee = App.Internals.Supplies.GetStuffBasePrices(
                selector: e => e,
                stuffId: stuffId,
                isCurrent: true)
            .ToList()
            .Select(u => new
            {
              Price = App.Internals.Supplies.CalculatePrice(
                        stuffPrice: u,
                        toCurrencyId: currencyId,
                        currencyRateValues: currencyRateValues)
                    ,
              OwnPrice = u.Price,
              DateTime = u.DateTime,
              Currency = u.Currency,
              RowVersion = u.RowVersion,
              Status = u.Status,
              Id = u.Id
            })
            .LastOrDefault();
        var numberOfAverageCount = App.Internals.ApplicationSetting.GetNumberOfPricesForAveraging();
        var lastPurchaseFees = App.Internals.Accounting.GetPurchasePrices(
                      selector: e => e,
                      stuffId: stuffId,
                      isDelete: false)
                  .OrderByDescending(u => u.DateTime)
                  .Take(numberOfAverageCount)
                  .ToList()
                  .Select(
                      u => new
                      {
                        Price = calculatePurchasePriceByOwnCurrencyRate
                              ? new StuffCalculationPriceResult
                              {
                                Price = u.Price * u.CurrencyRate
                              }
                              : App.Internals.Supplies.CalculatePrice(
                                      stuffPrice: u,
                                      toCurrencyId: currencyId,
                                      currencyRateValues: currencyRateValues)
                                  ,
                        SourceCurrencyPrice = new StuffCalculationPriceResult
                        {
                          Price = u.Price,
                        },
                        SourceCurrencyId = u.CurrencyId,
                        SourceCurrencyTitle = u.Currency.Title,
                        SourceCurrencyDecimalDigitCount = u.Currency.DecimalDigitCount,
                        DateTime = u.DateTime
                      }
                  );
        var lastPurchaseFee = lastPurchaseFees.OrderByDescending(u => u.DateTime).LastOrDefault();
        var lastEstimatedFees = App.Internals.Supplies.GetEstimatedPurchasePrices(
                      selector: e => e,
                      stuffId: stuffId)
                  .OrderByDescending(u => u.DateTime)
                  .Take(numberOfAverageCount)
                  .ToList()
                  .Select(
                      u => new
                      {
                        Price = App.Internals.Supplies.CalculatePrice(
                                  stuffPrice: u,
                                  toCurrencyId: currencyId,
                                  currencyRateValues: currencyRateValues)
                              ,
                        SourceCurrencyPrice = new StuffCalculationPriceResult
                        {
                          Price = u.Price,
                        },
                        SourceCurrencyId = u.CurrencyId,
                        SourceCurrencyTitle = u.Currency.Title,
                        SourceCurrencyDecimalDigitCount = u.Currency.DecimalDigitCount,
                        DateTime = u.DateTime
                      }
                  );
        var lastEstimatedFee = lastEstimatedFees.OrderByDescending(u => u.DateTime).LastOrDefault();
        #endregion
        #region Result
        var result = new BillOfMaterialPriceResult
        {
          Version = null,
          StuffId = stuff.Id,
          StuffCategoryId = stuff.StuffCategoryId,
          StuffCategoryName = stuff.StuffCategory.Name,
          StuffCode = stuff.Code,
          Amount = factor,
          Factor = factor,
          StuffName = stuff.Name,
          StuffNoun = stuff.Noun,
          UnitId = unit?.Id,
          UnitName = unit?.Name,
          StuffType = stuff.StuffType,
          BaseFee = baseFee?.Price.Price,
          StuffPriceStatus = baseFee?.Status,
          StuffPriceCurrencyId = baseFee?.Currency?.Id,
          StuffPriceCurrencyTitle = baseFee?.Currency?.Title,
          StuffPriceDateTime = baseFee?.DateTime,
          BaseCustomsFee = baseFee?.Price?.Customs,
          BaseTransportFee = baseFee?.Price?.Transport,
          BaseTotalFee = baseFee?.Price?.Total,
          BaseFeeDateTime = baseFee?.DateTime,
          BasePrice = baseFee?.Price.Total * factor,
          AveragePurchaseFee = lastPurchaseFees.Select(u => u.Price?.Total ?? 0).DefaultIfEmpty(0).Sum(),
          AverageEstimatedFee = lastEstimatedFees.Select(u => u.Price?.Total ?? 0).DefaultIfEmpty(0).Sum(),
          LastPurchaseFee = lastPurchaseFee?.Price?.Total,
          LastPurchaseFeeInSourceCurrency = lastPurchaseFee?.SourceCurrencyPrice.Total,
          LastPurchaseFeeSourceCurrencyId = lastPurchaseFee?.SourceCurrencyId,
          LastPurchaseFeeSourceCurrencyTitle = lastPurchaseFee?.SourceCurrencyTitle,
          LastPurchaseFeeSourceCurrencyDecimalDigitCount = lastPurchaseFee?.SourceCurrencyDecimalDigitCount,
          LastPurchaseFeeDateTime = lastPurchaseFee?.DateTime,
          LastEstimatedFee = lastEstimatedFee?.Price.Total,
          LastEstimatedFeeInSourceCurrency = lastEstimatedFee?.SourceCurrencyPrice.Total,
          LastEstimatedFeeSourceCurrencyId = lastEstimatedFee?.SourceCurrencyId,
          LastEstimatedFeeSourceCurrencyTitle = lastEstimatedFee?.SourceCurrencyTitle,
          LastEstimatedFeeSourceCurrencyDecimalDigitCount = lastEstimatedFee?.SourceCurrencyDecimalDigitCount,
          LastEstimateDateTime = lastEstimatedFee?.DateTime,
          InnersPrices = new List<BillOfMaterialPriceResult>(),
          BaseCurrencyTitle = baseFee?.Currency?.Title,
          BaseCurrencySign = baseFee?.Currency?.Sign,
          BaseCurrencyId = baseFee?.Currency?.Id,
          BaseFeeByOwnCurrency = baseFee?.OwnPrice,
          BaseCurrencyDecimalDigitCount = baseFee?.Currency?.DecimalDigitCount ?? 0,
          BasePriceCode = "",
          BasePriceRowVersion = baseFee?.RowVersion,
          BasePriceId = baseFee?.Id
        };
        return result;
        #endregion
      }
    }
    protected List<BillOfMaterialPriceResult> GetBillOfMaterialPriceNonHierarchicalInnerResult(
        int stuffId,
        short? version,
        int currencyId,
        CurrencyRateValue[] currencyRateValues,
        bool calculatePurchasePriceByOwnCurrencyRate,
        double factor = 1)
    {
      #region GetStuff
      var stuff = App.Internals.SaleManagement.GetStuff(id: stuffId);
      #endregion
      if (stuff.StuffType != StuffType.General && stuff.StuffType != StuffType.RawMaterial)
      {
        #region Bill Of Material
        BillOfMaterial billOfMaterial = null;
        if (version.HasValue)
          billOfMaterial = App.Internals.Planning.GetBillOfMaterial(
                        stuffId: stuffId,
                        version: version.Value);
        else
        {
          billOfMaterial = App.Internals.Planning.GetPublishedBillOfMaterial(stuffId: stuffId, ignoreNotFoundException: true);
        }
        #endregion
        #region Inner Prices
        var innersPricesQuery = App.Internals.Planning.GetBillOfMaterialDetails(
                billOfMaterialStuffId: billOfMaterial.StuffId,
                billOfMaterialVersion: billOfMaterial.Version);
        var innerPrices = new List<BillOfMaterialPriceResult>();
        innersPricesQuery
                  .ToList()
                  .Select(i => GetBillOfMaterialPriceNonHierarchicalInnerResult(
                      stuffId: i.StuffId,
                      version: i.SemiProductBillOfMaterialVersion,
                      currencyId: currencyId,
                      currencyRateValues: currencyRateValues,
                      calculatePurchasePriceByOwnCurrencyRate: calculatePurchasePriceByOwnCurrencyRate,
                      factor: (i.Value / i.ForQty) * factor * i.Unit.ConversionRatio
                  ))
              .ToList()
              .ForEach(i => innerPrices.AddRange(i));
        return innerPrices;
        #endregion
      }
      else
      {
        #region Stuff
        var unit = App.Internals.ApplicationBase.GetUnits(
                selector: e => e,
                unitTypeId: stuff.UnitTypeId,
                isMainUnit: true)
            .FirstOrDefault();
        #endregion
        #region Calculate Price
        var x = App.Internals.Supplies.GetStuffBasePrices(
                selector: e => e,
                stuffId: stuffId,
                isCurrent: true)
            .ToList();
        var baseFee = x
                  .Select(u =>
                      new
                      {
                        Price = App.Internals.Supplies.CalculatePrice(
                                  stuffPrice: u,
                                  toCurrencyId: currencyId,
                                  currencyRateValues: currencyRateValues)
                              ,
                        OwnPrice = u.Price,
                        DateTime = u.DateTime,
                        Currency = u.Currency,
                        RowVersion = u.RowVersion,
                        Status = u.Status,
                        Id = u.Id
                      })
                  .LastOrDefault();
        var numberOfAverageCount = App.Internals.ApplicationSetting.GetNumberOfPricesForAveraging();
        var lastPurchaseFees = App.Internals.Accounting.GetPurchasePrices(
                      selector: e => e,
                      stuffId: stuffId,
                      isDelete: false)
                  .OrderByDescending(u => u.DateTime)
                  .Take(numberOfAverageCount)
                  .ToList()
                  .Select(
                      u => new
                      {
                        Price = calculatePurchasePriceByOwnCurrencyRate
                              ? new StuffCalculationPriceResult
                              {
                                Price = u.Price * u.CurrencyRate
                              }
                              : App.Internals.Supplies.CalculatePrice(
                                      stuffPrice: u,
                                      toCurrencyId: currencyId,
                                      currencyRateValues: currencyRateValues)
                                  ,
                        SourceCurrencyPrice = new StuffCalculationPriceResult
                        {
                          Price = u.Price,
                        },
                        SourceCurrencyId = u.CurrencyId,
                        SourceCurrencyTitle = u.Currency.Title,
                        SourceCurrencyDecimalDigitCount = u.Currency.DecimalDigitCount,
                        DateTime = u.DateTime
                      }
                  );
        var lastPurchaseFee = lastPurchaseFees.OrderByDescending(u => u.DateTime).LastOrDefault();
        var lastEstimatedFees = App.Internals.Supplies.GetEstimatedPurchasePrices(
                     selector: e => e,
                     stuffId: stuffId)
                 .OrderByDescending(u => u.DateTime)
                 .Take(numberOfAverageCount)
                 .ToList()
                 .Select(
                     u => new
                     {
                       Price = App.Internals.Supplies.CalculatePrice(u, currencyId, currencyRateValues)
                                 ,
                       SourceCurrencyPrice = new StuffCalculationPriceResult
                       {
                         Price = u.Price,
                       },
                       SourceCurrencyId = u.CurrencyId,
                       SourceCurrencyTitle = u.Currency.Title,
                       SourceCurrencyDecimalDigitCount = u.Currency.DecimalDigitCount,
                       DateTime = u.DateTime
                     }
                 );
        var lastEstimatedFee = lastEstimatedFees.OrderByDescending(u => u.DateTime).LastOrDefault();
        #endregion
        #region Result
        var result = new BillOfMaterialPriceResult
        {
          Version = null,
          StuffId = stuff.Id,
          StuffCategoryId = stuff.StuffCategoryId,
          StuffCategoryName = stuff.StuffCategory.Name,
          StuffCode = stuff.Code,
          Amount = factor,
          Factor = factor,
          StuffName = stuff.Name,
          StuffNoun = stuff.Noun,
          UnitId = unit?.Id,
          UnitName = unit?.Name,
          StuffType = stuff.StuffType,
          BaseFee = baseFee?.Price.Price,
          StuffPriceStatus = baseFee?.Status,
          StuffPriceCurrencyId = baseFee?.Currency.Id,
          StuffPriceCurrencyTitle = baseFee?.Currency.Title,
          StuffPriceDateTime = baseFee?.DateTime,
          BaseCustomsFee = baseFee?.Price?.Customs,
          BaseTransportFee = baseFee?.Price?.Transport,
          BaseTotalFee = baseFee?.Price?.Total,
          BaseFeeDateTime = baseFee?.DateTime,
          BasePrice = baseFee?.Price?.Total * factor,
          BaseCurrencyTitle = baseFee?.Currency?.Title,
          BaseCurrencySign = baseFee?.Currency?.Sign,
          BaseCurrencyId = baseFee?.Currency.Id,
          AveragePurchaseFee = lastPurchaseFees.Select(u => u.Price?.Total ?? 0).DefaultIfEmpty(0).Sum(),
          AverageEstimatedFee = lastEstimatedFees.Select(u => u.Price?.Total ?? 0).DefaultIfEmpty(0).Sum(),
          LastPurchaseFee = lastPurchaseFee?.Price?.Total,
          LastPurchaseFeeInSourceCurrency = lastPurchaseFee?.SourceCurrencyPrice.Total,
          LastPurchaseFeeSourceCurrencyId = lastPurchaseFee?.SourceCurrencyId,
          LastPurchaseFeeSourceCurrencyTitle = lastPurchaseFee?.SourceCurrencyTitle,
          LastPurchaseFeeSourceCurrencyDecimalDigitCount = lastPurchaseFee?.SourceCurrencyDecimalDigitCount,
          LastPurchaseFeeDateTime = lastPurchaseFee?.DateTime,
          LastEstimatedFee = lastEstimatedFee?.Price.Total,
          LastEstimatedFeeInSourceCurrency = lastEstimatedFee?.SourceCurrencyPrice.Total,
          LastEstimatedFeeSourceCurrencyId = lastEstimatedFee?.SourceCurrencyId,
          LastEstimatedFeeSourceCurrencyTitle = lastEstimatedFee?.SourceCurrencyTitle,
          LastEstimatedFeeSourceCurrencyDecimalDigitCount = lastEstimatedFee?.SourceCurrencyDecimalDigitCount,
          LastEstimateDateTime = lastEstimatedFee?.DateTime,
          InnersPrices = new List<BillOfMaterialPriceResult>(),
          BaseFeeByOwnCurrency = baseFee?.OwnPrice,
          BaseCurrencyDecimalDigitCount = baseFee?.Currency?.DecimalDigitCount ?? 0,
          BasePriceCode = "",
          BasePriceRowVersion = baseFee?.RowVersion,
          BasePriceId = baseFee?.Id
        };
        return new List<BillOfMaterialPriceResult> { result };
        #endregion
      }
    }
  }
}