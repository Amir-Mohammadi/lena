using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.StoreReceipt;
using lena.Models.WarehouseManagement.StoreReceiptDeleteRequestStuffSerial;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region  AddStoreReceiptDeleteRequest
    public void AddStoreReceiptDeleteRequestProcess(
        int storeReceiptId,
        string description,
        AddStuffSerialToDeleteStoreReceipt[] stuffSerials)
    {
      #region check not action status
      var storeReceiptDeleteRequests = GetStoreReceiptDeleteRequests(
          selector: e => e,
          status: StoreReceiptDeleteRequestStatus.NoAction,
          storeReceiptId: storeReceiptId,
          isDelete: false);
      if (storeReceiptDeleteRequests.Any())
        throw new StoreReceiptHasNoActionRequestException(storeReceiptId);
      #endregion
      if (stuffSerials == null && stuffSerials.Length == 0)
        throw new SerialsIsEmptyException();
      var deleteRequest = AddStoreReceiptDeleteRequest(
              storeReceiptId: storeReceiptId,
              dateTime: DateTime.Now.ToUniversalTime(),
              status: StoreReceiptDeleteRequestStatus.NoAction,
              userId: App.Providers.Security.CurrentLoginData.UserId,
              description: description,
              isDelete: false);
      foreach (var stuffSerialItem in stuffSerials)
      {
        var warehouseManagement = App.Internals.WarehouseManagement;
        var stuffSerial = warehouseManagement.GetStuffSerial(
                  serial: stuffSerialItem.Serial);
        AddStoreReceiptDeleteRequestStuffSerial(
                 storeReceiptDeleteRequestId: deleteRequest.Id,
                 stuffSerialStuffId: stuffSerial.StuffId,
                 stuffSerialStuffCode: stuffSerial.Code,
                 amount: stuffSerialItem.Amount,
                 unitId: stuffSerialItem.UnitId);
      }
    }
    public StoreReceiptDeleteRequest AddStoreReceiptDeleteRequest(
        int storeReceiptId,
        DateTime dateTime,
        StoreReceiptDeleteRequestStatus status,
        int userId,
        string description,
        bool isDelete)
    {
      var storeReceiptDeleteRequest = repository.Create<StoreReceiptDeleteRequest>();
      storeReceiptDeleteRequest.StoreReceiptId = storeReceiptId;
      storeReceiptDeleteRequest.DateTime = dateTime;
      storeReceiptDeleteRequest.Status = status;
      storeReceiptDeleteRequest.UserId = userId;
      storeReceiptDeleteRequest.IsDelete = isDelete;
      storeReceiptDeleteRequest.Description = description;
      repository.Add(storeReceiptDeleteRequest);
      return storeReceiptDeleteRequest;
    }
    #endregion
    #region Edit
    internal StoreReceiptDeleteRequest EditStoreReceiptDeleteRequest(
        int id,
        byte[] rowVersion,
        TValue<int> storeReceiptId = null,
        TValue<string> description = null,
        TValue<bool> isDelete = null,
        TValue<StoreReceiptDeleteRequestStatus> status = null)
    {
      var storeReceiptDeleteRequest = GetStoreReceiptDeleteRequest(id: id);
      return EditStoreReceiptDeleteRequest(
                storeReceiptDeleteRequest: storeReceiptDeleteRequest,
                rowVersion: rowVersion,
                storeReceiptId: storeReceiptId,
                description: description,
                isDelete: isDelete,
                status: status);
    }
    internal StoreReceiptDeleteRequest EditStoreReceiptDeleteRequest(
       StoreReceiptDeleteRequest storeReceiptDeleteRequest,
       byte[] rowVersion,
       TValue<int> storeReceiptId = null,
       TValue<string> description = null,
       TValue<bool> isDelete = null,
       TValue<StoreReceiptDeleteRequestStatus> status = null)
    {
      if (storeReceiptId != null)
        storeReceiptDeleteRequest.StoreReceiptId = storeReceiptId;
      if (description != null)
        storeReceiptDeleteRequest.Description = description;
      if (status != null)
        storeReceiptDeleteRequest.Status = status;
      if (isDelete != null)
        storeReceiptDeleteRequest.IsDelete = isDelete;
      repository.Update(rowVersion: rowVersion, entity: storeReceiptDeleteRequest);
      return storeReceiptDeleteRequest;
    }
    #endregion
    #region Accept
    public void AcceptStoreReceiptDeleteRequestProcess(
        int storeReceiptDeleteRequestId,
        string description,
        byte[] rowVersion)
    {
      var warehouseManagement = App.Internals.WarehouseManagement;
      #region CheckUserGroup
      var userId = App.Providers.Security.CurrentLoginData.UserId;
      var membershipId = App.Internals.UserManagement.GetMemberships(
                selector: e => e.Id,
                userId: userId,
                userGroupId: Models.StaticData.UserGroups.StoreReceiptDeleteRequestConfirmers.Id);
      if (!membershipId.Any())
        throw new UserHasNoPermmisionToAcceptOrRejectRequestException(userId);
      #endregion
      #region CheckDuplicatedConfirmationLogs
      var getStoreReceiptDeleteRequestConfirmationLog = warehouseManagement.GetStoreReceiptDeleteRequestConfirmationLogs(
          selector: e => e,
          storeReceiptDeleteRequestId: storeReceiptDeleteRequestId,
          confirmerUserId: userId);
      if (getStoreReceiptDeleteRequestConfirmationLog.Any())
        throw new UserCanNotAcceptOrRejectTwiceReceiptRequestException();
      #endregion
      #region Check Status
      var storeReceiptDeleteRequest = GetStoreReceiptDeleteRequest(
          id: storeReceiptDeleteRequestId);
      if (storeReceiptDeleteRequest.Status != StoreReceiptDeleteRequestStatus.NoAction)
        throw new StoreReceiptDeleteRequestStatusIsNotInNoActionException(storeReceiptDeleteRequestId);
      #endregion
      AddStoreReceiptDeleteRequestConfirmationLog(
         storeReceiptDeleteRequestId: storeReceiptDeleteRequestId,
         description: description,
         status: StoreReceiptDeleteRequestStatus.Confirmed);
      #region All the users in group must confirm the request
      var usersInConfirmerUserGroup = App.Internals.UserManagement.GetMemberships(
          selector: e => new { UserId = e.UserId, UserGroupId = e.UserGroupId },
          userGroupId: Models.StaticData.UserGroups.StoreReceiptDeleteRequestConfirmers.Id)
      .Distinct();
      var confirmationLogs = GetStoreReceiptDeleteRequestConfirmationLogs(
                selector: e => new { e.StoreReceiptDeleteRequestId, e.ConfirmerUserId, e.Status },
                storeReceiptDeleteRequestId: storeReceiptDeleteRequestId)
            .Distinct();
      var joinedResult = from membership in usersInConfirmerUserGroup
                         join log in confirmationLogs
                               on membership.UserId equals log.ConfirmerUserId
                         select new
                         {
                           UserId = log.ConfirmerUserId
                         };
      if (confirmationLogs.All(l => l.Status == StoreReceiptDeleteRequestStatus.Confirmed)
                && usersInConfirmerUserGroup.Count() == joinedResult.Count())
      {
        #region Delete
        DeleteStoreReceiptProcess(storeReceiptDeleteRequestId);
        #endregion
        EditStoreReceiptDeleteRequest(
            id: storeReceiptDeleteRequestId,
            status: StoreReceiptDeleteRequestStatus.Confirmed,
            rowVersion: rowVersion);
      }
      #endregion
    }
    #endregion
    #region Reject
    public void RejectStoreReceiptDeleteRequestProcess(
        int storeReceiptDeleteRequestId,
        string description,
        byte[] rowVersion)
    {
      var warehouseManagement = App.Internals.WarehouseManagement;
      #region CheckUserGroup
      var userId = App.Providers.Security.CurrentLoginData.UserId;
      var membershipId = App.Internals.UserManagement.GetMemberships(
                selector: e => e.Id,
                userId: userId,
                userGroupId: Models.StaticData.UserGroups.StoreReceiptDeleteRequestConfirmers.Id);
      if (!membershipId.Any())
        throw new UserHasNoPermmisionToAcceptOrRejectRequestException(userId);
      #endregion
      #region CheckDuplicatedConfirmationLogs
      var getStoreReceiptDeleteRequestConfirmationLog = warehouseManagement.GetStoreReceiptDeleteRequestConfirmationLogs(
          selector: e => e,
          storeReceiptDeleteRequestId: storeReceiptDeleteRequestId,
          confirmerUserId: userId);
      if (getStoreReceiptDeleteRequestConfirmationLog.Any())
        throw new UserCanNotAcceptOrRejectTwiceReceiptRequestException();
      #endregion
      #region RejectCurrentReceipt
      var storeReceiptDeleteRequest = GetStoreReceiptDeleteRequest(id: storeReceiptDeleteRequestId);
      if (storeReceiptDeleteRequest.Status != StoreReceiptDeleteRequestStatus.NoAction)
        throw new StoreReceiptDeleteRequestStatusIsNotInNoActionException(storeReceiptDeleteRequestId);
      EditStoreReceiptDeleteRequest(
                id: storeReceiptDeleteRequestId,
                status: StoreReceiptDeleteRequestStatus.Rejected,
                rowVersion: rowVersion);
      AddStoreReceiptDeleteRequestConfirmationLog(
                storeReceiptDeleteRequestId: storeReceiptDeleteRequestId,
                description: description,
                status: StoreReceiptDeleteRequestStatus.Rejected);
      #endregion
    }
    #endregion
    #region Remove
    public void RemoveStoreReceiptDeleteRequestProcess(
        int storeReceiptDeleteRequestId,
        byte[] rowVersion)
    {
      var storeReceiptDeleteRequest = GetStoreReceiptDeleteRequest(id: storeReceiptDeleteRequestId);
      if (storeReceiptDeleteRequest.Status != StoreReceiptDeleteRequestStatus.NoAction)
        throw new StoreReceiptDeleteRequestStatusIsNotInNoActionException(storeReceiptDeleteRequestId);
      EditStoreReceiptDeleteRequest(
                id: storeReceiptDeleteRequestId,
                isDelete: true,
                rowVersion: rowVersion);
    }
    #endregion
    #region Get
    public StoreReceiptDeleteRequest GetStoreReceiptDeleteRequest(int id) => GetStoreReceiptDeleteRequest(selector: e => e, id: id);
    public TResult GetStoreReceiptDeleteRequest<TResult>(
        Expression<Func<StoreReceiptDeleteRequest, TResult>> selector,
        int id)
    {
      var StoreReceiptDeleteRequest = GetStoreReceiptDeleteRequests(
                selector: selector,
                id: id).FirstOrDefault();
      if (StoreReceiptDeleteRequest == null)
        throw new StoreReceiptDeleteRequestNotFoundException(id);
      return StoreReceiptDeleteRequest;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStoreReceiptDeleteRequests<TResult>(
       Expression<Func<StoreReceiptDeleteRequest, TResult>> selector,
       TValue<int?> id = null,
       TValue<bool> isDelete = null,
       TValue<int?> storeReceiptId = null,
       TValue<string> stuffCode = null,
       TValue<int> stuffId = null,
       TValue<string> cooperatorName = null,
       TValue<string> storeReceiptCode = null,
       TValue<DateTime?> dateTime = null,
       TValue<DateTime?> toDate = null,
       TValue<DateTime?> fromDate = null,
       TValue<int?> userId = null,
       TValue<DateTime?> changeStatusDateTime = null,
       TValue<DateTime?> receiptDateTime = null,
       TValue<int?> changeStatusUserId = null,
       TValue<StoreReceiptDeleteRequestStatus> status = null,
       TValue<StoreReceiptType> StoreReceiptType = null,
       TValue<string> description = null
       )
    {
      var StoreReceiptDeleteRequest = repository.GetQuery<StoreReceiptDeleteRequest>();
      if (id != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(i => i.Id == id);
      if (isDelete != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(r => r.IsDelete == isDelete);
      if (storeReceiptId != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(r => r.StoreReceiptId == storeReceiptId);
      if (storeReceiptCode != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(r => r.StoreReceipt.Code == storeReceiptCode);
      if (stuffCode != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(r => r.StoreReceipt.Stuff.Code == stuffCode);
      if (stuffId != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(r => r.StoreReceipt.StuffId == stuffId);
      if (cooperatorName != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(r => r.StoreReceipt.Cooperator.Name == cooperatorName);
      if (dateTime != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(r => r.DateTime == dateTime);
      if (fromDate != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(i => i.DateTime <= toDate);
      if (userId != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(i => i.UserId == userId);
      if (receiptDateTime != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(i => i.StoreReceipt.DateTime == receiptDateTime);
      if (status != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(i => i.Status == status);
      if (StoreReceiptType != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(i => i.StoreReceipt.StoreReceiptType == StoreReceiptType);
      if (description != null)
        StoreReceiptDeleteRequest = StoreReceiptDeleteRequest.Where(i => i.Description == description);
      return StoreReceiptDeleteRequest.Select(selector);
    }
    #endregion
    #region ToResult
    public Expression<Func<StoreReceiptDeleteRequest, StoreReceiptDeleteRequestResult>> ToStoreReceiptDeleteRequestResult =
        storeReceiptDeleteRequest => new StoreReceiptDeleteRequestResult
        {
          Id = storeReceiptDeleteRequest.Id,
          StoreReceiptId = storeReceiptDeleteRequest.StoreReceiptId,
          StoreReceiptCode = storeReceiptDeleteRequest.StoreReceipt.Code,
          StuffCode = storeReceiptDeleteRequest.StoreReceipt.Stuff.Code,
          StuffName = storeReceiptDeleteRequest.StoreReceipt.Stuff.Name,
          StoreReceiptAmount = storeReceiptDeleteRequest.StoreReceipt.Amount,
          UnitId = storeReceiptDeleteRequest.StoreReceipt.UnitId,
          UnitName = storeReceiptDeleteRequest.StoreReceipt.Unit.Name,
          CooperatorName = storeReceiptDeleteRequest.StoreReceipt.Cooperator.Name,
          DateTime = storeReceiptDeleteRequest.DateTime,
          ReceiptDateTime = storeReceiptDeleteRequest.StoreReceipt.DateTime,
          Status = storeReceiptDeleteRequest.Status,
          UserId = storeReceiptDeleteRequest.UserId,
          StoreReceiptType = storeReceiptDeleteRequest.StoreReceipt.StoreReceiptType,
          CreatorUserFullName = storeReceiptDeleteRequest.User.Employee.FirstName + " " + storeReceiptDeleteRequest.User.Employee.LastName,
          Description = storeReceiptDeleteRequest.Description,
          RowVersion = storeReceiptDeleteRequest.RowVersion
        };
    #endregion
    #region Search
    public IQueryable<StoreReceiptDeleteRequestResult> SearchStoreReceiptDeleteRequestResult(
        IQueryable<StoreReceiptDeleteRequestResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText = null)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = query.Where(item =>
            item.StoreReceiptId.ToString().Contains(searchText) ||
            item.StoreReceiptType.ToString().Contains(searchText) ||
            item.CooperatorName.ToString().Contains(searchText) ||
            item.Description.Contains(searchText));
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StoreReceiptDeleteRequestResult> StoreReceiptDeleteRequestResult(
           IQueryable<StoreReceiptDeleteRequestResult> query,
        SortInput<StoreReceiptDeleteRequestSortType> sort)
    {
      switch (sort.SortType)
      {
        case StoreReceiptDeleteRequestSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case StoreReceiptDeleteRequestSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case StoreReceiptDeleteRequestSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case StoreReceiptDeleteRequestSortType.ChangeStatusDateTime:
          return query.OrderBy(a => a.ChangeStatusDateTime, sort.SortOrder);
        case StoreReceiptDeleteRequestSortType.ChangeStatusUserId:
          return query.OrderBy(a => a.ChangeStatusUserId, sort.SortOrder);
        case StoreReceiptDeleteRequestSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case StoreReceiptDeleteRequestSortType.StoreReceiptId:
          return query.OrderBy(a => a.StoreReceiptId, sort.SortOrder);
        case StoreReceiptDeleteRequestSortType.StoreReceiptCode:
          return query.OrderBy(a => a.StoreReceiptCode, sort.SortOrder);
        case StoreReceiptDeleteRequestSortType.UserId:
          return query.OrderBy(a => a.UserId, sort.SortOrder);
        case StoreReceiptDeleteRequestSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case StoreReceiptDeleteRequestSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case StoreReceiptDeleteRequestSortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case StoreReceiptDeleteRequestSortType.StoreReceiptType:
          return query.OrderBy(a => a.StoreReceiptType, sort.SortOrder);
        case StoreReceiptDeleteRequestSortType.ReceiptDateTime:
          return query.OrderBy(a => a.ReceiptDateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}