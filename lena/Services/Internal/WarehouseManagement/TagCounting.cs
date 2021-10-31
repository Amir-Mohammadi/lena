using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.WarehouseManagement.TagCounting;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    internal TagCounting AddTagCounting(
        TagCounting tagCounting,
        int stockCheckingTagId,
        double amount,
        byte unitId,
        DateTime dateTime,
        int userId)
    {

      tagCounting = tagCounting ?? repository.Create<TagCounting>();
      tagCounting.StockCheckingTagId = stockCheckingTagId;
      tagCounting.Amount = amount;
      tagCounting.UnitId = unitId;
      tagCounting.DateTime = dateTime;
      tagCounting.UserId = userId;
      tagCounting.IsDelete = false;
      repository.Add(tagCounting);
      return tagCounting;
    }
    #endregion
    #region AddTagCountingProcess
    internal TagCounting AddTagCountingProcess(
        int stockCheckingTagId,
        double amount,
        byte unitId,
        bool replaceIfExist)
    {

      #region GetStockCheckingTag
      var stockCheckingTag = GetStockCheckingTag(
              selector: e => e,
              id: stockCheckingTagId);
      #endregion

      #region GetRelatedStockCheckingPerson
      var currentUser = App.Providers.Security.CurrentLoginData;
      var stockCheckingPerson = GetStockCheckingPersons(
                stockCheckingId: stockCheckingTag.StockCheckingId,
                userId: currentUser.UserId)


                .FirstOrDefault();

      if (stockCheckingPerson == null)
        throw new TheUserIsNotDefineForThisStockCheckingException(userFullName: currentUser.UserFullName);
      #endregion
      #region Check Exist
      var oldTagCountings = GetTagCountings(
              selector: e => e,
              stockCheckingTagId: stockCheckingTagId,
              isDelete: false);
      #endregion

      #region Replace If Exist
      if (oldTagCountings.Any())
      {
        if (!replaceIfExist)
          throw new TagCountingHasExistException();
        foreach (var oldTagCounting in oldTagCountings)
        {
          RemoveTagCounting(tagCounting: oldTagCounting,
                        rowVersion: oldTagCounting.RowVersion);
        }
      }
      #endregion
      #region AddTagCounting
      var userId = App.Providers.Security.CurrentLoginData.UserId;
      var dateTime = DateTime.Now.ToUniversalTime();
      var tagCounting = AddTagCounting(
                    tagCounting: null,
                    stockCheckingTagId: stockCheckingTagId,
                    amount: amount,
                    unitId: unitId,
                    dateTime: dateTime,
                    userId: userId);
      #endregion

      #region EditStockCheckingTag
      EditStockCheckingTag(
              stockCheckingTag: stockCheckingTag,
              rowVersion: stockCheckingTag.RowVersion,
              amount: amount,
              unitId: unitId);
      #endregion
      return tagCounting;
    }
    #endregion
    #region AddSerialTagCountingProcess
    internal TagCounting AddSerialTagCountingProcess(
        int stockCheckingId,
        short warehouseId,
        string serial,
        double amount,
        byte unitId,
        bool replaceIfExist)
    {

      #region GetStockChecking
      var stockChecking = GetStockChecking(id: stockCheckingId);
      var tagTypeId = stockChecking.ActiveTagTypeId;
      #endregion
      #region GetRelatedStockCheckingPerson
      var currentUser = App.Providers.Security.CurrentLoginData;
      var stockCheckingPerson = GetStockCheckingPersons(
                stockCheckingId: stockCheckingId,
                userId: currentUser.UserId)


                .FirstOrDefault();

      if (stockCheckingPerson == null)
        throw new TheUserIsNotDefineForThisStockCheckingException(userFullName: currentUser.UserFullName);
      #endregion

      #region GetOrAddStockCheckingTag
      var stockCheckingTag = GetOrAddStockCheckingTag(
          stockCheckingId: stockCheckingId,
          warehouseId: warehouseId,
          tagTypeId: tagTypeId.Value,
          serial: serial);
      #endregion
      #region AddTagCounting
      var tagCounting = AddTagCountingProcess(
              stockCheckingTagId: stockCheckingTag.Id,
              amount: amount,
              unitId: unitId,
              replaceIfExist: replaceIfExist);
      #endregion
      return tagCounting;
    }
    #endregion
    #region Edit
    internal TagCounting EditTagCounting(
        int id,
        byte[] rowVersion,
        TValue<int> stockCheckingTagId = null,
        TValue<double> amount = null,
        TValue<byte> unitId = null,
        TValue<DateTime> dateTime = null,
        TValue<int> userId = null,
        TValue<bool> isDelete = null)
    {

      var tagCounting = GetTagCounting(id: id);
      tagCounting = EditTagCounting(
                    tagCounting: tagCounting,
                    rowVersion: rowVersion,
                    stockCheckingTagId: stockCheckingTagId,
                    amount: amount,
                    unitId: unitId,
                    dateTime: dateTime,
                    userId: userId,
                    isDelete: isDelete);
      return tagCounting;
    }
    internal TagCounting EditTagCounting(
        TagCounting tagCounting,
        byte[] rowVersion,
        TValue<int> stockCheckingTagId = null,
        TValue<double> amount = null,
        TValue<byte> unitId = null,
        TValue<DateTime> dateTime = null,
        TValue<int> userId = null,
        TValue<bool> isDelete = null)
    {

      if (stockCheckingTagId != null)
        tagCounting.StockCheckingTagId = stockCheckingTagId;
      if (amount != null)
        tagCounting.Amount = amount;
      if (unitId != null)
        tagCounting.UnitId = unitId;
      if (dateTime != null)
        tagCounting.DateTime = dateTime;
      if (userId != null)
        tagCounting.UserId = userId;
      if (isDelete != null)
        tagCounting.IsDelete = isDelete;
      repository.Update(tagCounting, rowVersion);
      return tagCounting;
    }
    #endregion
    #region Delete

    public void DeleteTagCountingProcess(
        int id,
        byte[] rowVersion)
    {

      var tagCounting = GetTagCounting(id: id);

      var stockChecking = GetStockChecking(
                tagCounting.StockCheckingTag.StockCheckingId);

      if (stockChecking.Status == StockCheckingStatus.Finished)
        throw new StockCheckingIsFinishedException(id: stockChecking.Id);

      RemoveTagCounting(id: id, rowVersion: rowVersion);
    }

    internal void RemoveTagCounting(
        int id,
        byte[] rowVersion)
    {

      EditTagCounting(id: id,
                    rowVersion: rowVersion,
                    isDelete: true);
    }
    internal void RemoveTagCounting(
        TagCounting tagCounting,
        byte[] rowVersion)
    {

      EditTagCounting(tagCounting: tagCounting,
                    rowVersion: rowVersion,
                    isDelete: true);
    }
    #endregion
    #region Get
    internal TagCounting GetTagCounting(int id) => GetTagCounting(selector: e => e, id: id);
    internal TResult GetTagCounting<TResult>(
        Expression<Func<TagCounting, TResult>> selector,
            int id)
    {

      var data = GetTagCountings(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (data == null)
        throw new TagCountingNotFoundException(id);
      return data;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetTagCountings<TResult>(
        Expression<Func<TagCounting, TResult>> selector,
        TValue<int> id = null,
        TValue<int> stockCheckingTagId = null,
        TValue<double> amount = null,
        TValue<int> unitId = null,
        TValue<DateTime> dateTime = null,
        TValue<int> userId = null,
        TValue<int> employeeId = null,
        TValue<bool> isDelete = null,
        TValue<int> stockCheckingId = null,
        TValue<int> warehouseId = null,
        TValue<int> tagTypeId = null,
        TValue<int> stuffId = null,
        TValue<string> serial = null)
    {

      var query = repository.GetQuery<TagCounting>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (stockCheckingTagId != null)
        query = query.Where(i => i.StockCheckingTagId == stockCheckingTagId);
      if (amount != null)
        query = query.Where(i => i.Amount == amount);
      if (unitId != null)
        query = query.Where(i => i.UnitId == unitId);
      if (dateTime != null)
        query = query.Where(i => i.DateTime == dateTime);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (employeeId != null)
        query = query.Where(i => i.User.Employee.Id == employeeId);
      if (isDelete != null)
        query = query.Where(i => i.IsDelete == isDelete);
      if (stockCheckingId != null)
        query = query.Where(i => i.StockCheckingTag.StockCheckingId == stockCheckingId);
      if (warehouseId != null)
        query = query.Where(i => i.StockCheckingTag.WarehouseId == warehouseId);
      if (tagTypeId != null)
        query = query.Where(i => i.StockCheckingTag.TagTypeId == tagTypeId);
      if (stuffId != null)
        query = query.Where(i => i.StockCheckingTag.StuffId == stuffId);
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial);
        query = query.Where(i => i.StockCheckingTag.StuffSerial.Serial == serial);
      }


      return query.Select(selector);
    }
    #endregion
    #region ToResult
    internal Expression<Func<TagCounting, TagCountingResult>> ToTagCountingResult =
        tagCounting => new TagCountingResult
        {
          Id = tagCounting.Id,
          UserId = tagCounting.UserId,
          EmployeeFullName = tagCounting.User.Employee.FirstName + " " + tagCounting.User.Employee.LastName,
          Amount = tagCounting.Amount,
          UnitId = tagCounting.UnitId,
          UnitName = tagCounting.Unit.Name,
          UnitConversionRatio = tagCounting.Unit.ConversionRatio,
          DateTime = tagCounting.DateTime,
          StockCheckingTagId = tagCounting.StockCheckingTagId,
          StockCheckingTagNumber = tagCounting.StockCheckingTag.Number,
          StockCheckingId = tagCounting.StockCheckingTag.StockCheckingId,
          WarehouseId = tagCounting.StockCheckingTag.WarehouseId,
          WarehouseName = tagCounting.StockCheckingTag.StockCheckingWarehouse.Warehouse.Name,
          TagTypeId = tagCounting.StockCheckingTag.TagTypeId,
          TagTypeName = tagCounting.StockCheckingTag.TagType.Name,
          StuffId = tagCounting.StockCheckingTag.StuffId,
          Serial = tagCounting.StockCheckingTag.StuffSerial.Serial,
          StuffCode = tagCounting.StockCheckingTag.Stuff.Code,
          StuffName = tagCounting.StockCheckingTag.Stuff.Name,
          IsDelete = tagCounting.IsDelete,
          RowVersion = tagCounting.RowVersion
        };
    #endregion
    #region Search
    public IQueryable<TagCountingResult> SearchTagCountingResults(
        IQueryable<TagCountingResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<TagCountingResult> SortTagCountingResults(
        IQueryable<TagCountingResult> query,
        SortInput<TagCountingSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case TagCountingSortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);
        case TagCountingSortType.EmployeeFullName:
          return query.OrderBy(i => i.EmployeeFullName, sortInput.SortOrder);
        case TagCountingSortType.Amount:
          return query.OrderBy(i => i.Amount, sortInput.SortOrder);
        case TagCountingSortType.UnitName:
          return query.OrderBy(i => i.UnitName, sortInput.SortOrder);
        case TagCountingSortType.UnitConversionRatio:
          return query.OrderBy(i => i.UnitConversionRatio, sortInput.SortOrder);
        case TagCountingSortType.DateTime:
          return query.OrderBy(i => i.DateTime, sortInput.SortOrder);
        case TagCountingSortType.StockCheckingTagNumber:
          return query.OrderBy(i => i.StockCheckingTagNumber, sortInput.SortOrder);
        case TagCountingSortType.WarehouseName:
          return query.OrderBy(i => i.WarehouseName, sortInput.SortOrder);
        case TagCountingSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sortInput.SortOrder);
        case TagCountingSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sortInput.SortOrder);
        case TagCountingSortType.IsDelete:
          return query.OrderBy(i => i.IsDelete, sortInput.SortOrder);
        case TagCountingSortType.Serial:
          return query.OrderBy(i => i.Serial, sortInput.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
