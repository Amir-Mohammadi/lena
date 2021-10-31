using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase;
using lena.Services.Internals.Planning.Exception;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.StuffPrice;
using lena.Models.ApplicationBase.CurrencyRate;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public StuffPrice AddStuffPrice(
        StuffPrice stuffPrice,
        byte currencyId,
        double price,
        int stuffId,
        StuffPriceType type,
        string description)
    {
      stuffPrice = stuffPrice ?? repository.Create<StuffPrice>();
      stuffPrice.CurrencyId = currencyId;
      stuffPrice.Status = StuffPriceStatus.Current;
      stuffPrice.Price = price;
      stuffPrice.StuffId = stuffId;
      stuffPrice.Type = type;
      stuffPrice.Description = description;
      App.Internals.ApplicationBase.AddBaseEntity(
                baseEntity: stuffPrice,
                transactionBatch: null,
                description: description);
      return stuffPrice;
    }
    #endregion
    #region Edit
    public StuffPrice EditStuffPrice(
        int id,
        byte[] rowVersion,
        TValue<byte> currencyId = null,
        TValue<double> price = null,
        TValue<int> stuffId = null,
        TValue<int?> confirmUserId = null,
        TValue<DateTime> confirmDate = null,
        TValue<StuffPriceStatus> status = null)
    {
      var stuffPrice = GetStuffPrice(id: id);
      return EditStuffPrice(
                stuffPrice: stuffPrice,
                rowVersion: rowVersion,
                currencyId: currencyId,
                price: price,
                stuffId: stuffId,
                confirmUserId: confirmUserId,
                confirmDate: confirmDate,
                status: status);
    }
    public StuffPrice EditStuffPrice(
        StuffPrice stuffPrice,
        byte[] rowVersion,
        TValue<byte> currencyId = null,
        TValue<double> price = null,
        TValue<int> stuffId = null,
        TValue<int?> confirmUserId = null,
        TValue<DateTime> confirmDate = null,
        TValue<StuffPriceStatus> status = null)
    {
      if (currencyId != null)
        stuffPrice.CurrencyId = currencyId;
      if (price != null)
        stuffPrice.Price = price;
      if (stuffId != null)
        stuffPrice.StuffId = stuffId;
      if (confirmUserId != null)
        stuffPrice.ConfirmUserId = confirmUserId;
      if (confirmDate != null)
        stuffPrice.ConfirmDate = confirmDate;
      if (status != null)
        stuffPrice.Status = status;
      App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: stuffPrice,
                    rowVersion: rowVersion);
      return stuffPrice;
    }
    #endregion
    #region CalculatePrice
    public StuffCalculationPriceResult CalculatePrice(StuffPrice stuffPrice, int toCurrencyId, CurrencyRateValue[] currencyRateValues = null)
    {
      #region Calculate Price
      switch (stuffPrice.Type)
      {
        case StuffPriceType.BasePrice:
          {
            var stuffBasePrice = (StuffBasePrice)stuffPrice;
            switch (stuffBasePrice.StuffBasePriceType)
            {
              case StuffBasePriceType.Constant:
                {
                  var currencyRate = App.Internals.ApplicationBase.GetCurrencyRate(
                                   rates: currencyRateValues,
                                   fromCurrencyId: stuffBasePrice.CurrencyId,
                                   toCurrencyId: toCurrencyId);
                  return new StuffCalculationPriceResult()
                  {
                    Price = stuffBasePrice.Price * currencyRate
                  };
                }
              case StuffBasePriceType.Computional:
                {
                  var price = new StuffCalculationPriceResult();
                  var currencyRate = App.Internals.ApplicationBase.GetCurrencyRate(
                                   rates: currencyRateValues,
                                   fromCurrencyId: stuffBasePrice.CurrencyId,
                                   toCurrencyId: toCurrencyId);
                  price.Price = stuffBasePrice.Price * currencyRate;
                  var customs = stuffBasePrice.StuffBasePriceCustoms;
                  #region Calculate Customs Price
                  var customCurrencyRate = App.Internals.ApplicationBase.GetCurrencyRate(
                            rates: currencyRateValues,
                            fromCurrencyId: customs.CurrencyId,
                            toCurrencyId: toCurrencyId);
                  switch (customs.Type)
                  {
                    case StuffBasePriceCustomsType.Computional:
                      #region Calculate Computional Customs Price
                      var A = customs.Price;
                      var B = customs.Tariff ?? 0;
                      var C = customs.HowToBuyRatio ?? 0 * customs.Weight ?? 0 * A;
                      var E = 0.005 * A;
                      var F = A + C + E;
                      var G = B * F;
                      var H = 0.01 * G;
                      var I = 0.03 * (G + F);
                      var J = 0.06 * (G + F);
                      var T = I + J + H + G;
                      price.Customs = T * customCurrencyRate;
                      #endregion
                      break;
                    case StuffBasePriceCustomsType.Percentage:
                      #region Calculate Percentage Customs Price
                      price.Customs = customs.Price * ((customs.Percent ?? 0) / 100) * customCurrencyRate; ;
                      #endregion
                      break;
                    default:
                      throw new ArgumentOutOfRangeException();
                  }
                  #endregion
                  #region Calculate Transport Price
                  var transport = stuffBasePrice.StuffBasePriceTransport;
                  switch (transport.Type)
                  {
                    case StuffBasePriceTransportType.Computional:
                      price.Transport = transport.Price / (transport.ComputeType == StuffBasePriceTransportComputeType.Volumetric ?
                                                  stuffBasePrice.Stuff.Volume :
                                                  stuffBasePrice.Stuff.GrossWeight) ?? 0 * currencyRate;
                      break;
                    case StuffBasePriceTransportType.Percentage:
                      price.Transport = stuffBasePrice.Price * ((transport.Percent ?? 0) / 100) * currencyRate;
                      break;
                    default:
                      throw new ArgumentOutOfRangeException();
                  }
                  return price;
                  #endregion
                }
              default:
                throw new ArgumentOutOfRangeException();
            }
          }
        case StuffPriceType.EstimatedPurchasePrice:
          {
            var estimatedPurchasePrice = (EstimatedPurchasePrice)stuffPrice;
            var currencyRate = App.Internals.ApplicationBase.GetCurrencyRate(
                                       rates: currencyRateValues,
                                       fromCurrencyId: estimatedPurchasePrice.CurrencyId,
                                       toCurrencyId: toCurrencyId);
            return new StuffCalculationPriceResult
            {
              Price = estimatedPurchasePrice.Price * currencyRate
            };
          }
        case StuffPriceType.PurchasePrice:
          {
            var purchasePrice = (PurchasePrice)stuffPrice;
            var currencyRate = App.Internals.ApplicationBase.GetCurrencyRate(
                                   rates: currencyRateValues,
                                   fromCurrencyId: purchasePrice.CurrencyId,
                                   toCurrencyId: toCurrencyId);
            return new StuffCalculationPriceResult
            {
              Price = purchasePrice.Price * currencyRate
            };
          }
        default:
          throw new ArgumentOutOfRangeException();
      }
      #endregion
    }
    #endregion
    #region GenerateCode
    //public string GenerateStuffPriceCode(
    //   StuffPrice stuffPrice)
    //{
    //    (repository) =>
    //    {
    //        var code = "";
    //        #region Part1
    //        switch (stuffPrice.Type)
    //        {
    //            case StuffPriceType.BasePrice:
    //                code += "B";
    //                break;
    //            case StuffPriceType.EstimatedPurchasePrice:
    //                code += "E";
    //                break;
    //            case StuffPriceType.PurchasePrice:
    //                code += "P";
    //                break;
    //        }
    //        #endregion
    //        #region Part2
    //        switch ((stuffPrice as StuffBasePrice)?.StuffBasePriceType)
    //        {
    //            case StuffBasePriceType.Constant:
    //                code += "F";
    //                break;
    //            case StuffBasePriceType.Computional:
    //                code += "C";
    //                break;
    //        }
    //        #endregion
    //        #region Part3
    //        switch ((stuffPrice as ComputionalStuffBasePrice)?.StuffBasePriceCustoms.Type)
    //        {
    //            case StuffBasePriceCustomsType.Percentage:
    //                code += "P";
    //                break;
    //            case StuffBasePriceCustomsType.Computional:
    //                code += "C";
    //                break;
    //        }
    //        #endregion
    //        #region Part4
    //        switch ((stuffPrice as ComputionalStuffBasePrice)?.StuffBasePriceTransports.Type)
    //        {
    //            case StuffBasePriceTransportType.Percentage:
    //                code += "P";
    //                break;
    //            case StuffBasePriceTransportType.Computional:
    //                code += "C";
    //                break;
    //        }
    //        #endregion
    //        return code;
    //    });
    //}
    #endregion
    #region ArchiveProcess
    public StuffPrice ArchiveStuffPriceProcess(
        int id,
        byte[] rowVersion)
    {
      var stuffPrice = GetStuffPrice(id: id);
      return ArchiveStuffPriceProcess(
                    stuffPrice: stuffPrice,
                    rowVersion: rowVersion);
    }
    public StuffPrice ArchiveStuffPriceProcess(
        StuffPrice stuffPrice,
        byte[] rowVersion)
    {
      var status = stuffPrice.Status;
      status = status | StuffPriceStatus.Archived;
      status = status & (~StuffPriceStatus.Current);
      EditStuffPrice(
                    stuffPrice: stuffPrice,
                    rowVersion: rowVersion,
                    status: status);
      return stuffPrice;
    }
    #endregion
    #region DeleteProcess
    public StuffPrice DeleteStuffPriceProcess(
        int id,
        byte[] rowVersion)
    {
      var stuffPrice = GetStuffPrice(id: id);
      return DeleteStuffPriceProcess(
                    stuffPrice: stuffPrice,
                    rowVersion: rowVersion);
    }
    public StuffPrice DeleteStuffPriceProcess(
        StuffPrice stuffPrice,
        byte[] rowVersion)
    {
      var status = stuffPrice.Status;
      status = status | StuffPriceStatus.Deleted;
      status = status & (~StuffPriceStatus.Current);
      EditStuffPrice(
                    stuffPrice: stuffPrice,
                    rowVersion: rowVersion,
                    status: status);
      return stuffPrice;
    }
    #endregion
    #region ConfirmProcess
    public StuffPrice ConfirmStuffPriceProcess(
        int id,
        byte[] rowVersion,
        string description)
    {
      var stuffPrice = GetStuffPrice(id: id);
      return ConfirmStuffPriceProcess(
                    stuffPrice: stuffPrice,
                    rowVersion: rowVersion,
                    description: description);
    }
    public StuffPrice ConfirmStuffPriceProcess(
        StuffPrice stuffPrice,
        byte[] rowVersion,
        string description)
    {
      #region AcceptBaseEntityConfirmation
      var acceptBaseEntityConfirmation = App.Internals.Confirmation.AcceptBaseEntityConfirmation(
                       baseEntityConfirmTypeId: Models.StaticData.StaticBaseEntityConfirmTypes.BasePriceConfirmation.Id,
                       confirmingEntityId: stuffPrice.Id,
                       confirmDescription: description);
      #endregion
      #region Set Status
      var status = stuffPrice.Status;
      if (!status.HasFlag(StuffPriceStatus.Current))
        throw new StuffPriceNotInCurrentStatusException();
      if (status.HasFlag(StuffPriceStatus.Check))
        throw new StuffPriceIsCheckedException();
      status = StuffPriceStatus.Current | StuffPriceStatus.Check | StuffPriceStatus.Accept;
      EditStuffPrice(
                    stuffPrice: stuffPrice,
                    rowVersion: rowVersion,
                    status: status,
                    confirmUserId: acceptBaseEntityConfirmation.ConfirmerId,
                    confirmDate: acceptBaseEntityConfirmation.ConfirmDateTime);
      #endregion
      return stuffPrice;
    }
    #endregion
    #region RejectProcess
    public StuffPrice RejectStuffPriceProcess(
        int id,
        byte[] rowVersion,
        string description)
    {
      var stuffPrice = GetStuffPrice(id: id);
      return RejectStuffPriceProcess(
                    stuffPrice: stuffPrice,
                    rowVersion: rowVersion,
                    description: description);
    }
    public StuffPrice RejectStuffPriceProcess(
        StuffPrice stuffPrice,
        byte[] rowVersion,
        string description)
    {
      #region Set Status
      var status = stuffPrice.Status;
      if (!status.HasFlag(StuffPriceStatus.Current))
        throw new StuffPriceNotInCurrentStatusException();
      if (status.HasFlag(StuffPriceStatus.Check))
        throw new StuffPriceIsCheckedException();
      status = StuffPriceStatus.Current | StuffPriceStatus.Check | StuffPriceStatus.Reject;
      EditStuffPrice(
                    stuffPrice: stuffPrice,
                    rowVersion: rowVersion,
                    status: status);
      #endregion
      #region RejectBaseEntityConfirmation
      App.Internals.Confirmation.RejectBaseEntityConfirmation(
                      baseEntityConfirmTypeId: Models.StaticData.StaticBaseEntityConfirmTypes.BasePriceConfirmation.Id,
                      confirmingEntityId: stuffPrice.Id,
                      confirmDescription: description);
      #endregion
      return stuffPrice;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStuffPrices<TResult>(
        Expression<Func<StuffPrice, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<int> stuffId = null,
        TValue<StuffPriceStatus> status = null,
        TValue<int> currencyId = null,
        TValue<int> userId = null,
        TValue<StuffPriceType> priceType = null,
        TValue<StuffPriceStatus[]> statuses = null,
        TValue<StuffPriceStatus[]> notHasStatuses = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<bool> isCurrent = null,
        TValue<bool> isDelete = null,
        TValue<string> description = null)
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    userId: userId,
                    isDelete: isDelete);
      var query = baseQuery.OfType<StuffPrice>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (currencyId != null)
        query = query.Where(i => i.CurrencyId == currencyId);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (priceType != null)
        query = query.Where(i => i.Type == priceType);
      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);
      if (status != null)
        query = query.Where(i => i.Status.HasFlag(status));
      if (statuses != null)
      {
        var s = StuffPriceStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        query = query.Where(i => (i.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = StuffPriceStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        query = query.Where(i => (i.Status & s) == 0);
      }
      if (isCurrent != null)
      {
        query = query.Where(i => i.Status.HasFlag(StuffPriceStatus.Current) == isCurrent);
        query = query.Where(i => !i.Status.HasFlag(StuffPriceStatus.Archived)
                                       && !i.Status.HasFlag(StuffPriceStatus.Deleted));
      }
      if (description != null)
        query = query.Where(i => i.Description == description);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public StuffPrice GetStuffPrice(int id) => GetStuffPrice(selector: e => e, id: id);
    public TResult GetStuffPrice<TResult>(Expression<Func<StuffPrice, TResult>> selector, int id)
    {
      var stuffPrice = GetStuffPrices(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (stuffPrice == null)
        throw new StuffPriceNotFoundException(id);
      return stuffPrice;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffPriceResult> SortStuffPriceResult(
        IQueryable<StuffPriceResult> input,
        SortInput<StuffPriceSortType> options)
    {
      switch (options.SortType)
      {
        case StuffPriceSortType.CurrencyTitle:
          return input.OrderBy(i => i.CurrencyTitle, options.SortOrder);
        case StuffPriceSortType.DateTime:
          return input.OrderBy(i => i.DateTime, options.SortOrder);
        case StuffPriceSortType.EmployeeFullName:
          return input.OrderBy(i => i.EmployeeFullName, options.SortOrder);
        case StuffPriceSortType.Status:
          return input.OrderBy(i => i.Status, options.SortOrder);
        case StuffPriceSortType.Price:
          return input.OrderBy(i => i.Price, options.SortOrder);
        case StuffPriceSortType.StuffCode:
          return input.OrderBy(i => i.StuffCode, options.SortOrder);
        case StuffPriceSortType.StuffName:
          return input.OrderBy(i => i.StuffName, options.SortOrder);
        case StuffPriceSortType.PriceType:
          return input.OrderBy(i => i.PriceType, options.SortOrder);
        case StuffPriceSortType.PurchaseOrderQty:
          return input.OrderBy(i => i.PurchaseOrderQty, options.SortOrder);
        case StuffPriceSortType.ConfirmStuffPriceEmployeeFullName:
          return input.OrderBy(i => i.ConfirmStuffPriceEmployeeFullName, options.SortOrder);
        case StuffPriceSortType.ConfirmStuffPriceDate:
          return input.OrderBy(i => i.ConfirmStuffPriceDate, options.SortOrder);
        case StuffPriceSortType.LastPurchaseOrderDate:
          return input.OrderBy(i => i.LastPurchaseOrderDate, options.SortOrder);
        case StuffPriceSortType.LastPurchaseOrderPrice:
          return input.OrderBy(i => i.LastPurchaseOrderPrice, options.SortOrder);
        case StuffPriceSortType.LastPurchaseOrderCurrencyTitle:
          return input.OrderBy(i => i.LastPurchaseOrderCurrencyTitle, options.SortOrder);
        case StuffPriceSortType.PurchaseOrderProviderName:
          return input.OrderBy(i => i.PurchaseOrderProviderName, options.SortOrder);
        case StuffPriceSortType.PriceCustomsType:
          return input.OrderBy(i => i.PriceCustomsType, options.SortOrder);
        case StuffPriceSortType.PriceCustomsTariff:
          return input.OrderBy(i => i.PriceCustomsTariff, options.SortOrder);
        case StuffPriceSortType.PriceCustomsPercent:
          return input.OrderBy(i => i.PriceCustomsPercent, options.SortOrder);
        case StuffPriceSortType.PriceCustomsHowToBuyRatio:
          return input.OrderBy(i => i.PriceCustomsHowToBuyRatio, options.SortOrder);
        case StuffPriceSortType.PriceCustomsPrice:
          return input.OrderBy(i => i.PriceCustomsPrice, options.SortOrder);
        case StuffPriceSortType.PriceCustomsWeight:
          return input.OrderBy(i => i.PriceCustomsWeight, options.SortOrder);
        case StuffPriceSortType.PriceCustomsCurrenyTitle:
          return input.OrderBy(i => i.PriceCustomsCurrenyTitle, options.SortOrder);
        case StuffPriceSortType.PriceTransportsType:
          return input.OrderBy(i => i.PriceTransportsType, options.SortOrder);
        case StuffPriceSortType.PriceTransportsComputeType:
          return input.OrderBy(i => i.PriceTransportsComputeType, options.SortOrder);
        case StuffPriceSortType.PriceTransportsComputePercent:
          return input.OrderBy(i => i.PriceTransportsComputePercent, options.SortOrder);
        case StuffPriceSortType.PriceTransportsComputePrice:
          return input.OrderBy(i => i.PriceTransportsComputePrice, options.SortOrder);
        case StuffPriceSortType.PurchaseOrderCode:
          return input.OrderBy(i => i.PurchaseOrderCode, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    public Expression<Func<StuffPrice, StuffPriceResult>> ToStuffPriceResult =>
        stuffPrice => new StuffPriceResult
        {
          Id = stuffPrice.Id,
          CurrencyId = stuffPrice.CurrencyId,
          PriceType = stuffPrice.Type,
          CurrencyTitle = stuffPrice.Currency.Title,
          DateTime = stuffPrice.DateTime,
          EmployeeFullName = stuffPrice.User.Employee.FirstName + " " + stuffPrice.User.Employee.LastName,
          EmployeeId = stuffPrice.User.Employee.Id,
          Status = stuffPrice.Status,
          Price = stuffPrice.Price,
          StuffId = stuffPrice.StuffId,
          StuffCode = stuffPrice.Stuff.Code,
          StuffName = stuffPrice.Stuff.Name,
          Description = stuffPrice.Description,
          RowVersion = stuffPrice.RowVersion
        };
    #endregion
    #region ToStuffPriceQuery
    public IQueryable<StuffPriceResult> ToStuffPriceQuery(
        IQueryable<StuffPrice> stuffPrices,
        IQueryable<StuffBasePrice> stuffBasePrices,
        IQueryable<EstimatedPurchasePrice> estimatedPurchasePrices)
    {
      var result = from stuffPrice in stuffPrices
                   join estimatedPurchasePrice in estimatedPurchasePrices on
                   stuffPrice.Id equals estimatedPurchasePrice.Id into estimatedPurchasePriceTemp
                   from estimatedPurchasePrice in estimatedPurchasePriceTemp.DefaultIfEmpty()
                   join stuffBasePrice in stuffBasePrices on
                   stuffPrice.Id equals stuffBasePrice.Id into stuffBasePriceTemp
                   from stuffBasePrice in stuffBasePriceTemp.DefaultIfEmpty()
                   select new StuffPriceResult
                   {
                     Id = stuffPrice.Id,
                     CurrencyId = stuffPrice.CurrencyId,
                     PriceType = stuffPrice.Type,
                     CurrencyTitle = stuffPrice.Currency.Title,
                     DateTime = stuffPrice.DateTime,
                     EmployeeFullName = stuffPrice.User.Employee.FirstName + " " + stuffPrice.User.Employee.LastName,
                     EmployeeId = stuffPrice.User.Employee.Id,
                     Status = stuffPrice.Status,
                     Price = stuffPrice.Price,
                     StuffId = stuffPrice.StuffId,
                     StuffCode = stuffPrice.Stuff.Code,
                     StuffName = stuffPrice.Stuff.Name,
                     RowVersion = stuffPrice.RowVersion,
                     PurchaseOrderQty = estimatedPurchasePrice.PurchaseOrder.Qty,
                     PurchaseOrderId = stuffPrice.Type == StuffPriceType.BasePrice ? stuffBasePrice.PurchaseOrder.Id : estimatedPurchasePrice.PurchaseOrderId,
                     PurchaseOrderCode = stuffPrice.Type == StuffPriceType.BasePrice ? stuffBasePrice.PurchaseOrder.Code : estimatedPurchasePrice.PurchaseOrder.Code,
                     PurchaseOrderProviderId = estimatedPurchasePrice.PurchaseOrder.ProviderId,
                     PurchaseOrderProviderName = estimatedPurchasePrice.PurchaseOrder.Provider.Name,
                     ConfirmStuffPriceEmployeeFullName = null,
                     ConfirmStuffPriceDate = null,
                     LastPurchaseOrderDate = estimatedPurchasePrice.PurchaseOrder.PurchaseOrderDateTime,
                     LastPurchaseOrderPrice = estimatedPurchasePrice.PurchaseOrder.Price,
                     LastPurchaseOrderCurrencyTitle = estimatedPurchasePrice.PurchaseOrder.Currency.Title,
                     QualityControlFailedQty = estimatedPurchasePrice.PurchaseOrder.PurchaseOrderSummary.QualityControlFailedQty,
                     QualityControlPassedQty = estimatedPurchasePrice.PurchaseOrder.PurchaseOrderSummary.QualityControlPassedQty,
                     ReceiptedQty = estimatedPurchasePrice.PurchaseOrder.PurchaseOrderSummary.ReceiptedQty,
                     PriceCustomsType = stuffBasePrice.StuffBasePriceCustoms.Type,
                     PriceCustomsTariff = stuffBasePrice.StuffBasePriceCustoms.Tariff,
                     PriceCustomsPercent = stuffBasePrice.StuffBasePriceCustoms.Percent,
                     PriceCustomsHowToBuyRatio = stuffBasePrice.StuffBasePriceCustoms.HowToBuyRatio,
                     PriceCustomsPrice = stuffBasePrice.StuffBasePriceCustoms.Price,
                     PriceCustomsWeight = stuffBasePrice.StuffBasePriceCustoms.Weight,
                     PriceCustomsCurrenyId = stuffBasePrice.StuffBasePriceCustoms.CurrencyId,
                     PriceCustomsCurrenyTitle = stuffBasePrice.StuffBasePriceCustoms.Currency.Title,
                     PriceTransportsType = stuffBasePrice.StuffBasePriceTransport.Type,
                     PriceTransportsComputeType = stuffBasePrice.StuffBasePriceTransport.ComputeType,
                     PriceTransportsComputePercent = stuffBasePrice.StuffBasePriceTransport.Percent,
                     PriceTransportsComputePrice = stuffBasePrice.StuffBasePriceTransport.Price,
                     Description = stuffBasePrice.Description
                   };
      return result;
    }
    #endregion
    #region Search
    public IQueryable<StuffPriceResult> SearchStuffPriceResultQuery(
        IQueryable<StuffPriceResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from stuffPrice in query
                where stuffPrice.CurrencyTitle.Contains(searchText) ||
                      stuffPrice.EmployeeFullName.Contains(searchText) ||
                      stuffPrice.StuffCode.Contains(searchText) ||
                      stuffPrice.StuffName.Contains(searchText)
                select stuffPrice;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region GetBaseEntityConfirmationStuffBasePrice
    public TResult GetBaseEntityConfirmationStuffBasePrice<TResult>(
        Expression<Func<BaseEntityConfirmation, TResult>> selector,
        int id)
    {
      var record = App.Internals.Confirmation.GetBaseEntityConfirmations(
                    selector: selector,
                    confirmingEntityId: id,
                    baseEntityConfirmTypeId: Models.StaticData.StaticBaseEntityConfirmTypes.BasePriceConfirmation.Id)
                .FirstOrDefault();
      if (record == null)
        throw new RecordNotFoundException(id, typeof(BaseEntityConfirmation));
      return record;
    }
    #endregion
    #region GetBaseEntityConfirmationStuffPrice
    public TResult GetBaseEntityConfirmationStuffPrice<TResult>(
        Expression<Func<BaseEntityConfirmation, TResult>> selector,
        int id)
    {
      var record = App.Internals.Confirmation.GetBaseEntityConfirmations(
                    selector: selector,
                    confirmingEntityId: id,
                    baseEntityConfirmTypeId: Models.StaticData.StaticBaseEntityConfirmTypes.BasePriceConfirmation.Id)
                .FirstOrDefault();
      if (record == null)
        throw new RecordNotFoundException(id, typeof(BaseEntityConfirmation));
      return record;
    }
    #endregion
  }
}