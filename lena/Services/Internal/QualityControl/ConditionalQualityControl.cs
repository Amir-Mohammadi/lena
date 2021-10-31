using System;
using lena.Models.Common;
using lena.Domains;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Services.Internals.QualityControl.Exception;
using lena.Domains.Enums;
using lena.Models.QualityControl.ConditionalQualityControl;
using lena.Models.QualityControl.ConditionalQualityControlItem;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Common;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region Add
    public ConditionalQualityControl AddConditionalQualityControl(
        ConditionalQualityControl conditionalQualityControl,
        TransactionBatch transactionBatch,
        string description,
        int qualityControlAccepterId,
        int qualityControlConfirmationId)
    {

      conditionalQualityControl = conditionalQualityControl ?? repository.Create<ConditionalQualityControl>();
      conditionalQualityControl.QualityControlAccepterId = qualityControlAccepterId;
      conditionalQualityControl.QualityControlConfirmationId = qualityControlConfirmationId;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: conditionalQualityControl,
                    transactionBatch: transactionBatch,
                    description: description);
      return conditionalQualityControl;
    }
    #endregion
    #region AddProcess
    public ConditionalQualityControl AddConditionalQualityControlProcess(
        TransactionBatch transactionBatch,
        string description,
        int qualityControlAccepterId,
        int qualityControlConfirmationId,
        AddConditionalQualityControlItemInput[] conditionalQualityControlItemInputs)
    {

      #region AddConditionalQualityControl
      var conditionalQualityControl = repository.Create<ConditionalQualityControl>();
      conditionalQualityControl = AddConditionalQualityControl(
                    conditionalQualityControl: conditionalQualityControl,
                    transactionBatch: transactionBatch,
                    description: description,
                    qualityControlAccepterId: qualityControlAccepterId,
                    qualityControlConfirmationId: qualityControlConfirmationId);
      #endregion
      #region Add QualityControlItem Process
      foreach (var inputItem in conditionalQualityControlItemInputs)
      {
        var conditionalQualityControlItem = App.Internals.QualityControl.GetQualityControlConfirmationItem(
                  selector: e => e,
                  id: inputItem.QualityControlConfirmationItemId);

        App.Internals.WarehouseManagement.SetStuffSerialQualityControlDescription(
                      stuffSerial: conditionalQualityControlItem.QualityControlItem.StuffSerial,
                      rowVersion: conditionalQualityControlItem.QualityControlItem.StuffSerial.RowVersion,
                      qualityControlDescription: inputItem.Description);

        AddConditionalQualityControlItemProcess(
                      conditionalQualityControlId: conditionalQualityControl.Id,
                      qualityControlConfirmationItemId: inputItem.QualityControlConfirmationItemId,
                      // See #1149
                      // qty: conditionalQualityControlItem.Qty,
                      // unitId: conditionalQualityControlItem.UnitId,
                      description: conditionalQualityControlItem.Description);
      }
      #endregion
      #region Get QualityControlConfirmation
      var qualityControlConfirmation = GetQualityControlConfirmation(id: qualityControlConfirmationId);
      #endregion
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
              baseEntityId: qualityControlConfirmation.QualityControl.Id,
              scrumTaskType: ScrumTaskTypes.QualityControlConfirmation);
      #endregion
      #region GetUserGroup
      var userGroup = App.Internals.UserManagement.GetUserGroup(
          id: conditionalQualityControl.QualityControlAccepter.UserGroupId);
      #endregion

      #region Add ResponseConditionalQualityControl Task
      //check projectWork not null
      if (projectWorkItem != null)
      {
        #region Get DescriptionForTask
        var conditionalQualityControlId = conditionalQualityControl.QualityControlConfirmation.QualityControl.Id;
        var qualityControl = App.Internals.QualityControl.GetQualityControl(e => new
        {
          StuffName = e.Stuff.Name,

        }, id: conditionalQualityControlId);
        #endregion

        var userIds = App.Internals.UserManagement.GetMemberships(selector: e => e.UserId, userGroupId: userGroup.Id).ToList();
        foreach (var user in userIds)
        {
          App.Internals.ProjectManagement.AddProjectWorkItem(
                   projectWorkItem: null,
                   name: $"تایید کنترل کیفی مشروط {conditionalQualityControl.Code} ",
                   description: $"عنوان کالا:{qualityControl.StuffName}",
                   color: "",
                   departmentId: (Int32)(Departments.QualityControl),
                   estimatedTime: 10800,
                   isCommit: false,
                   scrumTaskTypeId: (int)ScrumTaskTypes.ResponseConditionalQualityControl,
                   userId: user,
                   spentTime: 0,
                   remainedTime: 0,
                   scrumTaskStep: ScrumTaskStep.ToDo,
                   projectWorkId: projectWorkItem.ScrumBackLogId,
                   baseEntityId: conditionalQualityControl.Id);
        }

      }
      #endregion
      #region ResetQualityControlSummaryByQualityControlId
      ResetQualityControlSummaryByQualityControlId(qualityControlId: qualityControlConfirmation.QualityControl.Id);
      #endregion
      return conditionalQualityControl;
    }
    #endregion
    #region Edit
    public ConditionalQualityControl EditConditionalQualityControl(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<int> qualityControlAccepterId = null,
        TValue<int> qualityControlConfirmationId = null,
        TValue<int> responseConditionalConfirmationUserId = null,
        TValue<DateTime> responseConditionalConfirmationDate = null,
        TValue<ConditionalQualityControlStatus> conditionalQualityControlStatus = null
        )
    {

      var conditionalQualityControl = GetConditionalQualityControl(id: id);
      return EditConditionalQualityControl(
                    conditionalQualityControl: conditionalQualityControl,
                    rowVersion: rowVersion,
                    description: description,
                    qualityControlAccepterId: qualityControlAccepterId,
                    qualityControlConfirmationId: qualityControlConfirmationId,
                    responseConditionalConfirmationUserId: responseConditionalConfirmationUserId,
                    responseConditionalConfirmationDate: responseConditionalConfirmationDate,
                    conditionalQualityControlStatus: conditionalQualityControlStatus
                    );
    }
    public ConditionalQualityControl EditConditionalQualityControl(
        ConditionalQualityControl conditionalQualityControl,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<int> qualityControlAccepterId = null,
        TValue<int> qualityControlConfirmationId = null,
        TValue<int> responseConditionalConfirmationUserId = null,
        TValue<DateTime> responseConditionalConfirmationDate = null,
        TValue<ConditionalQualityControlStatus> conditionalQualityControlStatus = null
        )
    {

      if (qualityControlAccepterId != null)
        conditionalQualityControl.QualityControlAccepterId = qualityControlAccepterId;
      if (qualityControlConfirmationId != null)
        conditionalQualityControl.QualityControlConfirmationId = qualityControlConfirmationId;
      if (conditionalQualityControlStatus != null)
        conditionalQualityControl.Status = conditionalQualityControlStatus;
      if (responseConditionalConfirmationUserId != null)
        conditionalQualityControl.ResponseConditionalConfirmationUserId = responseConditionalConfirmationUserId;
      if (responseConditionalConfirmationDate != null)
        conditionalQualityControl.ResponseConditionalConfirmationDate = responseConditionalConfirmationDate;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: conditionalQualityControl,
                    description: description,
                    rowVersion: rowVersion);
      return retValue as ConditionalQualityControl;
    }
    #endregion
    #region Get
    public ConditionalQualityControl GetConditionalQualityControl(int id) => GetConditionalQualityControl(selector: e => e, id: id);
    public TResult GetConditionalQualityControl<TResult>(
        Expression<Func<ConditionalQualityControl, TResult>> selector,
        int id)
    {

      var conditionalQualityControl = GetConditionalQualityControls(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (conditionalQualityControl == null)
        throw new ConditionalQualityControlNotFoundException(id);
      return conditionalQualityControl;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetConditionalQualityControls<TResult>(
        Expression<Func<ConditionalQualityControl, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> qualityControlId = null,
        TValue<int> qualityControlAccepterId = null,
        TValue<int> qualityControlConfirmationId = null,
        TValue<QualityControlType> qualityControlType = null,
        TValue<int> responseConditionalConfirmationUserId = null,
        TValue<int> qualityControlAccepterUserGroupId = null,
        TValue<ConditionalQualityControlStatus> status = null,
        TValue<ConditionalQualityControlStatus[]> statuses = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null,
        TValue<bool> showMyRelativeRequests = null,
        TValue<int> stuffId = null,
        TValue<string> serial = null,
        TValue<int> warehouseId = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<ConditionalQualityControl>();
      if (qualityControlId != null)
        query = query.Where(i => i.QualityControlConfirmation.QualityControl.Id == qualityControlId);
      if (qualityControlAccepterId != null)
        query = query.Where(i => i.QualityControlAccepterId == qualityControlAccepterId);
      if (qualityControlConfirmationId != null)
        query = query.Where(i => i.QualityControlConfirmationId == qualityControlConfirmationId);
      if (qualityControlType != null)
        query = query.Where(i => i.QualityControlConfirmation.QualityControl.QualityControlType == qualityControlType);
      if (stuffId != null)
        query = query.Where(i => i.QualityControlConfirmation.QualityControl.StuffId == stuffId);
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial);
        query = query.Where(i => i.ConditionalQualityControlItems.Any(j => j.QualityControlConfirmationItem.QualityControlItem.StuffSerial.Serial == serial));
      }
      if (warehouseId != null)
        query = query.Where(i => i.QualityControlConfirmation.QualityControl.WarehouseId == warehouseId);
      if (fromDate != null)
        query = query.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        query = query.Where(i => i.DateTime <= toDate);
      if (responseConditionalConfirmationUserId != null)
        query = query.Where(i => i.ResponseConditionalConfirmationUserId == responseConditionalConfirmationUserId);
      if (qualityControlAccepterUserGroupId != null)
        query = query.Where(i => i.QualityControlAccepter.UserGroupId == qualityControlAccepterUserGroupId);

      if (status != null)
        query = query.Where(i => i.Status == status);

      if (statuses != null)
        query = query.Where(i => statuses.Value.Contains(i.Status));

      if (showMyRelativeRequests == true)
      {
        var currentUserId = App.Providers.Security.CurrentLoginData.UserId;
        var userGroupIds = App.Internals.UserManagement.GetMemberships(selector: e => e.UserGroupId, userId: currentUserId).ToList();
        query = query.Where(i => userGroupIds.Contains(i.QualityControlAccepter.UserGroupId));
      }

      return query.Select(selector);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ConditionalQualityControlResult> SortConditionalQualityControlResult(
        IQueryable<ConditionalQualityControlResult> query,
        SortInput<ConditionalQualityControlSortType> sort)
    {
      switch (sort.SortType)
      {
        case ConditionalQualityControlSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ConditionalQualityControlSortType.QualityControlCode:
          return query.OrderBy(a => a.QualityControlCode, sort.SortOrder);
        case ConditionalQualityControlSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case ConditionalQualityControlSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case ConditionalQualityControlSortType.QualityControlType:
          return query.OrderBy(a => a.QualityControlType, sort.SortOrder);
        case ConditionalQualityControlSortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        case ConditionalQualityControlSortType.QualityControlStatus:
          return query.OrderBy(a => a.QualityControlStatus, sort.SortOrder);
        case ConditionalQualityControlSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);

        case ConditionalQualityControlSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case ConditionalQualityControlSortType.ConfirmationDateTime:
          return query.OrderBy(a => a.ConfirmationDateTime, sort.SortOrder);
        case ConditionalQualityControlSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case ConditionalQualityControlSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case ConditionalQualityControlSortType.AcceptedQty:
          return query.OrderBy(a => a.AcceptedQty, sort.SortOrder);
        case ConditionalQualityControlSortType.FailedQty:
          return query.OrderBy(a => a.FailedQty, sort.SortOrder);
        case ConditionalQualityControlSortType.ConditionalQty:
          return query.OrderBy(a => a.ConditionalQty, sort.SortOrder);
        case ConditionalQualityControlSortType.ConditionalRequestQty:
          return query.OrderBy(a => a.ConditionalRequestQty, sort.SortOrder);

        case ConditionalQualityControlSortType.QualityControlDescription:
          return query.OrderBy(a => a.QualityControlDescription, sort.SortOrder);
        case ConditionalQualityControlSortType.QualityControlConfirmationDescription:
          return query.OrderBy(a => a.QualityControlConfirmationDescription, sort.SortOrder);

        case ConditionalQualityControlSortType.ConditionalQualityControlConfirmationDescription:
          return query.OrderBy(a => a.ConditionalQualityControlConfirmationDescription, sort.SortOrder);
        case ConditionalQualityControlSortType.ConditionalQualityControlRequestDescription:
          return query.OrderBy(a => a.ConditionalQualityControlRequestDescription, sort.SortOrder);
        case ConditionalQualityControlSortType.ResponseConditionalConfirmationUserName:
          return query.OrderBy(a => a.ResponseConditionalConfirmationUserName, sort.SortOrder);
        case ConditionalQualityControlSortType.ResponseConditionalConfirmationDate:
          return query.OrderBy(a => a.ResponseConditionalConfirmationDate, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<ConditionalQualityControlResult> SearchConditionalQualityControlResult(
        IQueryable<ConditionalQualityControlResult> query,
        string search,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.Code.Contains(search) ||
                    item.QualityControlCode.Contains(search) ||
                    item.StuffCode.Contains(search) ||
                    item.StuffName.Contains(search) ||
                    item.WarehouseName.Contains(search) ||
                    item.UnitName.Contains(search) ||
                    item.QualityControlDescription.Contains(search) ||
                    item.QualityControlConfirmationDescription.Contains(search) ||
                     item.ConditionalQualityControlRequestDescription.Contains(search) ||
                    item.ConditionalQualityControlConfirmationDescription.Contains(search)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<ConditionalQualityControl, ConditionalQualityControlResult>> ToConditionalQualityControlResult =
        conditionalQualityControl => new ConditionalQualityControlResult
        {
          Id = conditionalQualityControl.Id,
          Code = conditionalQualityControl.Code,
          QualityControlAccepterTitle = conditionalQualityControl.QualityControlAccepter.Title,
          QualityControlAccepterUserGroupId = conditionalQualityControl.QualityControlAccepter.UserGroup.Id,
          QualityControlAccepterGroupName = conditionalQualityControl.QualityControlAccepter.UserGroup.Name,
          QualityControlConfirmationId = conditionalQualityControl.QualityControlConfirmationId,
          QualityControlConfirmationCode = conditionalQualityControl.QualityControlConfirmation.Code,
          QualityControlId = conditionalQualityControl.QualityControlConfirmation.QualityControl.Id,
          QualityControlCode = conditionalQualityControl.QualityControlConfirmation.QualityControl.Code,
          StuffId = conditionalQualityControl.QualityControlConfirmation.QualityControl.StuffId,
          StuffCode = conditionalQualityControl.QualityControlConfirmation.QualityControl.Stuff.Code,
          StuffName = conditionalQualityControl.QualityControlConfirmation.QualityControl.Stuff.Name,
          QualityControlType = conditionalQualityControl.QualityControlConfirmation.QualityControl.QualityControlType,
          WarehouseId = conditionalQualityControl.QualityControlConfirmation.QualityControl.WarehouseId,
          WarehouseName = conditionalQualityControl.QualityControlConfirmation.QualityControl.Warehouse.Name,
          QualityControlStatus = conditionalQualityControl.QualityControlConfirmation.Status,
          Status = conditionalQualityControl.Status,
          Qty = conditionalQualityControl.QualityControlConfirmation.QualityControl.Qty,
          UnitId = conditionalQualityControl.QualityControlConfirmation.QualityControl.UnitId,
          UnitName = conditionalQualityControl.QualityControlConfirmation.QualityControl.Unit.Name,
          AcceptedQty = (double?)conditionalQualityControl.QualityControlConfirmation.QualityControl.QualityControlSummary.AcceptedQty,
          FailedQty = (double?)conditionalQualityControl.QualityControlConfirmation.QualityControl.QualityControlSummary.FailedQty,
          ConditionalRequestQty = (double?)conditionalQualityControl.QualityControlConfirmation.QualityControl.QualityControlSummary.ConditionalRequestQty,
          ConditionalQty = (double?)conditionalQualityControl.QualityControlConfirmation.QualityControl.QualityControlSummary.ConditionalQty,
          ReturnedQty = (double?)conditionalQualityControl.QualityControlConfirmation.QualityControl.QualityControlSummary.ReturnedQty,
          ConsumedQty = (double?)conditionalQualityControl.QualityControlConfirmation.QualityControl.QualityControlSummary.ConsumedQty,
          DateTime = conditionalQualityControl.DateTime,
          ConfirmationDateTime = conditionalQualityControl.QualityControlConfirmation.DateTime,
          QualityControlConfirmationDescription = conditionalQualityControl.QualityControlConfirmation.Description,
          QualityControlDescription = conditionalQualityControl.QualityControlConfirmation.QualityControl.Description,
          ConditionalQualityControlConfirmationDescription = conditionalQualityControl.ResponseConditionalQualityControl.Description,
          ConditionalQualityControlRequestDescription = conditionalQualityControl.Description,
          ResponseConditionalConfirmationUserId = conditionalQualityControl.ResponseConditionalConfirmationUserId,
          ResponseConditionalConfirmationUserName = conditionalQualityControl.ResponseConditionalConfirmationlUser.Employee.FirstName + " " + conditionalQualityControl.ResponseConditionalConfirmationlUser.Employee.LastName,
          ResponseConditionalConfirmationDate = conditionalQualityControl.ResponseConditionalConfirmationDate,
          RowVersion = conditionalQualityControl.RowVersion
        };
    #endregion
  }
}
