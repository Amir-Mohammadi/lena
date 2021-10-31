using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains;
using lena.Models.Supplies.StuffBasePrice;
using lena.Models.Common;
using lena.Services.Core;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region AddProcess
    public StuffBasePrice AddStuffBasePriceProcess(
        StuffBasePrice stuffBasePrice,
        byte currencyId,
        double price,
        int stuffId,
        StuffBasePriceType stuffBasePriceType,
        string description)
    {
      #region Deactivate older prices
      var oldStuffBasePrices = GetStuffBasePrices(
                  selector: e => e,
                  stuffId: stuffId,
                  isCurrent: true);
      foreach (var oldStuffBasePrice in oldStuffBasePrices)
      {
        ArchiveStuffPriceProcess(stuffPrice: oldStuffBasePrice,
                      rowVersion: oldStuffBasePrice.RowVersion);
      }
      #endregion
      #region Add new Price
      return AddStuffBasePrice(
          stuffBasePrice: stuffBasePrice,
          currencyId: currencyId,
              price: price,
              stuffId: stuffId,
              stuffBasePriceType: stuffBasePriceType,
              description: description);
      #endregion
    }
    #endregion
    #region Add
    public StuffBasePrice AddStuffBasePrice(
        StuffBasePrice stuffBasePrice,
        byte currencyId,
        double price,
        int stuffId,
        StuffBasePriceType stuffBasePriceType,
        string description)
    {
      stuffBasePrice = stuffBasePrice ?? repository.Create<StuffBasePrice>();
      stuffBasePrice.StuffBasePriceType = stuffBasePriceType;
      AddStuffPrice(
                stuffPrice: stuffBasePrice,
                currencyId: currencyId,
                price: price,
                stuffId: stuffId,
                type: StuffPriceType.BasePrice,
                description: description);
      return stuffBasePrice;
    }
    #endregion
    #region EditProcess
    public StuffBasePrice EditStuffBasePriceProcess(
        int id,
        byte[] rowVersion,
        byte currencyId,
        double price,
        StuffBasePriceType stuffBasePriceType,
        string description)
    {
      #region Archive
      var stuffPrice = ArchiveStuffPriceProcess(id: id,
              rowVersion: rowVersion);
      #endregion
      #region Add New StuffBasePrice
      return AddStuffBasePriceProcess(
              stuffBasePrice: null,
              currencyId: currencyId,
              price: price,
              stuffId: stuffPrice.StuffId,
              stuffBasePriceType: stuffBasePriceType,
              description: description);
      #endregion
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStuffBasePrices<TResult>(
        Expression<Func<StuffBasePrice, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<StuffBasePriceType> stuffBasePriceType = null,
        TValue<int> stuffId = null,
        TValue<StuffPriceStatus> status = null,
        TValue<StuffPriceStatus[]> statuses = null,
        TValue<StuffPriceStatus[]> notHasStatuses = null,
        TValue<int> currencyId = null,
        TValue<int> userId = null,
        TValue<bool> isCurrent = null,
        TValue<string> description = null)
    {
      var baseQuery = GetStuffPrices(
                    selector: e => e,
                    id: id,
                    code: code,
                    stuffId: stuffId,
                    status: status,
                    statuses: statuses,
                    notHasStatuses: notHasStatuses,
                    currencyId: currencyId,
                    userId: userId,
                    priceType: StuffPriceType.BasePrice,
                    isCurrent: isCurrent,
                    description: description);
      var query = baseQuery.OfType<StuffBasePrice>();
      if (stuffBasePriceType != null)
        query = query.Where(i => i.StuffBasePriceType == stuffBasePriceType);
      return query.Select(selector);
    }
    #endregion
    #region GetResults
    public IQueryable<StuffBasePriceResult> GetStuffBasePriceResults(
        TValue<long> id = null,
        TValue<string> code = null,
        TValue<int> stuffId = null,
        TValue<StuffPriceStatus> status = null,
        TValue<StuffPriceStatus[]> statuses = null,
        TValue<StuffPriceStatus[]> notHasStatuses = null,
        TValue<int> currencyId = null,
        TValue<int> userId = null,
        TValue<string> description = null)
    {
      #region StuffBasePriceQuery
      var stuffBasePriceQuery = GetStuffBasePrices(
              selector: e => e);
      var stuffBasePriceResult = ToStuffBasePriceResult(stuffBasePriceQuery);
      var stuffBasePriceStuffIds = GetStuffBasePrices(
                selector: e => e.StuffId);
      #endregion
      #region stuffWithoutPriceQuery
      var stuffs = App.Internals.SaleManagement.GetStuffs(
          selector: e => new { e.Id, e.Code, e.Name, e.UnitTypeId },
          stuffType: StuffType.RawMaterial);
      #endregion
      var mainUnits = App.Internals.ApplicationBase.GetUnits(
                selector: App.Internals.ApplicationBase.ToUnitResult,
                isMainUnit: true);
      #region ToResult
      var stuffWithoutPrice = from stuff in stuffs
                              join unit in mainUnits on stuff.UnitTypeId equals unit.UnitTypeId
                              where !stuffBasePriceStuffIds.Contains(stuff.Id)
                              select new StuffBasePriceResult
                              {
                                Id = null,
                                CurrencyId = null,
                                CurrencyTitle = "",
                                CurrencyDecimalDigitCount = 0,
                                DateTime = null,
                                UserId = null,
                                EmployeeFullName = null,
                                EmployeeId = null,
                                Status = StuffPriceStatus.None,
                                Price = 0,
                                MainPrice = 0,
                                StuffId = stuff.Id,
                                StuffCode = stuff.Code,
                                StuffName = stuff.Name,
                                UnitId = unit.Id,
                                UnitName = unit.Name,
                                RowVersion = null,
                                CustomsHowToBuyId = null,
                                CustomsHowToBuyTitle = null,
                                CustomsHowToBuyRatio = null,
                                CustomsId = null,
                                CustomsPercent = null,
                                CustomsPrice = null,
                                CustomsTariff = null,
                                CustomsWeight = null,
                                CustomsType = null,
                                CustomsCurrencyId = null,
                                StuffBasePriceType = StuffBasePriceType.Constant,
                                TransportType = null,
                                TransportComputeType = null,
                                TransportId = null,
                                TransportPercent = null,
                                Type = StuffPriceType.BasePrice,
                                Code = "",
                                ConfirmStuffBasePriceEmployeeFullName = null,
                                ConfirmStuffBasePriceDate = null,
                                Description = description
                              };
      stuffBasePriceResult = stuffBasePriceResult.Union(stuffWithoutPrice);
      #endregion
      #region FilterResult
      if (id != null)
        stuffBasePriceResult = stuffBasePriceResult.Where(i => i.Id == id);
      if (stuffId != null)
        stuffBasePriceResult = stuffBasePriceResult.Where(i => i.StuffId == stuffId);
      if (status != null)
        stuffBasePriceResult = stuffBasePriceResult.Where(i => i.Status.HasFlag(status));
      if (statuses != null)
      {
        var s = StuffPriceStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        stuffBasePriceResult = stuffBasePriceResult.Where(i => (i.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = StuffPriceStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        stuffBasePriceResult = stuffBasePriceResult.Where(i => (i.Status & s) == 0);
      }
      if (currencyId != null)
        stuffBasePriceResult = stuffBasePriceResult.Where(i => i.CurrencyId == currencyId);
      if (userId != null)
        stuffBasePriceResult = stuffBasePriceResult.Where(i => i.UserId == userId);
      if (description != null)
        stuffBasePriceResult = stuffBasePriceResult.Where(i => i.Description == description);
      #endregion
      return stuffBasePriceResult;
    }
    #endregion
    #region GetResult
    public StuffBasePriceResult GetStuffBasePriceResult(long id)
    {
      var stuffBasePrice = GetStuffBasePriceResults(id: id)
                .FirstOrDefault();
      if (stuffBasePrice == null)
        throw new StuffBasePriceNotFoundException(id);
      return stuffBasePrice;
    }
    #endregion
    #region Get
    public StuffBasePrice GetStuffBasePrice(int id) => GetStuffBasePrice(selector: e => e, id: id);
    public TResult GetStuffBasePrice<TResult>(Expression<Func<StuffBasePrice, TResult>> selector, int id)
    {
      var stuffBasePrice = GetStuffBasePrices(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (stuffBasePrice == null)
        throw new StuffBasePriceNotFoundException(id);
      return stuffBasePrice;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffBasePriceResult> SortStuffBasePriceResult(IQueryable<StuffBasePriceResult> input,
        SortInput<StuffBasePriceSortType> options)
    {
      switch (options.SortType)
      {
        case StuffBasePriceSortType.CurrencyTitle:
          return input.OrderBy(i => i.CurrencyTitle, options.SortOrder);
        case StuffBasePriceSortType.DateTime:
          return input.OrderBy(i => i.DateTime, options.SortOrder);
        case StuffBasePriceSortType.EmployeeFullName:
          return input.OrderBy(i => i.EmployeeFullName, options.SortOrder);
        case StuffBasePriceSortType.Status:
          return input.OrderBy(i => i.Status, options.SortOrder);
        case StuffBasePriceSortType.Price:
          return input.OrderBy(i => i.Price, options.SortOrder);
        case StuffBasePriceSortType.StuffCode:
          return input.OrderBy(i => i.StuffCode, options.SortOrder);
        case StuffBasePriceSortType.StuffName:
          return input.OrderBy(i => i.StuffName, options.SortOrder);
        case StuffBasePriceSortType.UnitName:
          return input.OrderBy(i => i.UnitName, options.SortOrder);
        case StuffBasePriceSortType.Code:
          return input.OrderBy(i => i.Code, options.SortOrder);
        case StuffBasePriceSortType.ConfirmStuffBasePriceEmployeeFullName:
          return input.OrderBy(i => i.ConfirmStuffBasePriceEmployeeFullName, options.SortOrder);
        case StuffBasePriceSortType.ConfirmStuffBasePriceDate:
          return input.OrderBy(i => i.ConfirmStuffBasePriceDate, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<StuffBasePriceResult> SearchStuffBasePriceResultQuery(
        IQueryable<StuffBasePriceResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from stuffBasePrice in query
                where stuffBasePrice.CurrencyTitle.Contains(searchText) ||
                      stuffBasePrice.EmployeeFullName.Contains(searchText) ||
                      stuffBasePrice.StuffCode.Contains(searchText) ||
                      stuffBasePrice.StuffName.Contains(searchText) ||
                      stuffBasePrice.Description.Contains(searchText)
                select stuffBasePrice;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region ToResult
    public IQueryable<StuffBasePriceResult> ToStuffBasePriceResult(IQueryable<StuffBasePrice> query)
    {
      var mainUnits = App.Internals.ApplicationBase.GetUnits(
                       selector: App.Internals.ApplicationBase.ToUnitResult,
                       isMainUnit: true); ; var stuffPrices = GetStuffPrices(e => e);
      return (from stuffBasePrice in query
              join unit in mainUnits on stuffBasePrice.Stuff.UnitTypeId equals unit.UnitTypeId
              //join stuffPrice in stuffPrices on stuffBasePrice.Id equals stuffPrice.Id into mmm
              //from stuffPriceObj in mmm.DefaultIfEmpty()
              where unit.IsMainUnit == true
              select new StuffBasePriceResult()
              {
                Id = stuffBasePrice.Id,
                CurrencyId = stuffBasePrice.CurrencyId,
                CurrencyTitle = stuffBasePrice.Currency.Title,
                CurrencyDecimalDigitCount = stuffBasePrice.Currency.DecimalDigitCount,
                DateTime = stuffBasePrice.DateTime,
                UserId = stuffBasePrice.UserId,
                EmployeeFullName = stuffBasePrice.User.Employee.FirstName + " " + stuffBasePrice.User.Employee.LastName,
                EmployeeId = stuffBasePrice.User.Employee.Id,
                Status = stuffBasePrice.Status,
                Price = stuffBasePrice.Price,
                MainPrice = stuffBasePrice.MainPrice,
                StuffId = stuffBasePrice.StuffId,
                StuffCode = stuffBasePrice.Stuff.Code,
                StuffName = stuffBasePrice.Stuff.Name,
                UnitId = unit.Id,
                UnitName = unit.Name,
                RowVersion = stuffBasePrice.RowVersion,
                CustomsHowToBuyId = stuffBasePrice.StuffBasePriceCustoms.HowToBuyId,
                CustomsHowToBuyTitle = stuffBasePrice.StuffBasePriceCustoms.HowToBuy.Title,
                CustomsHowToBuyRatio = stuffBasePrice.StuffBasePriceCustoms.HowToBuyRatio,
                CustomsId = stuffBasePrice.StuffBasePriceCustoms.Id,
                CustomsPercent = stuffBasePrice.StuffBasePriceCustoms.Percent,
                CustomsPrice = stuffBasePrice.StuffBasePriceCustoms.Price,
                CustomsTariff = stuffBasePrice.StuffBasePriceCustoms.Tariff,
                CustomsWeight = stuffBasePrice.StuffBasePriceCustoms.Weight,
                CustomsType = stuffBasePrice.StuffBasePriceCustoms.Type,
                CustomsCurrencyId = stuffBasePrice.StuffBasePriceCustoms.CurrencyId,
                StuffBasePriceType = stuffBasePrice.StuffBasePriceType,
                TransportType = stuffBasePrice.StuffBasePriceTransport.Type,
                TransportComputeType = stuffBasePrice.StuffBasePriceTransport.ComputeType,
                TransportId = stuffBasePrice.StuffBasePriceTransport.Id,
                TransportPercent = stuffBasePrice.StuffBasePriceTransport.Percent,
                Type = stuffBasePrice.Type,
                Code = stuffBasePrice.Code,
                ConfirmStuffBasePriceEmployeeFullName = stuffBasePrice.ConfirmUser.Employee.FirstName + " " + stuffBasePrice.ConfirmUser.Employee.LastName,
                ConfirmStuffBasePriceDate = stuffBasePrice.ConfirmDate,
                Description = stuffBasePrice.Description
              });
      #endregion
    }
  }
}