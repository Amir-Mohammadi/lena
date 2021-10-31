using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.StockCheckingTag;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    internal StockCheckingTag AddStockCheckingTag(
    StockCheckingTag stockCheckingTag,
    int number,
    int stockCheckingId,
    short warehouseId,
    int stuffId,
    long? stuffSerialCode,
    int tagTypeId,
    double? amount,
    int? unitId)
    {

      stockCheckingTag = stockCheckingTag ?? repository.Create<StockCheckingTag>();
      stockCheckingTag.Number = number;
      stockCheckingTag.StockCheckingId = stockCheckingId;
      stockCheckingTag.WarehouseId = warehouseId;
      stockCheckingTag.StuffId = stuffId;
      stockCheckingTag.StuffSerialCode = stuffSerialCode;
      stockCheckingTag.TagTypeId = tagTypeId;
      stockCheckingTag.Amount = tagTypeId;
      stockCheckingTag.TagTypeId = tagTypeId;
      repository.Add(stockCheckingTag);
      return stockCheckingTag;
    }
    #endregion
    #region GetOrAdd
    internal StockCheckingTag GetOrAddStockCheckingTag(
        int stockCheckingId,
        short warehouseId,
        int tagTypeId,
        string serial)
    {

      #region GetStockCheckingTags
      var stockCheckingTags = GetStockCheckingTags(
              selector: e => e,
              stockCheckingId: stockCheckingId,
              warehouseId: warehouseId,
              serial: serial,
              tagTypeId: tagTypeId);
      #endregion
      var stockCheckingTag = stockCheckingTags.FirstOrDefault();
      if (stockCheckingTag == null)
      {
        #region GetSerialInfo
        var stuffSerial = GetStuffSerial(
                selector: e => e,
                serial: serial);
        #endregion
        #region GetMaxTagNumber
        var tagNumber = GetMaxTagNumber(
                stockCheckingId: stockCheckingId,
                warehouseId: warehouseId,
                tagTypeId: tagTypeId);
        tagNumber++;
        #endregion
        #region Add StockChekingTag
        stockCheckingTag = AddStockCheckingTag(
                stockCheckingTag: null,
                number: tagNumber,
                stockCheckingId: stockCheckingId,
                warehouseId: warehouseId,
                stuffId: stuffSerial.StuffId,
                stuffSerialCode: stuffSerial.Code,
                tagTypeId: tagTypeId,
                amount: null,
                unitId: null);
        #endregion
      }
      return stockCheckingTag;
    }
    #endregion
    #region Adds
    internal IQueryable<StockCheckingTagResult> AddStockCheckingTags(
        int stockCheckingId,
        short warehouseId,
        int tagTypeId,
        TValue<int> number = null,
        TValue<int> stuffId = null,
        TValue<StuffType> stuffType = null,
        TValue<int> stuffCategoryId = null,
        TValue<bool> hasTag = null,
        TValue<bool> isExist = null,
        TValue<string> serial = null,
        TValue<int[]> stuffIds = null)
    {

      var ids = new List<int>();
      #region GetStuffStockChekingTags
      var stuffStockCheckingTags = GetStuffStockCheckingTags(
              stockCheckingId: stockCheckingId,
              warehouseId: warehouseId,
              tagTypeId: tagTypeId,
              number: number,
              stuffId: stuffId,
              stuffIds: stuffIds,
              stuffType: stuffType,
              stuffCategoryId: stuffCategoryId,
              isExist: isExist,
              serial: serial);
      var newStockTakingVariances = SearchStuffStockCheckingTagResult(
                query: stuffStockCheckingTags.AsQueryable(),
                number: number,
                hasTag: false);
      if (hasTag == null || hasTag == true)
      {
        ids = stuffStockCheckingTags
                  .Where(i => i.Id != null)
                  .Select(i => i.Id.Value)
                  .ToList();
      }
      #endregion
      #region GetMaxTagNumber
      var tagNumber = GetMaxTagNumber(
              stockCheckingId: stockCheckingId,
              warehouseId: warehouseId,
              tagTypeId: tagTypeId);
      #endregion
      #region AddStockCheckingTags
      foreach (var tag in newStockTakingVariances)
      {
        tagNumber++;
        var stockCheckingTag = AddStockCheckingTag(
                      stockCheckingTag: null,
                      number: tagNumber,
                      stockCheckingId: tag.StockCheckingId,
                      warehouseId: tag.WarehouseId,
                      stuffId: tag.StuffId,
                      stuffSerialCode: tag.StuffSerialCode,
                      tagTypeId: tag.TagTypeId,
                      amount: null,
                      unitId: null);
        ids.Add(stockCheckingTag.Id);
      }
      #endregion
      #region GetStockCheckingTags
      var stockCheckingTags = GetStockCheckingTags(
              selector: ToStockCheckingTagResult,
              ids: ids.ToArray());
      #endregion
      return stockCheckingTags;
    }
    #endregion
    #region Edit
    internal StockCheckingTag EditStockCheckingTag(
        int id,
        byte[] rowVersion,
        TValue<int> number = null,
        TValue<int> stockCheckingId = null,
        TValue<short> warehouseId = null,
        TValue<int> stuffId = null,
        TValue<long?> stuffSerialCode = null,
        TValue<int> tagTypeId = null,
        TValue<double?> amount = null,
        TValue<byte?> unitId = null)
    {

      var stockCheckingTag = GetStockCheckingTag(id: id);
      stockCheckingTag = EditStockCheckingTag(
                    stockCheckingTag: stockCheckingTag,
                    rowVersion: rowVersion,
                    number: number,
                    stockCheckingId: stockCheckingId,
                    warehouseId: warehouseId,
                    stuffId: stuffId,
                    stuffSerialCode: stuffSerialCode,
                    tagTypeId: tagTypeId,
                    amount: amount,
                    unitId: unitId);
      return stockCheckingTag;
    }
    internal StockCheckingTag EditStockCheckingTag(
        StockCheckingTag stockCheckingTag,
        byte[] rowVersion,
        TValue<int> number = null,
        TValue<int> stockCheckingId = null,
        TValue<short> warehouseId = null,
        TValue<int> stuffId = null,
        TValue<long?> stuffSerialCode = null,
        TValue<int> tagTypeId = null,
        TValue<double?> amount = null,
        TValue<byte?> unitId = null)
    {

      if (number != null)
        stockCheckingTag.Number = number;
      if (stockCheckingId != null)
        stockCheckingTag.StockCheckingId = stockCheckingId;
      if (warehouseId != null)
        stockCheckingTag.WarehouseId = warehouseId;
      if (stuffId != null)
        stockCheckingTag.StuffId = stuffId;
      if (stuffSerialCode != null)
        stockCheckingTag.StuffSerialCode = stuffSerialCode;
      if (tagTypeId != null)
        stockCheckingTag.TagTypeId = tagTypeId;
      if (amount != null)
        stockCheckingTag.Amount = amount;
      if (unitId != null)
        stockCheckingTag.UnitId = unitId;
      repository.Update(stockCheckingTag, rowVersion);
      return stockCheckingTag;
    }
    #endregion
    #region Delete
    internal void DeleteStockCheckingTag(int id)
    {

      var record = GetStockCheckingTag(id: id);
      repository.Delete(record);
    }
    #endregion
    #region Get
    internal StockCheckingTag GetStockCheckingTag(int id) => GetStockCheckingTag(selector: e => e, id: id);
    internal TResult GetStockCheckingTag<TResult>(
        Expression<Func<StockCheckingTag, TResult>> selector,
            int id)
    {

      var data = GetStockCheckingTags(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (data == null)
        throw new StockCheckingTagNotFoundException(id);
      return data;
    }
    internal StockCheckingTag GetStockCheckingTag(string tagSerial) => GetStockCheckingTag(selector: e => e, tagSerial: tagSerial);
    internal TResult GetStockCheckingTag<TResult>(
        Expression<Func<StockCheckingTag, TResult>> selector,
        string tagSerial)
    {

      var data = GetStockCheckingTags(
                    selector: selector,
                    tagSerial: tagSerial)


                .FirstOrDefault();
      if (data == null)
        throw new StockCheckingTagNotFoundException(tagSerial);
      return data;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetStockCheckingTags<TResult>(
        Expression<Func<StockCheckingTag, TResult>> selector,
        TValue<int> id = null,
        TValue<int> number = null,
        TValue<int> stockCheckingId = null,
        TValue<int> warehouseId = null,
        TValue<int> stuffId = null,
        TValue<long?> stuffSerialCode = null,
        TValue<string> serial = null,
        TValue<int> tagTypeId = null,
        TValue<string> tagSerial = null,
        TValue<int[]> ids = null)
    {

      var query = repository.GetQuery<StockCheckingTag>();
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial);
        query = query.Where(i => i.StuffSerial.Serial == serial);
      }
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (number != null)
        query = query.Where(i => i.Number == number);
      if (stockCheckingId != null)
        query = query.Where(i => i.StockCheckingId == stockCheckingId);
      if (warehouseId != null)
        query = query.Where(i => i.WarehouseId == warehouseId);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (stuffSerialCode != null)
        query = query.Where(i => i.StuffSerialCode == stuffSerialCode);
      if (tagTypeId != null)
        query = query.Where(i => i.TagTypeId == tagTypeId);
      if (tagSerial != null)
        query = query.Where(i => i.StockCheckingId + "-" + i.WarehouseId + "-" + i.Number + "-" + i.TagTypeId == tagSerial);
      if (ids != null)
        query = query.Where(i => ids.Value.Contains(i.Id));
      return query.Select(selector);
    }
    #endregion
    #region Gets
    internal int GetMaxTagNumber(
        int stockCheckingId,
        short warehouseId,
        int tagTypeId)
    {

      var stockCheckingTags = GetStockCheckingTags(
                    selector: e => e.Number,
                    stockCheckingId: stockCheckingId,
                    warehouseId: warehouseId,
                    tagTypeId: tagTypeId);
      var tagNumber = stockCheckingTags.Any() ? stockCheckingTags.Max(i => i) : 0;
      return tagNumber;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StockCheckingTagResult> SortStockCheckingTagResult(
        IQueryable<StockCheckingTagResult> query,
        SortInput<StockCheckingTagSortType> sort)
    {
      switch (sort.SortType)
      {
        case StockCheckingTagSortType.Id:
          return query.OrderBy(i => i.Id, sort.SortOrder);
        case StockCheckingTagSortType.Number:
          return query.OrderBy(i => i.Number, sort.SortOrder);
        case StockCheckingTagSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sort.SortOrder);
        case StockCheckingTagSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sort.SortOrder);
        case StockCheckingTagSortType.StuffType:
          return query.OrderBy(i => i.StuffType, sort.SortOrder);
        case StockCheckingTagSortType.StuffCategoryName:
          return query.OrderBy(i => i.StuffCategoryName, sort.SortOrder);
        case StockCheckingTagSortType.StuffSerialCode:
          return query.OrderBy(i => i.StuffSerialCode, sort.SortOrder);
        case StockCheckingTagSortType.Serial:
          return query.OrderBy(i => i.Serial, sort.SortOrder);
        case StockCheckingTagSortType.TagTypeName:
          return query.OrderBy(i => i.TagTypeName, sort.SortOrder);
        case StockCheckingTagSortType.WarehouseName:
          return query.OrderBy(i => i.WarehouseName, sort.SortOrder);
        case StockCheckingTagSortType.StockCheckingTitle:
          return query.OrderBy(i => i.StockCheckingTitle, sort.SortOrder);
        case StockCheckingTagSortType.TagSerial:
          return query.OrderBy(i => i.TagSerial, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<StockCheckingTagResult> SearchStockCheckingTagResult(
        IQueryable<StockCheckingTagResult> query,
        StuffType? stuffType,
        int? stuffCategoryId
        )
    {
      if (stuffType != null)
        query = query.Where(i => i.StuffType == stuffType);
      if (stuffCategoryId != null)
        query = query.Where(i => i.StuffCategoryId == stuffCategoryId);
      return query;
    }
    #endregion
    #region ToResult
    internal Expression<Func<StockCheckingTag, StockCheckingTagResult>> ToStockCheckingTagResult =
        stockCheckingTag => new StockCheckingTagResult
        {
          Id = stockCheckingTag.Id,
          Number = stockCheckingTag.Number,
          TagSerial = stockCheckingTag.StockCheckingId + "-" + stockCheckingTag.WarehouseId + "-" + stockCheckingTag.Number + "-" + stockCheckingTag.TagTypeId,
          StuffId = stockCheckingTag.StuffId,
          StuffCode = stockCheckingTag.Stuff.Code,
          StuffName = stockCheckingTag.Stuff.Name,
          StuffType = stockCheckingTag.Stuff.StuffType,
          StuffCategoryId = stockCheckingTag.Stuff.StuffCategoryId,
          StuffCategoryName = stockCheckingTag.Stuff.StuffCategory.Name,
          StuffSerialCode = stockCheckingTag.StuffSerialCode,
          Serial = stockCheckingTag.StuffSerial.Serial,
          TagTypeId = stockCheckingTag.TagTypeId,
          TagTypeName = stockCheckingTag.TagType.Name,
          WarehouseId = stockCheckingTag.StockCheckingWarehouse.WarehouseId,
          WarehouseName = stockCheckingTag.StockCheckingWarehouse.Warehouse.Name,
          StockCheckingId = stockCheckingTag.StockCheckingWarehouse.StockCheckingId,
          StockCheckingTitle = stockCheckingTag.StockCheckingWarehouse.StockChecking.Title,
          Amount = stockCheckingTag.Amount,
          UnitId = stockCheckingTag.UnitId,
          UnitName = stockCheckingTag.Unit.Name,
          UnitConversionRatio = (float?)stockCheckingTag.Unit.ConversionRatio,
          RowVersion = stockCheckingTag.RowVersion,
        };
    #endregion
    #region GetStuffStockCheckingTags
    internal IQueryable<StuffStockCheckingTagResult> GetStuffStockCheckingTags(
        int stockCheckingId,
        short warehouseId,
        int tagTypeId,
        TValue<int> id = null,
        TValue<int> number = null,
        TValue<int> stuffId = null,
        TValue<int[]> stuffIds = null,
        TValue<StuffType> stuffType = null,
        TValue<int> stuffCategoryId = null,
        TValue<long?> stuffSerialCode = null,
        TValue<string> serial = null,
        TValue<bool> isExist = null)
    {


      //if (serial != null)
      //{
      //    var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial: serial)
      //        
      //;
      //    stuffId = stuffSerial.StuffId;
      //    stuffSerialCode = stuffSerial.Code;

      //}

      #region GetStuffs
      var stuffs = App.Internals.SaleManagement.GetStuffs(
              selector: e => e,
              id: stuffId,
              stuffType: stuffType,
              stuffCategoryId: stuffCategoryId,
              ids: stuffIds);

      #endregion
      #region IsExist
      if (isExist == true)
      {
        var hasTransactionStuffIds = GetWarehouseTransactions(selector: e => e.StuffId,
                  warehouseId: warehouseId).Distinct();
        stuffs = from stuff in stuffs
                 join i in hasTransactionStuffIds on stuff.Id equals i
                 select stuff;
      }
      #endregion

      #region GetTagTypes

      var tagTypes = GetTagTypes(
              selector: e => e,
              id: tagTypeId);

      #endregion

      #region GetStockCheckingTags

      var stockCheckingTags = GetStockCheckingTags(
              selector: ToStockCheckingTagResult,
              id: id,
              number: number,
              stockCheckingId: stockCheckingId,
              warehouseId: warehouseId,
              stuffId: stuffId,
              stuffSerialCode: stuffSerialCode,
              tagTypeId: tagTypeId);

      #endregion

      #region GetStockCheckingWarehouses

      var stockCheckingWarehouses = GetStockCheckingWarehouses(
              selector: e => e,
              stockCheckingId: stockCheckingId,
              warehouseId: warehouseId);

      #endregion

      #region ToResult

      var result = ToStuffStockCheckingTagResult(
          stockCheckingWarehouses: stockCheckingWarehouses,
          stuffs: stuffs,
          tagTypes: tagTypes,
          stockCheckingTagResults: stockCheckingTags);

      #endregion

      return result;
    }
    #endregion
    #region SortStuffStockCheckingTag
    public IOrderedQueryable<StuffStockCheckingTagResult> SortStuffStockCheckingTagResult(
        IQueryable<StuffStockCheckingTagResult> query,
        SortInput<StockCheckingTagSortType> sort)
    {
      switch (sort.SortType)
      {
        case StockCheckingTagSortType.Id:
          return query.OrderBy(i => i.Id, sort.SortOrder);
        case StockCheckingTagSortType.Number:
          return query.OrderBy(i => i.Number, sort.SortOrder);
        case StockCheckingTagSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sort.SortOrder);
        case StockCheckingTagSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sort.SortOrder);
        case StockCheckingTagSortType.StuffType:
          return query.OrderBy(i => i.StuffType, sort.SortOrder);
        case StockCheckingTagSortType.StuffCategoryName:
          return query.OrderBy(i => i.StuffCategoryName, sort.SortOrder);
        case StockCheckingTagSortType.StuffSerialCode:
          return query.OrderBy(i => i.StuffSerialCode, sort.SortOrder);
        case StockCheckingTagSortType.Serial:
          return query.OrderBy(i => i.Serial, sort.SortOrder);
        case StockCheckingTagSortType.TagTypeName:
          return query.OrderBy(i => i.TagTypeName, sort.SortOrder);
        case StockCheckingTagSortType.WarehouseName:
          return query.OrderBy(i => i.WarehouseName, sort.SortOrder);
        case StockCheckingTagSortType.StockCheckingTitle:
          return query.OrderBy(i => i.StockCheckingTitle, sort.SortOrder);
        case StockCheckingTagSortType.TagSerial:
          return query.OrderBy(i => i.TagSerial, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region SearchStuffStockCheckingTag
    public IQueryable<StuffStockCheckingTagResult> SearchStuffStockCheckingTagResult(
        IQueryable<StuffStockCheckingTagResult> query,
        TValue<int> number,
        TValue<bool> hasTag)
    {
      if (hasTag != null)
        query = query.Where(i => (i.Id != null) == hasTag);
      if (number != null)
        query = query.Where(i => i.Number == number);
      return query;
    }
    #endregion
    #region ToStuffStockCheckingTagResult
    internal IQueryable<StuffStockCheckingTagResult> ToStuffStockCheckingTagResult(
        IQueryable<Stuff> stuffs,
        IQueryable<TagType> tagTypes,
        IQueryable<StockCheckingWarehouse> stockCheckingWarehouses,
        IQueryable<StockCheckingTagResult> stockCheckingTagResults)
    {
      var result = from stuff in stuffs
                   from tagType in tagTypes
                   from stockCheckingWarehouse in stockCheckingWarehouses
                   join tStockCheckingTag in stockCheckingTagResults on
                   new
                   {
                     StuffId = (int?)stuff.Id,
                     TagTypeId = (int?)tagType.Id,
                     WarehouseId = (int?)stockCheckingWarehouse.WarehouseId,
                     StockCheckingId = (int?)stockCheckingWarehouse.StockCheckingId
                   } equals
                   new
                   {
                     StuffId = (int?)tStockCheckingTag.StuffId,
                     TagTypeId = (int?)tStockCheckingTag.TagTypeId,
                     WarehouseId = (int?)tStockCheckingTag.WarehouseId,
                     StockCheckingId = (int?)tStockCheckingTag.StockCheckingId
                   }
                   into tempStockCheckingTags
                   from stockCheckingTag in tempStockCheckingTags.DefaultIfEmpty()
                   select new StuffStockCheckingTagResult()
                   {
                     Id = (int?)stockCheckingTag.Id,
                     Number = (int?)stockCheckingTag.Number,
                     StuffId = stuff.Id,
                     StuffCode = stuff.Code,
                     StuffName = stuff.Name,
                     StuffType = stuff.StuffType,
                     StuffCategoryId = stuff.StuffCategoryId,
                     StuffCategoryName = stuff.StuffCategory.Name,
                     StuffSerialCode = stockCheckingTag.StuffSerialCode,
                     Serial = stockCheckingTag.Serial,
                     TagTypeId = tagType.Id,
                     TagTypeName = tagType.Name,
                     WarehouseId = stockCheckingWarehouse.WarehouseId,
                     WarehouseName = stockCheckingWarehouse.Warehouse.Name,
                     StockCheckingId = stockCheckingWarehouse.StockCheckingId,
                     StockCheckingTitle = stockCheckingWarehouse.StockChecking.Title,
                     Amount = stockCheckingTag.Amount,
                     UnitId = stockCheckingTag.UnitId,
                     UnitName = stockCheckingTag.UnitName,
                     TagSerial = stockCheckingTag.TagSerial,
                     UnitConversionRatio = stockCheckingTag.UnitConversionRatio,
                     RowVersion = stockCheckingTag.RowVersion,
                   };
      return result;
    }
    #endregion
    #region GetStockTakingVariances

    internal IQueryable<StockTakingVarianceResult> GetStockTakingVariances(
        int stockCheckingId,
        short warehouseId,
        int tagTypeId,
        TValue<string> serial = null,
        TValue<int> stuffId = null,
        TValue<StuffType> stuffType = null,
        TValue<int> stuffCategoryId = null,
        TValue<long?> stuffSerialCode = null,
        TValue<bool> groupBySerial = null,
        TValue<StockCheckingTagStatus[]> statuses = null
    )
    {

      #region GetStockChecking

      var stockChecking = GetStockChecking(id: stockCheckingId);

      #endregion

      #region GetStockCheckingWarehouse

      var stockCheckingWarehouses = GetStockCheckingWarehouses(
              selector: ToStockCheckingWarehouseResult,
              stockCheckingId: stockCheckingId,
              warehouseId: warehouseId);
      var stockCheckingWarehousesList = stockCheckingWarehouses.ToList();

      #endregion

      #region GetTagTypes

      var tagTypes = GetTagTypes(
              selector: e => e,
              id: tagTypeId);
      var tagTypesList = tagTypes.ToList();

      #endregion

      #region GetStuffs

      var stuffs = App.Internals.SaleManagement.GetStuffs(
              selector: i => new
              {
                Id = i.Id,
                Code = i.Code,
                Name = i.Name,
                StuffType = i.StuffType,
                StuffCategoryId = i.StuffCategoryId,
                StuffCategoryName = i.StuffCategory.Name,
                UnitTypeId = i.UnitTypeId
              },
              id: stuffId,
              stuffType: stuffType,
              stuffCategoryId: stuffCategoryId);

      var stuffsList = stuffs.ToList();

      #endregion

      #region unit

      var units = App.Internals.ApplicationBase.GetUnits(
              selector: e => e,
              isMainUnit: true);
      var unitsList = units.ToList();

      #endregion

      #region GetStockCheckingTags

      var stockCheckingTags = GetStockCheckingTags(
              selector: i => new
              {
                Id = i.Id,
                StockCheckingId = i.StockCheckingId,
                WarehouseId = i.WarehouseId,
                StuffId = i.StuffId,
                StuffSerialCode = i.StuffSerialCode,
                Serial = i.StuffSerial.Serial,
                Amount = i.Amount * i.Unit.ConversionRatio,
                UnitRialPrice = i.StuffSerial.UnitRialPrice
              },
              stockCheckingId: stockCheckingId,
              warehouseId: warehouseId,
              stuffId: stuffId,
              stuffSerialCode: stuffSerialCode,
              serial: serial,
              tagTypeId: tagTypeId);
      var stockCheckingTagsList = stockCheckingTags.ToList();

      #endregion
      IEnumerable<StockTakingVarianceResult> result;

      if (groupBySerial == true)
      {
        #region TagCounting

        var tagCountings = GetTagCountings(
                selector: i => new
                {
                  StockCheckingTagId = i.StockCheckingTagId,
                  DateTime = i.DateTime,
                  StuffSerialCode = i.StockCheckingTag.StuffSerialCode
                },
                stockCheckingId: stockCheckingId,
                tagTypeId: tagTypeId,
                warehouseId: warehouseId,
                isDelete: false);
        var tagCountingsList = tagCountings.ToList();

        #endregion

        #region QtyCorrectionRequest

        var qtyCorrectionRequests = GetQtyCorrectionRequests(
                selector: i => new
                {
                  Id = i.StockCheckingTagId,
                  StuffSerialCode = i.StuffSerialCode,
                  Status = i.Status
                },
                types: new[]
                {
                                QtyCorrectionRequestType.IncreaseStockChecking,
                                QtyCorrectionRequestType.DecreaseStockChecking
              });
        var qtyCorrectionRequestsList = qtyCorrectionRequests.ToList();

        #endregion

        #region GetSerialStocks

        var serialInventories = App.Internals.WarehouseManagement.GetWarehouseInventories(
                warehouseId: warehouseId,
                groupBySerial: true,
                toEffectDateTime: stockChecking.StartDate,
                serial: serial)


            .Where(x => x.TotalAmount > 0);
        var serialInventoriesList = serialInventories.ToList();

        #endregion

        #region GetStuffSerials

        var stuffSerialsInWarehouse = serialInventoriesList.Select(i => new
        {
          StuffId = i.StuffId,
          Code = i.StuffSerialCode,
          Serial = i.Serial,
          UnitRialPrice = i.UnitRialPrice
        });

        var countedStuffSerials = stockCheckingTagsList.Select(i => new
        {
          StuffId = i.StuffId,
          Code = i.StuffSerialCode,
          Serial = i.Serial,
          UnitRialPrice = i.UnitRialPrice
        });

        var stuffSerials = stuffSerialsInWarehouse.Union(countedStuffSerials);
        var stuffSerialsList = stuffSerials.ToList();

        #endregion

        #region TagSerialGroup

        var tagSerialGroup = from tag in stockCheckingTagsList
                             group tag by new { tag.StuffId, tag.StuffSerialCode, tag.Id }
            into gItems
                             select new
                             {
                               Id = gItems.Key.Id,
                               StuffId = gItems.Key.StuffId,
                               StuffSerialCode = gItems.Key.StuffSerialCode,
                               Amount = gItems.Sum(i => i.Amount)
                             };
        var tagSerialGroupList = tagSerialGroup.ToList();

        #endregion

        #region TagStuffGroup

        var tagStuffGroup = from tagSerial in tagSerialGroupList
                            group tagSerial by tagSerial.StuffId
            into gItems
                            select new
                            {
                              StuffId = gItems.Key,
                              Amount = gItems.Sum(i => i.Amount)
                            };
        var tagStuffGroupList = tagStuffGroup.ToList();

        #endregion

        #region TagQuery

        var tagQuery = from stockCheckingWarehouse in stockCheckingWarehousesList
                       from tagStuff in tagStuffGroupList
                       from tagType in tagTypesList
                       join tagSerial in tagSerialGroupList on tagStuff.StuffId equals tagSerial.StuffId
                       join tagCounting in tagCountingsList on tagSerial.Id equals tagCounting
                                 .StockCheckingTagId into temptagCountings
                       from temptagCounting in temptagCountings.DefaultIfEmpty()
                       join qtyCorrectionRequest in qtyCorrectionRequestsList on
                                 new
                                 {
                                   StuffSerialCode = tagSerial.StuffSerialCode,
                                   Id = tagSerial.Id
                                 }
                                 equals new
                                 {
                                   StuffSerialCode = qtyCorrectionRequest.StuffSerialCode,
                                   Id = qtyCorrectionRequest.Id ?? 0
                                 }
                                 into tempqtyCorrectionRequests
                       from tempqtyCorrectionRequest in tempqtyCorrectionRequests.DefaultIfEmpty()
                       join stuff in stuffsList on tagStuff.StuffId equals stuff.Id
                       join unit in unitsList on stuff.UnitTypeId equals unit.UnitTypeId
                       join tStuffSerial in stuffSerialsList on
                                 new
                                 {
                                   StuffId = tagSerial.StuffId,
                                   stuffSerialCode = tagSerial.StuffSerialCode
                                 }
                                 equals new
                                 {
                                   StuffId = tStuffSerial.StuffId,
                                   stuffSerialCode = (long?)tStuffSerial.Code
                                 }
                                 into tempStuffSerials
                       from stuffSerial in tempStuffSerials.DefaultIfEmpty()
                       select new StockTakingVarianceResult
                       {
                         Id = tagSerial.Id,
                         StockCheckingId = stockCheckingWarehouse.StockCheckingId,
                         StockCheckingTitle = stockCheckingWarehouse.StockCheckingTitle,
                         WarehouseId = stockCheckingWarehouse.WarehouseId,
                         WarehouseName = stockCheckingWarehouse.WarehouseName,
                         TagTypeId = tagType.Id,
                         TagTypeName = tagType.Name,
                         StuffId = tagStuff.StuffId,
                         StuffCode = stuff.Code,
                         StuffName = stuff.Name,
                         StuffType = stuff.StuffType,
                         StuffCategoryId = stuff.StuffCategoryId,
                         StuffCategoryName = stuff.StuffCategoryName,
                         UnitId = unit.Id,
                         UnitName = unit.Name,
                         UnitConversionRatio = unit.ConversionRatio,
                         StuffSerialCode = tagSerial.StuffSerialCode,
                         UnitRialPrice = stuffSerial?.UnitRialPrice ?? 0,
                         Serial = stuffSerial?.Serial,
                         TagAmount = tagSerial.Amount ?? 0,
                         TagCountingTotal = tagStuff.Amount ?? 0,
                         StockTotal = 0,
                         StockSerialAmount = 0,
                         IsReCount = stockChecking.Status == StockCheckingStatus.Finished
                                             && temptagCounting?.DateTime > stockChecking.EndDate
                                     ? true
                                     : false,
                         QtyCorrectionRequestStatus = tempqtyCorrectionRequest?.Status
                       };
        var tagQueryList = tagQuery.ToList();

        #endregion

        #region StuffStocks

        var stuffStocks = from serialStock in serialInventoriesList
                          group serialStock by serialStock.StuffId
            into gItems
                          select new
                          {
                            StuffId = gItems.Key,
                            Amount = gItems.Sum(i => i.TotalAmount)
                          };
        var stuffStocksList = stuffStocks.ToList();

        #endregion

        #region StockQuery

        var stockQuery = from stockCheckingWarehouse in stockCheckingWarehousesList
                         from stuffStock in stuffStocksList
                         from tagType in tagTypesList
                         join serialStock in serialInventoriesList on stuffStock.StuffId equals serialStock.StuffId
                         join stuff in stuffsList on stuffStock.StuffId equals stuff.Id
                         join unit in unitsList on stuff.UnitTypeId equals unit.UnitTypeId
                         join tStuffSerial in stuffSerialsList on
                                   new { StuffId = serialStock.StuffId, stuffSerialCode = serialStock.StuffSerialCode }
                                   equals new
                                   {
                                     StuffId = tStuffSerial.StuffId,
                                     stuffSerialCode = (long?)tStuffSerial.Code
                                   } into tempStuffSerials
                         from stuffSerial in tempStuffSerials.DefaultIfEmpty()
                         select new StockTakingVarianceResult
                         {
                           StockCheckingId = stockCheckingWarehouse.StockCheckingId,
                           StockCheckingTitle = stockCheckingWarehouse.StockCheckingTitle,
                           WarehouseId = stockCheckingWarehouse.WarehouseId,
                           WarehouseName = stockCheckingWarehouse.WarehouseName,
                           TagTypeId = tagType.Id,
                           TagTypeName = tagType.Name,
                           StuffId = stuffStock.StuffId,
                           StuffCode = stuff.Code,
                           StuffName = stuff.Name,
                           StuffType = stuff.StuffType,
                           StuffCategoryId = stuff.StuffCategoryId,
                           StuffCategoryName = stuff.StuffCategoryName,
                           UnitId = unit.Id,
                           UnitName = unit.Name,
                           UnitConversionRatio = unit.ConversionRatio,
                           StuffSerialCode = serialStock.StuffSerialCode,
                           Serial = stuffSerial?.Serial,
                           TagAmount = 0,
                           TagCountingTotal = 0,
                           StockTotal = stuffStock.Amount ?? 0,
                           UnitRialPrice = serialStock.UnitRialPrice ?? 0,
                           StockSerialAmount = serialStock.TotalAmount ?? 0,
                           IsReCount = false,
                           QtyCorrectionRequestStatus = null
                         };

        var stockQueryList = stockQuery.ToList();

        #endregion

        #region LeftJoin

        var leftJoin = from tag in tagQueryList

                       join tStuffStock in stuffStocksList on tag.StuffId equals tStuffStock.StuffId
                             into tempStuffStocks
                       from stuffStock in tempStuffStocks.DefaultIfEmpty()

                       join tStock in stockQueryList on new { tag.StuffId, tag.StuffSerialCode } equals
                                 new { tStock.StuffId, tStock.StuffSerialCode } into tempStocks
                       from stock in tempStocks.DefaultIfEmpty()

                       join tStuffSerial in stuffSerialsList on
                                  new { StuffId = tag.StuffId, stuffSerialCode = tag.StuffSerialCode }
                                  equals new
                                  {
                                    StuffId = tStuffSerial.StuffId,
                                    stuffSerialCode = (long?)tStuffSerial.Code
                                  } into tempStuffSerials
                       from stuffSerial in tempStuffSerials.DefaultIfEmpty()

                       select new StockTakingVarianceResult
                       {
                         Id = tag.Id,
                         StockCheckingId = tag.StockCheckingId,
                         StockCheckingTitle = tag.StockCheckingTitle,
                         WarehouseId = tag.WarehouseId,
                         WarehouseName = tag.WarehouseName,
                         TagTypeId = tag.TagTypeId,
                         TagTypeName = tag.TagTypeName,
                         StuffId = tag.StuffId,
                         StuffCode = tag.StuffCode,
                         StuffName = tag.StuffName,
                         StuffType = tag.StuffType,
                         StuffCategoryId = tag.StuffCategoryId,
                         StuffCategoryName = tag.StuffCategoryName,
                         UnitId = tag.UnitId,
                         UnitName = tag.UnitName,
                         UnitConversionRatio = tag.UnitConversionRatio,
                         StuffSerialCode = tag.StuffSerialCode,
                         Serial = tag.Serial,
                         TagAmount = tag.TagAmount,
                         TagCountingTotal = tag.TagCountingTotal,
                         UnitRialPrice = stuffSerial.UnitRialPrice ?? 0,
                         StockTotal = stuffStock?.Amount ?? 0,
                         StockSerialAmount = stock?.StockSerialAmount ?? 0,
                         StockCheckingTagStatus = tag.TagAmount == null
                                     ? StockCheckingTagStatus.NotCounted
                                     : tag.Serial == null && stock?.StockSerialAmount == null
                                         ? StockCheckingTagStatus.NonSerialTag
                                         : tag.Serial != null && stock?.StockSerialAmount == null
                                             ? StockCheckingTagStatus.NotInventory
                                             : stock?.StockSerialAmount == tag.TagAmount
                                                 ? StockCheckingTagStatus.CorrectCounting
                                                 : StockCheckingTagStatus.Contradiction,
                         IsReCount = tag.IsReCount,
                         QtyCorrectionRequestStatus = tag.QtyCorrectionRequestStatus
                       };

        #endregion

        #region RightJoin

        var rightJoin = from stock in stockQueryList

                        join tTag in tagQueryList on new { stock.StuffId, stock.StuffSerialCode }
                                  equals new { tTag.StuffId, tTag.StuffSerialCode } into tempTags
                        from tag in tempTags.DefaultIfEmpty()

                        join tStuffSerial in stuffSerialsList on
                                 new { StuffId = stock.StuffId, stuffSerialCode = stock.StuffSerialCode }
                                 equals new
                                 {
                                   StuffId = tStuffSerial.StuffId,
                                   stuffSerialCode = (long?)tStuffSerial.Code
                                 } into tempStuffSerials
                        from stuffSerial in tempStuffSerials.DefaultIfEmpty()

                        select new StockTakingVarianceResult
                        {
                          Id = null,
                          StockCheckingId = stock.StockCheckingId,
                          StockCheckingTitle = stock.StockCheckingTitle,
                          WarehouseId = stock.WarehouseId,
                          WarehouseName = stock.WarehouseName,
                          TagTypeId = stock.TagTypeId,
                          TagTypeName = stock.TagTypeName,
                          StuffId = stock.StuffId,
                          StuffCode = stock.StuffCode,
                          StuffName = stock.StuffName,
                          StuffType = stock.StuffType,
                          StuffCategoryId = stock.StuffCategoryId,
                          StuffCategoryName = stock.StuffCategoryName,
                          UnitId = stock.UnitId,
                          UnitName = stock.UnitName,
                          UnitConversionRatio = stock.UnitConversionRatio,
                          StuffSerialCode = stock.StuffSerialCode,
                          Serial = stock.Serial,
                          TagAmount = tag?.TagAmount ?? 0,
                          UnitRialPrice = stuffSerial?.UnitRialPrice ?? 0,
                          TagCountingTotal = tag?.TagCountingTotal ?? 0,
                          StockTotal = stock.StockTotal,
                          StockSerialAmount = stock.StockSerialAmount,
                          StockCheckingTagStatus = tag?.TagAmount == null
                                      ? StockCheckingTagStatus.NotCounted
                                      : stock.StockSerialAmount == null && stock.Serial == null
                                          ? StockCheckingTagStatus.NonSerialTag
                                          : stock.StockSerialAmount == null && stock.Serial != null
                                              ? StockCheckingTagStatus.NotInventory
                                              : stock.StockSerialAmount == tag.TagAmount
                                                  ? StockCheckingTagStatus.CorrectCounting
                                                  : StockCheckingTagStatus.Contradiction,
                          IsReCount = stock.IsReCount,
                          QtyCorrectionRequestStatus = stock.QtyCorrectionRequestStatus
                        };

        #endregion

        #region Union

        if (statuses == null)
        {
          result = leftJoin.Union(rightJoin, new StockTakingVarianceComparer());
        }
        else
        {
          result = leftJoin.Union(rightJoin, new StockTakingVarianceComparer())
                    .Where(x => statuses.Value.Any(i => i == x.StockCheckingTagStatus));
        }

        #endregion
      }
      else
      {

        #region TagStuffGroup

        var tagStuffGroup = from tagStuff in stockCheckingTagsList
                            group tagStuff by tagStuff.StuffId
            into gItems
                            select new
                            {
                              StuffId = gItems.Key,
                              Amount = gItems.Sum(i => i.Amount)
                            };
        var tagStuffGroupList = tagStuffGroup.ToList();

        #endregion

        #region TagQuery

        var tagQuery = from stockCheckingWarehouse in stockCheckingWarehousesList
                       from tagStuff in tagStuffGroupList
                       from tagType in tagTypesList
                       join stuff in stuffsList on tagStuff.StuffId equals stuff.Id
                       join unit in unitsList on stuff.UnitTypeId equals unit.UnitTypeId
                       select new StockTakingVarianceResult
                       {
                         StockCheckingId = stockCheckingWarehouse.StockCheckingId,
                         StockCheckingTitle = stockCheckingWarehouse.StockCheckingTitle,
                         WarehouseId = stockCheckingWarehouse.WarehouseId,
                         WarehouseName = stockCheckingWarehouse.WarehouseName,
                         TagTypeId = tagType.Id,
                         TagTypeName = tagType.Name,
                         StuffId = tagStuff.StuffId,
                         StuffCode = stuff.Code,
                         StuffName = stuff.Name,
                         StuffType = stuff.StuffType,
                         StuffCategoryId = stuff.StuffCategoryId,
                         StuffCategoryName = stuff.StuffCategoryName,
                         UnitId = unit.Id,
                         UnitName = unit.Name,
                         UnitConversionRatio = unit.ConversionRatio,
                         StuffSerialCode = null,
                         Serial = "",
                         UnitRialPrice = 0,
                         TagAmount = 0,
                         TagCountingTotal = tagStuff?.Amount ?? 0,
                         StockTotal = 0,
                         StockSerialAmount = 0
                       };
        var tagQueryList = tagQuery.ToList();

        #endregion

        #region GetstuffStocks

        var stuffStocks = App.Internals.WarehouseManagement.GetWarehouseInventories(
                warehouseId: warehouseId,
                toEffectDateTime: stockChecking.StartDate)


            .Where(x => x.TotalAmount > 0);
        var stuffStocksList = stuffStocks.ToList();

        #endregion

        #region StockQuery

        var stockQuery = from stockCheckingWarehouse in stockCheckingWarehousesList
                         from stuffstock in stuffStocksList
                         from tagType in tagTypesList
                         join stuff in stuffsList on stuffstock.Id equals stuff.Id
                         join unit in unitsList on stuff.UnitTypeId equals unit.UnitTypeId
                         select new StockTakingVarianceResult
                         {
                           StockCheckingId = stockCheckingWarehouse.StockCheckingId,
                           StockCheckingTitle = stockCheckingWarehouse.StockCheckingTitle,
                           WarehouseId = stockCheckingWarehouse.WarehouseId,
                           WarehouseName = stockCheckingWarehouse.WarehouseName,
                           TagTypeId = tagType.Id,
                           TagTypeName = tagType.Name,
                           StuffId = stuff.Id,
                           StuffCode = stuff.Code,
                           StuffName = stuff.Name,
                           StuffType = stuff.StuffType,
                           StuffCategoryId = stuff.StuffCategoryId,
                           StuffCategoryName = stuff.StuffCategoryName,
                           UnitId = unit.Id,
                           UnitName = unit.Name,
                           UnitConversionRatio = unit.ConversionRatio,
                           StuffSerialCode = null,
                           UnitRialPrice = 0,
                           Serial = "",
                           TagAmount = 0,
                           TagCountingTotal = 0,
                           StockTotal = stuffstock.AvailableAmount ?? 0,
                           StockSerialAmount = 0
                         };

        var stockQueryList = stockQuery.ToList();

        #endregion

        #region LeftJoin

        var leftJoin = from tag in tagQueryList
                       join tStock in stockQueryList on new { tag.StuffId, tag.StuffSerialCode } equals
                                 new { tStock.StuffId, tStock.StuffSerialCode } into tempStocks
                       from stock in tempStocks.DefaultIfEmpty()
                       select new StockTakingVarianceResult
                       {
                         StockCheckingId = tag.StockCheckingId,
                         StockCheckingTitle = tag.StockCheckingTitle,
                         WarehouseId = tag.WarehouseId,
                         WarehouseName = tag.WarehouseName,
                         TagTypeId = tag.TagTypeId,
                         TagTypeName = tag.TagTypeName,
                         StuffId = tag.StuffId,
                         StuffCode = tag.StuffCode,
                         StuffName = tag.StuffName,
                         StuffType = tag.StuffType,
                         StuffCategoryId = tag.StuffCategoryId,
                         StuffCategoryName = tag.StuffCategoryName,
                         UnitId = tag.UnitId,
                         UnitName = tag.UnitName,
                         UnitConversionRatio = tag.UnitConversionRatio,
                         UnitRialPrice = tag.UnitRialPrice,
                         StuffSerialCode = tag.StuffSerialCode,
                         Serial = tag.Serial,
                         TagAmount = tag.TagAmount,
                         TagCountingTotal = tag.TagCountingTotal,
                         StockTotal = stock?.StockTotal ?? 0,
                         StockSerialAmount = stock?.StockSerialAmount ?? 0
                       };

        #endregion

        #region RightJoin

        var rightJoin = from stock in stockQueryList
                        join tTag in tagQueryList on new { stock.StuffId, stock.StuffSerialCode }
                                  equals new { tTag.StuffId, tTag.StuffSerialCode } into tempTags
                        from tag in tempTags.DefaultIfEmpty()
                        select new StockTakingVarianceResult
                        {
                          StockCheckingId = stock.StockCheckingId,
                          StockCheckingTitle = stock.StockCheckingTitle,
                          WarehouseId = stock.WarehouseId,
                          WarehouseName = stock.WarehouseName,
                          TagTypeId = stock.TagTypeId,
                          TagTypeName = stock.TagTypeName,
                          StuffId = stock.StuffId,
                          StuffCode = stock.StuffCode,
                          StuffName = stock.StuffName,
                          StuffType = stock.StuffType,
                          StuffCategoryId = stock.StuffCategoryId,
                          StuffCategoryName = stock.StuffCategoryName,
                          UnitId = stock.UnitId,
                          UnitName = stock.UnitName,
                          UnitRialPrice = stock.UnitRialPrice,
                          UnitConversionRatio = stock.UnitConversionRatio,
                          StuffSerialCode = stock.StuffSerialCode,
                          Serial = stock.Serial,
                          TagAmount = tag?.TagAmount ?? 0,
                          TagCountingTotal = tag?.TagCountingTotal ?? 0,
                          StockTotal = stock.StockTotal,
                          StockSerialAmount = stock.StockSerialAmount,
                        };
        #endregion

        #region Union

        result = leftJoin.Union(rightJoin, new StockTakingVarianceComparer());

        #endregion
      }

      return result.AsQueryable();

    }

    #endregion
    #region SortStockTakingVariance
    public IOrderedQueryable<StockTakingVarianceResult> SortStockTakingVarianceResult(
        IQueryable<StockTakingVarianceResult> query,
        SortInput<StockTakingVarianceSortType> sort)
    {
      switch (sort.SortType)
      {
        case StockTakingVarianceSortType.StockCheckingId:
          return query.OrderBy(i => i.StockCheckingId, sort.SortOrder);
        case StockTakingVarianceSortType.StockCheckingTitle:
          return query.OrderBy(i => i.StockCheckingTitle, sort.SortOrder);
        case StockTakingVarianceSortType.WarehouseId:
          return query.OrderBy(i => i.WarehouseId, sort.SortOrder);
        case StockTakingVarianceSortType.WarehouseName:
          return query.OrderBy(i => i.WarehouseName, sort.SortOrder);
        case StockTakingVarianceSortType.StuffId:
          return query.OrderBy(i => i.StuffId, sort.SortOrder);
        case StockTakingVarianceSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sort.SortOrder);
        case StockTakingVarianceSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sort.SortOrder);
        case StockTakingVarianceSortType.StuffType:
          return query.OrderBy(i => i.StuffType, sort.SortOrder);
        case StockTakingVarianceSortType.StuffCategoryId:
          return query.OrderBy(i => i.StuffCategoryId, sort.SortOrder);
        case StockTakingVarianceSortType.StuffCategoryName:
          return query.OrderBy(i => i.StuffCategoryName, sort.SortOrder);
        case StockTakingVarianceSortType.StuffSerialCode:
          return query.OrderBy(i => i.StuffSerialCode, sort.SortOrder);
        case StockTakingVarianceSortType.Serial:
          return query.OrderBy(i => i.Serial, sort.SortOrder);
        case StockTakingVarianceSortType.TagAmount:
          return query.OrderBy(i => i.TagAmount, sort.SortOrder);
        case StockTakingVarianceSortType.TagCountingTotal:
          return query.OrderBy(i => i.TagCountingTotal, sort.SortOrder);
        case StockTakingVarianceSortType.StockTotal:
          return query.OrderBy(i => i.StockTotal, sort.SortOrder);
        case StockTakingVarianceSortType.StockSerialAmount:
          return query.OrderBy(i => i.StockSerialAmount, sort.SortOrder);
        case StockTakingVarianceSortType.ContradictionTotal:
          return query.OrderBy(i => i.ContradictionTotal, sort.SortOrder);
        case StockTakingVarianceSortType.ContradictionAmount:
          return query.OrderBy(i => i.ContradictionAmount, sort.SortOrder);
        case StockTakingVarianceSortType.ContradictionAmountRialPrice:
          return query.OrderBy(i => i.ContradictionAmountRialPrice, sort.SortOrder);
        case StockTakingVarianceSortType.UnitName:
          return query.OrderBy(i => i.UnitName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search

    public IQueryable<StockTakingVarianceResult> SearchStockTakingVarianceResult(
        IQueryable<StockTakingVarianceResult> query,
        TValue<string> serial = null,
        TValue<string[]> serials = null,
        TValue<int[]> stuffIds = null)
    {
      if (serial != null)
        query = query.Where(i => i.Serial == serial);
      if (serials != null)
        query = query.Where(i => serials.Value.Contains(i.Serial));
      if (stuffIds != null)
        query = query.Where(i => stuffIds.Value.Contains(i.StuffId));

      return query;
    }

    #endregion
  }
}
