using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.ExitReceipt;
using lena.Models.WarehouseManagement.Receipt;
using lena.Models.WarehouseManagement.ExitReceiptDeleteRequestStuffSerial;


using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region  AddExitReceipt
    public void AddExitReceiptDeleteRequestProcess(
        int exitReceiptId,
        string description,
        AddStuffSerialToDeleteExitReceipt[] stuffSerials)
    {


      #region check not action status 
      var getExitReceiptDeleteRequests = GetExitReceiptDeleteRequests(
          selector: e => e,
          exitReceiptId: exitReceiptId,
          status: ExitReceiptDeleteRequestStatus.NotAction,
          isDelete: false);

      if (getExitReceiptDeleteRequests.Any())
        throw new ExitReceiptHasNoActionDeleteRequestException(exitReceiptId);
      #endregion

      #region check serials is empty 
      if (stuffSerials.Length == 0 || stuffSerials == null)
        throw new SerialsIsEmptyException();
      #endregion

      var exitReceipt = GetExitReceipt(id: exitReceiptId);
      var deleteRequest = AddExitReceiptDeleteRequest(
                exitReceipt: exitReceipt,
                createDateTime: DateTime.Now.ToUniversalTime(),
                changeStatusDateTime: null,
                status: ExitReceiptDeleteRequestStatus.NotAction,
                creatorUserId: App.Providers.Security.CurrentLoginData.UserId,
                changeStatusUserId: null,
                description: description,
                isDelete: false
              );

      foreach (var stuffSerialItem in stuffSerials)
      {
        var stuffSerial = GetStuffSerial(
                  serial: stuffSerialItem.Serial);

        AddExitReceiptDeleteRequestStuffSerial(
                  exitReceiptDeleteRequestId: deleteRequest.Id,
                  stuffSerialStuffId: stuffSerial.StuffId,
                  stuffSerialStuffCode: stuffSerial.Code,
                  amount: stuffSerialItem.Amount,
                  unitId: stuffSerialItem.UnitId);
      }

    }
    public ExitReceiptDeleteRequest AddExitReceiptDeleteRequest(
    ExitReceipt exitReceipt,
    DateTime createDateTime,
    DateTime? changeStatusDateTime,
    ExitReceiptDeleteRequestStatus status,
    int creatorUserId,
    int? changeStatusUserId,
    bool isDelete,
    string description)
    {

      var exitReceiptDeleteRequest = repository.Create<ExitReceiptDeleteRequest>();
      exitReceiptDeleteRequest.ExitReceipt = exitReceipt;
      exitReceiptDeleteRequest.CreateDateTime = createDateTime;
      exitReceiptDeleteRequest.ChangeStatusDateTime = changeStatusDateTime;
      exitReceiptDeleteRequest.Status = status;
      exitReceiptDeleteRequest.CreatorUserId = creatorUserId;
      exitReceiptDeleteRequest.ChangeStatusUserId = changeStatusUserId;
      exitReceiptDeleteRequest.IsDelete = isDelete;
      exitReceiptDeleteRequest.Description = description;
      repository.Add(exitReceiptDeleteRequest);
      return exitReceiptDeleteRequest;
    }
    #endregion


    #region Edit
    internal ExitReceiptDeleteRequest EditExitReceiptDeleteRequest(
        int id,
        byte[] rowVersion,
        TValue<int> exitReceiptId = null,
        TValue<string> description = null,
        TValue<bool> isDelete = null,
        TValue<ExitReceiptDeleteRequestStatus> status = null)
    {

      var exitReceiptDeleteRequest = GetExitReceiptDeleteRequest(id: id);

      return EditExitReceiptDeleteRequest(
                exitReceiptDeleteRequest: exitReceiptDeleteRequest,
                rowVersion: rowVersion,
                exitReceiptId: exitReceiptId,
                description: description,
                isDelete: isDelete,
                status: status);
    }



    internal ExitReceiptDeleteRequest EditExitReceiptDeleteRequest(
       ExitReceiptDeleteRequest exitReceiptDeleteRequest,
       byte[] rowVersion,
       TValue<int> exitReceiptId = null,
       TValue<string> description = null,
       TValue<bool> isDelete = null,
       TValue<ExitReceiptDeleteRequestStatus> status = null)
    {


      if (exitReceiptId != null)
        exitReceiptDeleteRequest.ExitReceiptId = exitReceiptId;
      if (description != null)
        exitReceiptDeleteRequest.Description = description;
      if (status != null)
        exitReceiptDeleteRequest.Status = status;
      if (isDelete != null)
        exitReceiptDeleteRequest.IsDelete = isDelete;
      repository.Update(rowVersion: rowVersion, entity: exitReceiptDeleteRequest);
      return exitReceiptDeleteRequest;
    }
    #endregion



    #region Get
    public ExitReceiptDeleteRequest GetExitReceiptDeleteRequest(int id) => GetExitReceiptDeleteRequest(selector: e => e, id: id);
    public TResult GetExitReceiptDeleteRequest<TResult>(
        Expression<Func<ExitReceiptDeleteRequest, TResult>> selector,
        int id)
    {

      var ExitReceiptDeleteRequest = GetExitReceiptDeleteRequests(
                selector: selector,
                id: id).FirstOrDefault();
      if (ExitReceiptDeleteRequest == null)
        throw new ExitReceiptDeleteRequestNotFoundException(id);
      return ExitReceiptDeleteRequest;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetExitReceiptDeleteRequests<TResult>(
       Expression<Func<ExitReceiptDeleteRequest, TResult>> selector,
       TValue<int?> id = null,
       TValue<bool> isDelete = null,
       TValue<int?> creatorUserId = null,
       TValue<string> description = null,
       TValue<int?> exitReceiptId = null,
       TValue<int?> stuffId = null,
       TValue<string> exitReceiptCode = null,
       TValue<DateTime?> fromCreateDateTime = null,
       TValue<DateTime?> toCreateDateTime = null,
       TValue<DateTime?> fromChangeStatusDateTime = null,
       TValue<DateTime?> toChangeStatusDateTime = null,
       TValue<DateTime?> changeStatusDateTime = null,
       TValue<int?> changeStatusUserId = null,
       TValue<ExitReceiptDeleteRequestStatus> status = null,
       TValue<byte[]> rowVersion = null)
    {

      var exitReceiptDeleteRequests = repository.GetQuery<ExitReceiptDeleteRequest>();
      if (id != null)
        exitReceiptDeleteRequests = exitReceiptDeleteRequests.Where(r => r.Id == id);

      if (isDelete != null)
        exitReceiptDeleteRequests = exitReceiptDeleteRequests.Where(r => r.IsDelete == isDelete);

      if (creatorUserId != null)
        exitReceiptDeleteRequests = exitReceiptDeleteRequests.Where(i => i.CreatorUserId == creatorUserId);

      if (description != null)
        exitReceiptDeleteRequests = exitReceiptDeleteRequests.Where(i => i.Description == description);

      if (exitReceiptId != null)
        exitReceiptDeleteRequests = exitReceiptDeleteRequests.Where(i => i.ExitReceiptId == exitReceiptId);

      if (exitReceiptCode != null)
        exitReceiptDeleteRequests = exitReceiptDeleteRequests.Where(i => i.ExitReceipt.Code == exitReceiptCode);


      if (fromCreateDateTime != null)
        exitReceiptDeleteRequests = exitReceiptDeleteRequests.Where(i => i.CreateDateTime >= fromCreateDateTime);

      if (toCreateDateTime != null)
        exitReceiptDeleteRequests = exitReceiptDeleteRequests.Where(i => i.CreateDateTime >= toCreateDateTime);

      if (fromChangeStatusDateTime != null)
        exitReceiptDeleteRequests = exitReceiptDeleteRequests.Where(i => i.ChangeStatusDateTime >= fromChangeStatusDateTime);

      if (toChangeStatusDateTime != null)
        exitReceiptDeleteRequests = exitReceiptDeleteRequests.Where(i => i.ChangeStatusDateTime >= toChangeStatusDateTime);

      if (changeStatusUserId != null)
        exitReceiptDeleteRequests = exitReceiptDeleteRequests.Where(i => i.ChangeStatusUserId == changeStatusUserId);

      if (status != null)
        exitReceiptDeleteRequests = exitReceiptDeleteRequests.Where(i => i.Status == status);

      if (stuffId != null)
      {
        var exitReceiptIds = GetPreparingSendings(
                      selector: e => e.SendProduct.ExitReceiptId,
                      stuffId: stuffId.Value);

        var exitReceipts = GetExitReceipts(selector: e => e);

        exitReceiptDeleteRequests = from item in exitReceipts
                                    join eri in exitReceiptIds on item.Id equals eri
                                    join erd in exitReceiptDeleteRequests on item.Id equals erd.ExitReceiptId
                                    select erd;
      }

      return exitReceiptDeleteRequests.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<ExitReceiptDeleteRequest, ExitReceiptDeleteRequestResult>> ToExitReceiptDeleteRequestResult =
        exitReceiptDeleteRequest => new ExitReceiptDeleteRequestResult
        {
          Id = exitReceiptDeleteRequest.Id,
          ExitReceiptId = exitReceiptDeleteRequest.ExitReceiptId,
          ExitReceiptCode = exitReceiptDeleteRequest.ExitReceipt.Code,
          CreateDateTime = exitReceiptDeleteRequest.CreateDateTime,
          ChangeStatusDateTime = exitReceiptDeleteRequest.ChangeStatusDateTime,
          CreatorUserId = exitReceiptDeleteRequest.CreatorUserId,
          ExitReceiptFullName = exitReceiptDeleteRequest.ExitReceipt.User.Employee.FirstName + " " + exitReceiptDeleteRequest.ExitReceipt.User.Employee.LastName,
          ChangeStatusUserFullName = exitReceiptDeleteRequest.ChangeStatusUser.Employee.FirstName + " " + exitReceiptDeleteRequest.ChangeStatusUser.Employee.LastName,
          CreatorUserFullName = exitReceiptDeleteRequest.CreatorUser.Employee.FirstName + " " + exitReceiptDeleteRequest.CreatorUser.Employee.LastName,
          Description = exitReceiptDeleteRequest.Description,
          Status = exitReceiptDeleteRequest.Status,
          RowVersion = exitReceiptDeleteRequest.RowVersion
        };
    #endregion

    #region Search
    public IQueryable<ExitReceiptDeleteRequestResult> SearchExitReceiptDeleteRequestResult(
        IQueryable<ExitReceiptDeleteRequestResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText = null)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = query.Where(item =>
            item.ExitReceiptId.ToString().Contains(searchText) ||
            item.ExitReceiptFullName.Contains(searchText) ||
            item.CreatorUserFullName.Contains(searchText) ||
            item.ChangeStatusUserFullName.Contains(searchText) ||
            item.Description.Contains(searchText));

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ExitReceiptDeleteRequestResult> SortExitReceiptDeleteRequestResult(
        IQueryable<ExitReceiptDeleteRequestResult> query,
        SortInput<ExitReceiptDeleteRequestSortType> sort)
    {
      switch (sort.SortType)
      {
        case ExitReceiptDeleteRequestSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ExitReceiptDeleteRequestSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case ExitReceiptDeleteRequestSortType.ExitReceiptId:
          return query.OrderBy(a => a.ExitReceiptId, sort.SortOrder);
        case ExitReceiptDeleteRequestSortType.ExitReceiptCode:
          return query.OrderBy(a => a.ExitReceiptCode, sort.SortOrder);
        case ExitReceiptDeleteRequestSortType.CreateDateTime:
          return query.OrderBy(a => a.CreateDateTime, sort.SortOrder);
        case ExitReceiptDeleteRequestSortType.ChangeStatusDateTime:
          return query.OrderBy(a => a.ChangeStatusDateTime, sort.SortOrder);
        case ExitReceiptDeleteRequestSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);


        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion


    #region Remove
    public void RemoveExitReceiptDeleteRequestProcess(
        int exitReceiptDeleteRequestId,
        byte[] rowVersion)
    {

      var exitReceiptDeleteRequest = GetExitReceiptDeleteRequest(
                id: exitReceiptDeleteRequestId);
      if (exitReceiptDeleteRequest.Status != ExitReceiptDeleteRequestStatus.NotAction)
        throw new ExitReceiptDeleteRequestStatusIsNotInNoActionException(exitReceiptDeleteRequestId);

      EditExitReceiptDeleteRequest(
                id: exitReceiptDeleteRequestId,
                isDelete: true,
                rowVersion: rowVersion);

    }
    #endregion

    #region Reject
    public void RejectExitReceiptDeleteRequestProcess(
        int exitReceiptDeleteRequestId,
        string description,
        bool isDelete,
        byte[] rowVersion)
    {

      var warehouseManagement = App.Internals.WarehouseManagement;


      #region CheckUserGroup
      var userId = App.Providers.Security.CurrentLoginData.UserId;

      var membershipId = App.Internals.UserManagement.GetMemberships(
                selector: e => e.Id,
                userId: userId,
                userGroupId: Models.StaticData.UserGroups.ExitReceiptDeleteRequestConfirmers.Id);

      if (!membershipId.Any())
        throw new UserHasNoPermmisionToAcceptOrRejectExitRequestException(userId);
      #endregion

      #region CheckDuplicatedConfirmationLogs
      var getExitReceiptDeleteRequestConfirmationLog = warehouseManagement.GetExitReceiptDeleteRequestConfirmationLogs(
          selector: e => e,
          exitReceiptDeleteRequestId: exitReceiptDeleteRequestId,
          confirmerUserId: userId);

      if (getExitReceiptDeleteRequestConfirmationLog.Any())
        throw new UserCanNotAcceptOrRejectTwiceExitReceiptRequestException();
      #endregion

      #region RejectCurrentReceipt
      var exitReceiptDeleteRequest = GetExitReceiptDeleteRequest(id: exitReceiptDeleteRequestId);
      if (exitReceiptDeleteRequest.Status != ExitReceiptDeleteRequestStatus.NotAction)
        throw new ExitReceiptDeleteRequestStatusIsNotInNoActionException(exitReceiptDeleteRequestId);

      EditExitReceiptDeleteRequest(
                id: exitReceiptDeleteRequestId,
                status: ExitReceiptDeleteRequestStatus.Rejected,
                isDelete: isDelete,
                rowVersion: rowVersion);

      AddExitReceiptDeleteRequestConfirmationLog(
                exitReceiptDeleteRequestId: exitReceiptDeleteRequestId,
                description: description,
                status: ExitReceiptDeleteRequestStatus.Rejected);
      #endregion

    }
    #endregion

    #region Accept
    public void AcceptExitReceiptDeleteRequestProcess(
       int exitReceiptDeleteRequestId,
       string description,
       byte[] rowVersion)
    {

      var warehouseManagement = App.Internals.WarehouseManagement;
      var userManagement = App.Internals.UserManagement;

      #region check user group
      var userId = App.Providers.Security.CurrentLoginData.UserId;

      var memberShipId = userManagement.GetMemberships(
                selector: e => e.Id,
                   userId: userId,
                   userGroupId: Models.StaticData.UserGroups.ExitReceiptDeleteRequestConfirmers.Id);

      if (!memberShipId.Any())
        throw new UserHasNoPermmisionToAcceptOrRejectRequestException(userId);
      #endregion

      #region checkDuplicateConfirmationLog
      var getExitReceiptDeleteRequestConfirmationLogs = warehouseManagement.GetExitReceiptDeleteRequestConfirmationLogs(
          selector: e => e,
          exitReceiptDeleteRequestId: exitReceiptDeleteRequestId,
          confirmerUserId: userId);

      if (getExitReceiptDeleteRequestConfirmationLogs.Any())
        throw new UserCanNotAcceptOrRejectTwiceExitReceiptRequestException();
      #endregion

      #region check Status
      var exitReceiptDeleteRequest = GetExitReceiptDeleteRequest(
              id: exitReceiptDeleteRequestId);

      if (exitReceiptDeleteRequest.Status != ExitReceiptDeleteRequestStatus.NotAction)
        throw new ExitReceiptDeleteRequestStatusIsNotInNoActionException(exitReceiptDeleteRequestId);
      #endregion

      AddExitReceiptDeleteRequestConfirmationLog(
          exitReceiptDeleteRequestId: exitReceiptDeleteRequestId,
          description: description,
          status: ExitReceiptDeleteRequestStatus.Accepted);

      #region all the users in group must confirmed the request

      var usersInConfirmerUserGroup = userManagement.GetMemberships(
          selector: e => new { e.UserId, e.UserGroupId },
          userGroupId: Models.StaticData.UserGroups.ExitReceiptDeleteRequestConfirmers.Id)


      .Distinct();

      var confirmationLogs = GetExitReceiptDeleteRequestConfirmationLogs(
                selector: e => new { e.ExitReceiptDeleteRequestId, e.ConfirmerUserId, e.Status },
                exitReceiptDeleteRequestId: exitReceiptDeleteRequestId)


            .Distinct();

      var joinResult = from membership in usersInConfirmerUserGroup
                       join log in confirmationLogs
                             on membership.UserId equals log.ConfirmerUserId
                       select new
                       {
                         UserId = log.ConfirmerUserId
                       };

      if (confirmationLogs.All(i => i.Status == ExitReceiptDeleteRequestStatus.Accepted) && usersInConfirmerUserGroup.Count() == joinResult.Count())
      {
        #region delete Compeletly
        DeleteExitReceiptProcess(
            exitReceiptDeleteRequestId);
        #endregion

        EditExitReceiptDeleteRequest(
            id: exitReceiptDeleteRequestId,
            status: ExitReceiptDeleteRequestStatus.Accepted,
            rowVersion: rowVersion);
      }
      #endregion
    }
    #endregion

  }
}
