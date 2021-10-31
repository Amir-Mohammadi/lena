using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.Ladings;
//using System.Data.Entity.SqlServer;
//using System.Data.Entity;
using lena.Models.Supplies.LadingItem;
using lena.Models.Supplies.LadingCustomhouseLog;

using lena.Models.Supplies.LadingItemDetail;
using lena.Models.Supplies.LadingChangeRequest;
using lena.Models.Supplies.LadingBlocker;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {

    #region AddLadingChangeRequestProcess
    public LadingChangeRequest AddLadingChangeRequestProcess(
       int ladingId,
       string description,
       LadingType ladingType,
       AddLadingItemDetailInput[] addLadingItemDetailChanges,
       EditLadingItemDetailInput[] editLadingItemDetailChanges,
       DeleteLadingItemDetailInput[] deleteLadingItemDetailChanges
        )
    {

      #region Check HasLadingChangeRequest
      var lading = GetLading(id: ladingId);
      if (lading.HasLadingChangeRequest)
      {
        throw new LadingHasChangeRequestException(id: lading.Id);
      }
      if (lading.Type != LadingType.Lading)
      {
        throw new LadingChangeRequestCanNotEditException(id: lading.Id);
      }
      EditLading(
                ladingId: lading.Id,
                rowVersion: lading.RowVersion,
                hasLadingChangeRequest: true);
      #endregion

      #region AddLadingChangeRequest
      var ladingChangeRequest = AddLadingChangeRequest(
          ladingId: ladingId,
          ladingType: ladingType,
          description: description);
      #endregion

      #region AddLadingItemDetailChanges
      foreach (var item in addLadingItemDetailChanges)
      {
        InsertToAddLadingItemDetailChange(
               cargoItemId: item.CargoItemId,
               cargoItemDetailId: item.CargoItemDetailId,
               ladingItemId: (TValue<int>)item.LadingItemId,
               qty: item.Qty,
               ladingChangeRequestId: ladingChangeRequest.Id);
      }
      #endregion

      #region EditLadingItemDetailChanges
      foreach (var item in editLadingItemDetailChanges)
      {
        InsertToEditLadingItemDetailChange(
               qty: item.Qty,
               ladingItemDetailId: item.Id,
               cargoItemId: item.CargoItemId,
               cargoItemDetailId: item.CargoItemDetailId,
               ladingItemId: (TValue<int>)item.LadingItemId,
               ladingItemDetailRowVersion: item.RowVersion,
               ladingChangeRequestId: ladingChangeRequest.Id);
      }
      #endregion

      #region DeleteLadingItemDetailChanges
      foreach (var item in deleteLadingItemDetailChanges)
      {
        InsertToDeleteLadingItemDetailChange(
               ladingItemDetailId: item.Id,
               cargoItemId: item.CargoItemId,
               ladingItemId: item.LadingItemId,
               ladingItemDetailRowVersion: item.RowVersion,
               ladingChangeRequestId: ladingChangeRequest.Id);
      }
      #endregion

      return ladingChangeRequest;
    }
    #endregion

    #region AddLadingChangeRequest
    public LadingChangeRequest AddLadingChangeRequest(
       int ladingId,
       string description,
       LadingType ladingType
       )
    {

      var ladingChangeRequest = repository.Create<LadingChangeRequest>();
      var currentUser = App.Providers.Security.CurrentLoginData;
      ladingChangeRequest.LadingId = ladingId;
      ladingChangeRequest.LadingType = ladingType;
      ladingChangeRequest.Description = description;
      ladingChangeRequest.UserId = currentUser.UserId;
      ladingChangeRequest.RequestDateTime = DateTime.Now.ToUniversalTime();
      ladingChangeRequest.Status = LadingChangeRequestStatus.NotAction;

      repository.Add(ladingChangeRequest);
      return ladingChangeRequest;
    }
    #endregion

    #region Insert To AddLadingItemDetailChange
    public AddLadingItemDetailChange InsertToAddLadingItemDetailChange(
            int cargoItemId,
            int cargoItemDetailId,
            int ladingItemId,
            double qty,
            int ladingChangeRequestId)
    {

      var addLadingItemDetailChange = repository.Create<AddLadingItemDetailChange>();
      addLadingItemDetailChange.CargoItemId = cargoItemId;
      addLadingItemDetailChange.CargoItemDetailId = cargoItemDetailId;
      addLadingItemDetailChange.LadingIemId = ladingItemId;
      addLadingItemDetailChange.Qty = qty;
      addLadingItemDetailChange.LadingChangeRequest = GetLadingChangeRequest(id: ladingChangeRequestId);
      repository.Add(addLadingItemDetailChange);
      return addLadingItemDetailChange;
    }
    #endregion

    #region Insert To EditLadingItemDetailChange
    public EditLadingItemDetailChange InsertToEditLadingItemDetailChange(
            int cargoItemId,
            int cargoItemDetailId,
            int ladingItemId,
            double qty,
            int ladingItemDetailId,
            byte[] ladingItemDetailRowVersion,
            int ladingChangeRequestId)
    {

      var editLadingItemDetailChange = repository.Create<EditLadingItemDetailChange>();
      editLadingItemDetailChange.Qty = qty;
      editLadingItemDetailChange.CargoItemId = cargoItemId;
      editLadingItemDetailChange.LadingIemId = ladingItemId;
      editLadingItemDetailChange.CargoItemDetailId = cargoItemDetailId;
      editLadingItemDetailChange.LadingItemDetailId = ladingItemDetailId;
      editLadingItemDetailChange.LadingItemDetailRowVersion = ladingItemDetailRowVersion;
      editLadingItemDetailChange.LadingChangeRequest = GetLadingChangeRequest(id: ladingChangeRequestId);
      repository.Add(editLadingItemDetailChange);
      return editLadingItemDetailChange;
    }
    #endregion

    #region Insert To DeleteLadingItemDetailChange
    public DeleteLadingItemDetailChange InsertToDeleteLadingItemDetailChange(
            int cargoItemId,
            int ladingItemId,
            int ladingItemDetailId,
            byte[] ladingItemDetailRowVersion,
            int ladingChangeRequestId)
    {

      var deleteLadingItemDetailChange = repository.Create<DeleteLadingItemDetailChange>();
      deleteLadingItemDetailChange.CargoItemId = cargoItemId;
      deleteLadingItemDetailChange.LadingIemId = ladingItemId;
      deleteLadingItemDetailChange.LadingItemDetailId = ladingItemDetailId;
      deleteLadingItemDetailChange.LadingItemDetailRowVersion = ladingItemDetailRowVersion;
      deleteLadingItemDetailChange.LadingChangeRequest = GetLadingChangeRequest(id: ladingChangeRequestId);
      repository.Add(deleteLadingItemDetailChange);
      return deleteLadingItemDetailChange;
    }
    #endregion

    #region Get
    public LadingChangeRequest GetLadingChangeRequest(int id) => GetLadingChangeRequest(selector: e => e, id: id);
    public TResult GetLadingChangeRequest<TResult>(
        Expression<Func<LadingChangeRequest, TResult>> selector,
        int id)
    {

      var ladingChangeRequest = GetLadingChangeRequests(
                    selector: selector,
                    id: id)


                .FirstOrDefault();

      if (ladingChangeRequest == null)
        throw new LadingChangeRequestNotFoundException(id);
      return ladingChangeRequest;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetLadingChangeRequests<TResult>(
      Expression<Func<LadingChangeRequest, TResult>> selector,
      TValue<int> id = null,
      TValue<LadingChangeRequestStatus> status = null,
      TValue<string> ladingCode = null)
    {

      var ladingChangeRequest = repository.GetQuery<LadingChangeRequest>();
      if (id != null)
        ladingChangeRequest = ladingChangeRequest.Where(i => i.Id == id);
      if (ladingCode != null)
        ladingChangeRequest = ladingChangeRequest.Where(i => i.Lading.Code == ladingCode);
      if (status != null)
        ladingChangeRequest = ladingChangeRequest.Where(i => i.Status == status);

      return ladingChangeRequest.Select(selector);
    }
    #endregion

    #region ToAddLadingItemDetailChangesResult
    public Expression<Func<AddLadingItemDetailChange, AddLadingChangeRequestResult>> ToAddLadingItemDetailChangetResult =
        addLadingItemDetailChange => new AddLadingChangeRequestResult
        {
          Id = addLadingItemDetailChange.Id,
          Qty = addLadingItemDetailChange.Qty,
          CargoItemId = addLadingItemDetailChange.CargoItemId,
          LadingItemId = addLadingItemDetailChange.LadingIemId,
          CargoItemDetailId = addLadingItemDetailChange.CargoItemDetailId,
          LadingChangeRequestId = addLadingItemDetailChange.LadingChangeRequestId,
          RowVersion = addLadingItemDetailChange.RowVersion
        };
    #endregion

    #region ToEditLadingItemDetailChangesResult
    public Expression<Func<EditLadingItemDetailChange, EditLadingChangeRequestResult>> ToEditLadingItemDetailChangetResult =
        editLadingItemDetailChange => new EditLadingChangeRequestResult
        {
          Id = editLadingItemDetailChange.Id,
          Qty = editLadingItemDetailChange.Qty,
          CargoItemId = editLadingItemDetailChange.CargoItemId,
          LadingItemId = editLadingItemDetailChange.LadingIemId,
          CargoItemDetailId = editLadingItemDetailChange.CargoItemDetailId,
          LadingItemDetailId = editLadingItemDetailChange.LadingItemDetailId,
          LadingItemDetailRowVersion = editLadingItemDetailChange.LadingItemDetailRowVersion,
          RowVersion = editLadingItemDetailChange.RowVersion,

        };
    #endregion

    #region ToDeleteLadingItemDetailChangesResult
    public Expression<Func<DeleteLadingItemDetailChange, DeleteLadingChangeRequestResult>> ToDeleteLadingItemDetailChangesResult =
        deleteLadingItemDetailChange => new DeleteLadingChangeRequestResult
        {
          Id = deleteLadingItemDetailChange.Id,
          CargoItemId = deleteLadingItemDetailChange.CargoItemId,
          LadingItemId = deleteLadingItemDetailChange.LadingIemId,
          LadingItemDetailId = deleteLadingItemDetailChange.LadingItemDetailId,
          LadingChangeRequestId = deleteLadingItemDetailChange.LadingChangeRequestId,
          LadingItemDetailRowVersion = deleteLadingItemDetailChange.LadingItemDetailRowVersion,
          RowVersion = deleteLadingItemDetailChange.RowVersion
        };
    #endregion

    #region ToResult
    public Expression<Func<LadingChangeRequest, LadingChangeRequestResult>> ToLadingChangeRequestResult =
        ladingChangeRequest => new LadingChangeRequestResult
        {
          Id = ladingChangeRequest.Id,
          Status = ladingChangeRequest.Status,
          DemandantUserId = ladingChangeRequest.User.Id,
          DemandantEmployeeFullName = ladingChangeRequest.User.Employee.FirstName + " " + ladingChangeRequest.User.Employee.LastName,
          ConfirmerUserId = ladingChangeRequest.ConfirmerUser.Id,
          ConfirmerEmployeeFullName = ladingChangeRequest.ConfirmerUser.Employee.FirstName + " " + ladingChangeRequest.ConfirmerUser.Employee.LastName,
          RequestDateTime = ladingChangeRequest.RequestDateTime,
          ConfirmDateTime = ladingChangeRequest.ConfirmDateTime,
          LadingId = ladingChangeRequest.LadingId,
          LadingType = ladingChangeRequest.LadingType,
          LadingCode = ladingChangeRequest.Lading.Code,
          Description = ladingChangeRequest.Description,
          AddLadingItemDetailChanges = ladingChangeRequest.AddLadingItemDetailChanges.AsQueryable().Select(App.Internals.Supplies.ToAddLadingItemDetailChangetResult),
          EditLadingItemDetailChanges = ladingChangeRequest.EditLadingItemDetailChanges.AsQueryable().Select(App.Internals.Supplies.ToEditLadingItemDetailChangetResult),
          DeleteLadingItemDetailChanges = ladingChangeRequest.DeleteLadingItemDetailChanges.AsQueryable().Select(App.Internals.Supplies.ToDeleteLadingItemDetailChangesResult),

          RowVersion = ladingChangeRequest.RowVersion
        };
    #endregion

    #region Search
    public IQueryable<LadingChangeRequestResult> SearchLadingChangeRequestResult(
      IQueryable<LadingChangeRequestResult> query,
      AdvanceSearchItem[] advanceSearchItems,
      string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where
                      item.LadingCode.Contains(searchText)

                select item;

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);


      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<LadingChangeRequestResult> SortLadingChangeRequestResult(IQueryable<LadingChangeRequestResult> query, SearchInput<LadingChangeRequestSortType> sortInput)
    {
      switch (sortInput.SortType)
      {

        case LadingChangeRequestSortType.LadingCode:
          return query.OrderBy(i => i.LadingCode, sortInput.SortOrder);
        case LadingChangeRequestSortType.DemandantEmployeeFullName:
          return query.OrderBy(i => i.DemandantEmployeeFullName, sortInput.SortOrder);
        case LadingChangeRequestSortType.ConfirmerEmployeeFullName:
          return query.OrderBy(i => i.ConfirmerEmployeeFullName, sortInput.SortOrder);
        case LadingChangeRequestSortType.Status:
          return query.OrderBy(i => i.Status, sortInput.SortOrder);
        case LadingChangeRequestSortType.RequestDateTime:
          return query.OrderBy(i => i.RequestDateTime, sortInput.SortOrder);
        case LadingChangeRequestSortType.ConfirmDateTime:
          return query.OrderBy(i => i.ConfirmDateTime, sortInput.SortOrder);
        case LadingChangeRequestSortType.Description:
          return query.OrderBy(i => i.Description, sortInput.SortOrder);


        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region RejectBillOfMaterialPublishRequest
    public LadingChangeRequest RejectLadingChangeRequestProcess(
        int id,
        int ladingId,
        byte[] rowVersion)
    {

      #region Check HasLadingChangeRequest
      var lading = GetLading(id: ladingId);
      if (!lading.HasLadingChangeRequest)
      {
        throw new LadingChangeRequestCanNotEditException(id: lading.Id);
      }
      EditLading(
                ladingId: lading.Id,
                rowVersion: lading.RowVersion,
                hasLadingChangeRequest: false);
      #endregion

      var ladingChangeRequest = GetLadingChangeRequest(id: id);
      if (ladingChangeRequest.Status != LadingChangeRequestStatus.NotAction)
      {
        throw new LadingChangeRequestCanNotEditException(id: ladingChangeRequest.Id);
      }

      ladingChangeRequest = EditLadingChangeRequest(
                    ladingChangeRequest: ladingChangeRequest,
                    rowVersion: rowVersion,
                    status: LadingChangeRequestStatus.Rejected);

      return ladingChangeRequest;
    }
    #endregion

    #region AcceptLadingChangeRequestProcess
    public LadingChangeRequest AcceptLadingChangeRequestProcess(
        int id,
        int ladingId,
        byte[] rowVersion)
    {

      #region Check HasLadingChangeRequest
      var lading = GetLading(id: ladingId);
      if (!lading.HasLadingChangeRequest)
      {
        throw new LadingChangeRequestCanNotEditException(id: lading.Id);
      }
      EditLading(
                ladingId: lading.Id,
                rowVersion: lading.RowVersion,
                hasLadingChangeRequest: false);
      #endregion

      var ladingChangeRequest = GetLadingChangeRequest(id: id);
      if (ladingChangeRequest.Status != LadingChangeRequestStatus.NotAction)
      {
        throw new LadingChangeRequestCanNotEditException(id: ladingChangeRequest.Id);
      }
      ladingChangeRequest = EditLadingChangeRequest(
                ladingChangeRequest: ladingChangeRequest,
                status: LadingChangeRequestStatus.Accepted,
                rowVersion: rowVersion);
      return ladingChangeRequest;
    }
    #endregion

    #region EditLadingChangeRequest
    public LadingChangeRequest EditLadingChangeRequest(
    LadingChangeRequest ladingChangeRequest,
    byte[] rowVersion,
    TValue<LadingChangeRequestStatus> status = null)
    {

      var currentUser = App.Providers.Security.CurrentLoginData;
      ladingChangeRequest.ConfirmerUserId = currentUser.UserId;
      ladingChangeRequest.ConfirmDateTime = DateTime.Now.ToUniversalTime();
      if (status != null)
        ladingChangeRequest.Status = status;

      repository.Update(rowVersion: rowVersion, entity: ladingChangeRequest);
      return ladingChangeRequest;
    }
    #endregion

  }
}
