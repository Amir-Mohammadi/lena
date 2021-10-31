using System;
using System.Linq;
using lena.Domains;
using lena.Services.Common;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using lena.Models;
using lena.Models.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Production.ReturnSerialToPreviousStateRequest;
using lena.Services.Internals.Exceptions;
using lena.Services.Core;
using lena.Domains.Enums;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Add  Process
    public ReturnSerialToPreviousStateRequest AddReturnSerialToPreviousStateRequestProcess(
        string serial,
        int wrongDoerUserId,
        string description)
    {


      var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial: serial);

      var serialTransactions = App.Internals.WarehouseManagement.GetWarehouseTransactions(
                    selector: e => e,
                    serial: serial)

                .OrderByDescending(i => i.Id);
      var lastTransaction = serialTransactions.FirstOrDefault();

      var warehouseInventories = App.Internals.WarehouseManagement.GetWarehouseInventories(
                  stuffCategoryId: null,
                  serial: serial,
                  serialStatuses: null,
                  billOfMaterialVersion: null,
                  groupBySerial: true,
                  groupByBillOfMaterialVersion: false);

      if (lastTransaction == null)
        throw new SerialHasNoTransactionException(serial: serial);
      if (lastTransaction.TransactionTypeId != Models.StaticData.StaticTransactionTypes.Consum.Id)
        throw new LastTransactionTypeIsNotConsumeException(serial);
      if (warehouseInventories.Any())
        throw new SerialIsExistInWarehousesException(serial);


      var returnSerialToPreviousStateRequest = AddReturnSerialToPreviousStateRequest(
                serial: serial,
                stuffId: stuffSerial.StuffId,
                stuffSerialCode: stuffSerial.Code,
                wrongDoerUserId: wrongDoerUserId,
                description: description);

      return returnSerialToPreviousStateRequest;
    }
    #endregion

    #region Add
    public ReturnSerialToPreviousStateRequest AddReturnSerialToPreviousStateRequest(
        int stuffId,
        long stuffSerialCode,
        string serial,
        int wrongDoerUserId,
        string description)
    {

      var returnSerialToPreviousStateRequest = repository.Create<ReturnSerialToPreviousStateRequest>();
      returnSerialToPreviousStateRequest.RequestDateTime = DateTime.UtcNow;
      returnSerialToPreviousStateRequest.Serial = serial;
      returnSerialToPreviousStateRequest.StuffId = stuffId;
      returnSerialToPreviousStateRequest.StuffSerialCode = stuffSerialCode;
      returnSerialToPreviousStateRequest.UserId = App.Providers.Security.CurrentLoginData.UserId;
      returnSerialToPreviousStateRequest.WrongDoerUserId = wrongDoerUserId;
      returnSerialToPreviousStateRequest.Description = description;

      repository.Add(returnSerialToPreviousStateRequest);
      return returnSerialToPreviousStateRequest;
    }
    #endregion

    #region Get
    public ReturnSerialToPreviousStateRequest GetReturnSerialToPreviousStateRequest(int id) => GetReturnSerialToPreviousStateRequest(selector: e => e, id: id);
    public TResult GetReturnSerialToPreviousStateRequest<TResult>(
        Expression<Func<ReturnSerialToPreviousStateRequest, TResult>> selector,
        int id)
    {

      var returnSerialToPreviousStateRequest = GetReturnSerialToPreviousStateRequests(
                selector: selector,
                id: id).FirstOrDefault();
      if (returnSerialToPreviousStateRequest == null)
        throw new ReturnSerialRequestNotFoundException(id);
      return returnSerialToPreviousStateRequest;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetReturnSerialToPreviousStateRequests<TResult>(
        Expression<Func<ReturnSerialToPreviousStateRequest, TResult>> selector,
        TValue<int> id = null,
        TValue<string> serial = null,
        TValue<int> stuffId = null,
        TValue<int> stuffCode = null,
        TValue<int> userId = null,
        TValue<int> confirmerUserId = null,
        TValue<int> wrongDoerUserId = null,
        TValue<string> description = null
        )
    {

      var returnSerialToPreviousStateRequests = repository.GetQuery<ReturnSerialToPreviousStateRequest>();
      if (id != null)
        returnSerialToPreviousStateRequests = returnSerialToPreviousStateRequests.Where(i => i.Id == id);
      if (serial != null)
        returnSerialToPreviousStateRequests = returnSerialToPreviousStateRequests.Where(i => i.Serial == serial);
      if (stuffId != null)
        returnSerialToPreviousStateRequests = returnSerialToPreviousStateRequests.Where(i => i.StuffId == stuffId);
      if (stuffCode != null)
        returnSerialToPreviousStateRequests = returnSerialToPreviousStateRequests.Where(i => i.StuffSerialCode == stuffCode);
      if (userId != null)
        returnSerialToPreviousStateRequests = returnSerialToPreviousStateRequests.Where(i => i.UserId == userId);
      if (wrongDoerUserId != null)
        returnSerialToPreviousStateRequests = returnSerialToPreviousStateRequests.Where(i => i.WrongDoerUserId == wrongDoerUserId);
      return returnSerialToPreviousStateRequests.Select(selector);
    }
    #endregion

    #region Remove ReturnSerialToPreviousStateRequest
    public void RemoveReturnSerialToPreviousStateRequest(int id, byte[] rowVersion)
    {

      var returnSerialToPreviousStateRequest = GetReturnSerialToPreviousStateRequest(id: id);

    }
    #endregion

    #region Delete ReturnSerialToPreviousStateRequest
    public void DeleteReturnSerialToPreviousStateRequest(int id)
    {

      var returnSerialToPreviousStateRequest = GetReturnSerialToPreviousStateRequest(id: id);
      repository.Delete(returnSerialToPreviousStateRequest);
    }
    #endregion

    #region EditProcess
    public ReturnSerialToPreviousStateRequest EditReturnSerialToPreviousStateRequest(
         byte[] rowVersion,
        int id,
        TValue<string> serial = null,
        TValue<int> stuffId = null,
        TValue<int> stuffCode = null,
        TValue<int> userId = null,
        TValue<int> confirmerUserId = null,
        TValue<int> wrongDoerUserId = null,
        TValue<string> description = null)
    {

      var returnSerialToPreviousStateRequest = GetReturnSerialToPreviousStateRequest(id: id);
      if (serial != null)
        returnSerialToPreviousStateRequest.Serial = serial;
      if (stuffId != null)
        returnSerialToPreviousStateRequest.StuffId = stuffId;
      if (stuffCode != null)
        returnSerialToPreviousStateRequest.StuffSerialCode = stuffCode;
      if (userId != null)
        returnSerialToPreviousStateRequest.UserId = userId;
      if (confirmerUserId != null)
        returnSerialToPreviousStateRequest.ConfirmerUserId = confirmerUserId;
      if (wrongDoerUserId != null)
        returnSerialToPreviousStateRequest.WrongDoerUserId = wrongDoerUserId;
      if (description != null)
        returnSerialToPreviousStateRequest.Description = description;

      repository.Update(returnSerialToPreviousStateRequest, rowVersion);
      return returnSerialToPreviousStateRequest;
    }
    #endregion

    #region Search
    public IQueryable<ReturnSerialToPreviousStateRequestResult> SearchReturnSerialToPreviousStateRequest(IQueryable<ReturnSerialToPreviousStateRequestResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems

        )
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.Serial.Contains(searchText) ||
                item.StuffCode.Contains(searchText)
                select item;

      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ReturnSerialToPreviousStateRequestResult> SortReturnSerialToPreviousStateRequestResult(IQueryable<ReturnSerialToPreviousStateRequestResult> query,
        SortInput<ReturnSerialToPreviousStateRequestSortType> sort)
    {
      switch (sort.SortType)
      {
        case ReturnSerialToPreviousStateRequestSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ReturnSerialToPreviousStateRequestSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case ReturnSerialToPreviousStateRequestSortType.StuffId:
          return query.OrderBy(a => a.StuffId, sort.SortOrder);
        case ReturnSerialToPreviousStateRequestSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case ReturnSerialToPreviousStateRequestSortType.UserId:
          return query.OrderBy(a => a.UserId, sort.SortOrder);
        case ReturnSerialToPreviousStateRequestSortType.ConfirmerUserId:
          return query.OrderBy(a => a.ConfirmerUserId, sort.SortOrder);
        case ReturnSerialToPreviousStateRequestSortType.WrongDoerUserId:
          return query.OrderBy(a => a.WrongDoerUserId, sort.SortOrder);
        case ReturnSerialToPreviousStateRequestSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToReturnSerialToPreviousStateRequestResult
    public Expression<Func<ReturnSerialToPreviousStateRequest, ReturnSerialToPreviousStateRequestResult>> ToReturnSerialToPreviousStateRequestResult =
        returnSerialToPreviousStateRequestResult => new ReturnSerialToPreviousStateRequestResult
        {
          Id = returnSerialToPreviousStateRequestResult.Id,
          Serial = returnSerialToPreviousStateRequestResult.Serial,
          StuffId = returnSerialToPreviousStateRequestResult.StuffId,
          StuffSerialCode = returnSerialToPreviousStateRequestResult.StuffSerialCode,
          UserId = returnSerialToPreviousStateRequestResult.UserId,
          ConfirmerUserId = (int)returnSerialToPreviousStateRequestResult.ConfirmerUserId,
          WrongDoerUserId = (int)returnSerialToPreviousStateRequestResult.WrongDoerUserId,
          Description = returnSerialToPreviousStateRequestResult.Description,
          RowVersion = returnSerialToPreviousStateRequestResult.RowVersion
        };
    #endregion
  }

}