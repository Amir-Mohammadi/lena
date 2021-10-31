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
using lena.Models.WarehouseManagement.StuffRequestItem;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add

    public StuffRequestItem AddStuffRequestItem(
        StuffRequestItem stuffRequestItem,
        TransactionBatch transactionBatch,
        int stuffRequestId,
        int? stuffId,
        short? billOfMaterialVersion,
        double qty,
        byte unitId,
        string description)
    {

      stuffRequestItem = stuffRequestItem ?? repository.Create<StuffRequestItem>();
      stuffRequestItem.StuffId = stuffId;
      stuffRequestItem.UnitId = unitId;
      stuffRequestItem.Qty = qty;
      stuffRequestItem.StuffRequestId = stuffRequestId;
      stuffRequestItem.Description = description;
      stuffRequestItem.BillOfMaterialVersion = (billOfMaterialVersion == 0 ? null : billOfMaterialVersion);
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: stuffRequestItem,
                    transactionBatch: transactionBatch,
                    description: description);
      return stuffRequestItem;
    }

    #endregion

    #region AddProcess
    public StuffRequestItem AddStuffRequestItemProcess(
        StuffRequestItem stuffRequestItem,
        TransactionBatch transactionBatch,
        int stuffRequestId,
        int? stuffId,
        short? billOfMaterialVersion,
        double qty,
        byte unitId,
        string description)
    {

      #region AddTransactionBatch
      transactionBatch = transactionBatch ?? AddTransactionBatch();
      #endregion
      #region AddStuffRequestItem

      stuffRequestItem = AddStuffRequestItem(
              stuffRequestItem: stuffRequestItem,
              transactionBatch: transactionBatch,
              stuffRequestId: stuffRequestId,
              stuffId: stuffId,
              billOfMaterialVersion: billOfMaterialVersion,
              qty: qty,
              unitId: unitId,
              description: description);

      #endregion
      return stuffRequestItem;
    }

    #endregion

    #region Edit

    public StuffRequestItem EditStuffRequestItem(
        int id,
        byte[] rowVersion,
        TValue<int> stuffRequestId = null,
        TValue<int?> stuffId = null,
        TValue<short?> billOfMaterialVersion = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<double> responsedQty = null,
        TValue<StuffRequestItemStatusType> status = null,
        TValue<string> description = null)
    {

      var stuffRequestItem = GetStuffRequestItem(id: id);
      return EditStuffRequestItem(
                    stuffRequestItem: stuffRequestItem,
                    rowVersion: rowVersion,
                    stuffRequestId: stuffRequestId,
                    stuffId: stuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    qty: qty,
                    unitId: unitId,
                    responsedQty: responsedQty,
                    status: status,
                    description: description);
    }

    public StuffRequestItem EditStuffRequestItem(
        StuffRequestItem stuffRequestItem,
        byte[] rowVersion,
        TValue<int> stuffRequestId = null,
        TValue<int?> stuffId = null,
        TValue<short?> billOfMaterialVersion = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<double> responsedQty = null,
        TValue<StuffRequestItemStatusType> status = null,
        TValue<string> description = null)
    {

      if (stuffRequestId != null)
        stuffRequestItem.StuffRequestId = stuffRequestId;
      if (stuffId != null)
        stuffRequestItem.StuffId = stuffId;
      if (billOfMaterialVersion != null)
        stuffRequestItem.BillOfMaterialVersion = billOfMaterialVersion;
      if (unitId != null)
        stuffRequestItem.UnitId = unitId;
      if (qty != null)
        stuffRequestItem.Qty = qty;
      if (status != null)
        stuffRequestItem.Status = status;
      if (responsedQty != null)
        stuffRequestItem.ResponsedQty = responsedQty;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: stuffRequestItem,
                    rowVersion: rowVersion,
                    description: description);
      return retValue as StuffRequestItem;
    }

    #endregion

    #region RemoveProcess

    public StuffRequestItem RemoveStuffRequestItemProcess(
        int? transactionBatchId,
        int id,
        byte[] rowVersion)
    {

      var stuffRequestItem = GetStuffRequestItem(id: id);
      var baseEntity = App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    transactionBatchId: transactionBatchId,
                    baseEntity: stuffRequestItem,
                    rowVersion: rowVersion);
      return baseEntity as StuffRequestItem;
    }

    #endregion

    #region Get

    public StuffRequestItem GetStuffRequestItem(int id) => GetStuffRequestItem(selector: e => e, id: id);

    public TResult GetStuffRequestItem<TResult>(
        Expression<Func<StuffRequestItem, TResult>> selector,
        int id)
    {

      var result = GetStuffRequestItems(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (result == null)
        throw new StuffRequestItemNotFoundException(id);
      return result;
    }

    public IQueryable<TResult> GetStuffRequestItems<TResult>(
        Expression<Func<StuffRequestItem, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> stuffRequestId = null,
        TValue<int> stuffId = null,
        TValue<int> unitId = null,
        TValue<double> qty = null,
        TValue<StuffRequestItemStatusType> status = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    ids: ids,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<StuffRequestItem>();
      if (stuffId != null)
        query = query.Where(r => r.StuffId == stuffId);
      if (stuffRequestId != null)
        query = query.Where(r => r.StuffRequestId == stuffRequestId);
      if (unitId != null)
        query = query.Where(r => r.UnitId == unitId);
      if (qty != null)
        query = query.Where(r => r.Qty == qty);
      if (status != null)
        query = query.Where(i => i.Status == status);
      return query.Select(selector);
    }

    #endregion

    #region Search

    public IQueryable<StuffRequestItemResult> SearchStuffRequestItemResult(IQueryable<StuffRequestItemResult> query,
        string search,
        int? customerId,
        int? orderItemId,
        int? stuffId,
        int? orderItemBlockId)
    {
      //if (!string.IsNullOrEmpty(search))
      //    query = query.Where(item =>
      //        item.StuffCode.Contains(search) ||
      //        item.StuffName.Contains(search));
      //if (customerId != null)
      //    query = query.Where(i => i.CustomerId == customerId);
      //if (orderItemId != null)
      //    query = query.Where(i => i.OrderItemId == orderItemId);
      //if (stuffId != null)
      //    query = query.Where(i => i.StuffId == stuffId);
      //if (orderItemBlockId != null)
      //    query = query.Where(i => i.OrderItemBlockId == orderItemBlockId);
      return query;
    }

    #endregion

    #region Sort

    public IOrderedQueryable<StuffRequestItemResult> SortSendProductResult(IQueryable<StuffRequestItemResult> query,
        SortInput<StuffRequestItemSortType> sort)
    {
      switch (sort.SortType)
      {
        case StuffRequestItemSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        //case StuffRequestItemSortType.StuffCode:
        //    return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        //case StuffRequestItemSortType.StuffName:
        //    return query.OrderBy(a => a.StuffName, sort.SortOrder);
        //case StuffRequestItemSortType.OrderItemCode:
        //    return query.OrderBy(a => a.OrderItemCode, sort.SortOrder);
        //case StuffRequestItemSortType.SendPermissionCode:
        //    return query.OrderBy(a => a.SendPermissionCode, sort.SortOrder);
        //case StuffRequestItemSortType.Qty:
        //    return query.OrderBy(a => a.Qty, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    #endregion


    #region ToStuffRequestItemResult

    //public IQueryable<StuffRequestItemResult> GetStuffRequestItemResult(IQueryable<StuffRequestItem> requestItem)
    //{
    //    
    //        var internals = App.Internals;
    //        var stuffQtyQuery = internals.WarehouseManagement.GetWarehouseInventoriesQueryable();


    //        var query = (from item in requestItem

    //                     join inventoryQuery in stuffQtyQuery on
    //                     new { WarehouseId = item.FromWarehouseId, StuffId = requestDetail.StuffId }
    //                     equals new
    //                     { WarehouseId = inventoryQuery.WarehouseId, StuffId = (int?)inventoryQuery.StuffId }

    //                     //join stuff in stuffs on q.StuffId equals stuff.Id
    //                     where requestDetail.StuffId != null
    //                     group inventoryQuery by new { requestDetail.StuffId, item.FromWarehouseId } into g


    //                     select new StuffRequestItemResult()
    //                     {
    //                         // StuffId = g.Key.StuffId,
    //                         //WarehoseId = g.Key.FromWarehouseId,
    //                         //StuffRequestItems = 
    //                         //count = g.Sum(a => a.AvailableAmount) 
    //                     }
    //                    ).ToList();

    //        return null;
    //    });
    //}
    //todo fix

    public Expression<Func<StuffRequestItem, StuffRequestItemResult>> ToStuffRequestItemResult =
        stuffRequestItem => new StuffRequestItemResult
        {
          Id = stuffRequestItem.Id,
          Code = stuffRequestItem.Code,
          StuffId = stuffRequestItem.StuffId,
          StuffType = stuffRequestItem.Stuff.StuffType,
          BillOfMaterialVersion = stuffRequestItem.BillOfMaterialVersion ?? stuffRequestItem.ResponseStuffRequestItems.OrderByDescending(r => r.Id).FirstOrDefault().BillOfMaterialVersion,
          BillOfMaterialTitle = stuffRequestItem.BillOfMaterialVersion != null ? stuffRequestItem.BillOfMaterial.Title : stuffRequestItem.ResponseStuffRequestItems.OrderByDescending(r => r.Id).FirstOrDefault().BillOfMaterial.Title,
          StuffCode = stuffRequestItem.Stuff.Code,
          StuffName = stuffRequestItem.Stuff.Name,
          Qty = stuffRequestItem.Qty,
          AvailableAmount = 0,
          UnitId = stuffRequestItem.UnitId,
          UnitName = stuffRequestItem.Unit.Name,
          ResponsedQty = stuffRequestItem.ResponsedQty,
          Status = stuffRequestItem.Status,
          Description = stuffRequestItem.Description,
          IsDelete = stuffRequestItem.IsDelete,
          StuffRequestId = stuffRequestItem.StuffRequestId,
          RowVersion = stuffRequestItem.RowVersion
        };

    #endregion

    #region ResponseStuffRequestItem

    public StuffRequestItem ResponseStuffRequestItem(
        int id,
        int stuffId,
        short? billOfMaterialVersion,
        byte[] rowVersion,
        double qty,
        StuffRequestItemStatusType status,
        string description,
        int fromWarehouseId,
        int? toWarehouseId)
    {

      var stuffRequestItem = GetStuffRequestItem(id: id);
      return ResponseStuffRequestItem(
                    stuffRequestItem: stuffRequestItem,
                    stuffId: stuffId,
                    rowVersion: rowVersion,
                    qty: qty,
                    status: status,
                    billOfMaterialVersion: billOfMaterialVersion,
                    description: description);
    }
    public StuffRequestItem ResponseStuffRequestItem(
        StuffRequestItem stuffRequestItem,
        int? stuffId,
        short? billOfMaterialVersion,
        byte[] rowVersion,
        double qty,
        StuffRequestItemStatusType status,
        string description)
    {

      #region AddTransactionBatch
      var transactionBatch = AddTransactionBatch();
      #endregion

      #region Remove ActiveResponse if exist

      var activeResponseStuffRequestItems = GetResponseStuffRequestItems(
              selector: e => e,
              stuffRequestItemId: stuffRequestItem.Id,
              isDelete: false);
      TransactionBatch removeTransactionBatch = null;
      #region AddTransactionBatch
      if (activeResponseStuffRequestItems.Any())
        removeTransactionBatch = AddTransactionBatch();
      #endregion
      foreach (var activeResponseStuffRequestItem in activeResponseStuffRequestItems)
      {
        RemoveResponseStuffRequestItemProcess(
                  transactionBatchId: removeTransactionBatch.Id,
                  id: activeResponseStuffRequestItem.Id,
                  rowVersion: activeResponseStuffRequestItem.RowVersion);
      }

      #endregion

      #region Add ResponseStuffRequestItem
      AddResponseStuffRequestItemProcess(
              responseStuffRequestItem: null,
              transactionBatch: transactionBatch,
              stuffRequestItemId: stuffRequestItem.Id,
              stuffId: stuffId,
              qty: qty,
              status: status,
              billOfMaterialVersion: billOfMaterialVersion,
              description: description);
      #endregion

      #region Change StuffRequestItemStatus

      var retValue = EditStuffRequestItem(
              id: stuffRequestItem.Id,
              rowVersion: rowVersion,
              stuffId: stuffId,
              responsedQty: qty,
              status: status);
      #endregion

      #region  CheckStuffRequestItemTask
      CheckConfirmStuffRequest(stuffRequestId: stuffRequestItem.StuffRequestId);
      #endregion


      return stuffRequestItem;
    }

    #endregion

    #region AcceptStuffRequestItem

    public StuffRequestItem AcceptStuffRequestItem(
        int id,
        int stuffId,
        byte[] rowVersion,
        double qty,
        string description,
        short? billOfMaterialVersion,
        int fromWarehouseId,
        int? toWarehouseId)
    {

      var stuffRequestItem = GetStuffRequestItem(id: id);
      return AcceptStuffRequestItem(
                    stuffRequestItem: stuffRequestItem,
                    billOfMaterialVersion: billOfMaterialVersion,
                    stuffId: stuffId,
                    rowVersion: rowVersion,
                    qty: qty,
                    description: description,
                    fromWarehouseId: fromWarehouseId,
                    toWarehouseId: toWarehouseId);
    }

    public StuffRequestItem AcceptStuffRequestItem(
        StuffRequestItem stuffRequestItem,
        int stuffId,
        short? billOfMaterialVersion,
        byte[] rowVersion,
        double qty,
        string description,
        int fromWarehouseId,
        int? toWarehouseId)
    {

      if (stuffRequestItem.StuffRequest.FromWarehouseId != fromWarehouseId ||
                stuffRequestItem.StuffRequest.ToWarehouseId != toWarehouseId)
      {
        throw new ResponseStuffRequestItemWarehouseNotMatchException(stuffRequestItem.Id);
      }

      ResponseStuffRequestItem(
                    stuffRequestItem: stuffRequestItem,
                    stuffId: stuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    rowVersion: rowVersion,
                    qty: qty,
                    status: StuffRequestItemStatusType.Blocked,
                    description: description);
      return stuffRequestItem;
    }

    #endregion

    #region RejectStuffRequestItem

    public StuffRequestItem RejectStuffRequestItem(
        int id,
        byte[] rowVersion,
        string description,
        int fromWarehouseId,
        int? toWarehouseId)
    {

      var stuffRequestItem = GetStuffRequestItem(id: id);
      return RejectStuffRequestItem(
                    stuffRequestItem: stuffRequestItem,
                    rowVersion: rowVersion,
                    description: description,
                    fromWarehouseId: fromWarehouseId,
                    toWarehouseId: toWarehouseId);
    }

    public StuffRequestItem RejectStuffRequestItem(
        StuffRequestItem stuffRequestItem,
        byte[] rowVersion,
        string description,
        int fromWarehouseId,
        int? toWarehouseId)
    {

      if (stuffRequestItem.StuffRequest.FromWarehouseId != fromWarehouseId || stuffRequestItem.StuffRequest.ToWarehouseId != toWarehouseId)
      {
        throw new ResponseStuffRequestItemWarehouseNotMatchException(stuffRequestItem.Id);
      }

      ResponseStuffRequestItem(
                    stuffId: null,
                    billOfMaterialVersion: null,
                    stuffRequestItem: stuffRequestItem,
                    rowVersion: rowVersion,
                    qty: 0,
                    status: StuffRequestItemStatusType.Rejected,
                    description: description);
      return stuffRequestItem;
    }

    #endregion

    #region RemoveStuffRequestItemResponse

    public StuffRequestItem RemoveStuffRequestItemResponse(
        int id,
        byte[] rowVersion,
        string description,
        int fromWarehouseId,
        int? toWarehouseId)
    {

      var stuffRequestItem = GetStuffRequestItem(id: id);
      return RemoveStuffRequestItemResponse(

                    stuffRequestItem: stuffRequestItem,
                    rowVersion: rowVersion,
                    description: description,
                    fromWarehouseId: fromWarehouseId,
                    toWarehouseId: toWarehouseId);
    }

    public StuffRequestItem RemoveStuffRequestItemResponse(
        StuffRequestItem stuffRequestItem,
        byte[] rowVersion,
        string description,
        int fromWarehouseId,
        int? toWarehouseId)
    {


      if (stuffRequestItem.StuffRequest.FromWarehouseId != fromWarehouseId || stuffRequestItem.StuffRequest.ToWarehouseId != toWarehouseId)
      {
        throw new ResponseStuffRequestItemWarehouseNotMatchException(stuffRequestItem.Id);
      }

      ResponseStuffRequestItem(
                        stuffId: null,
                        billOfMaterialVersion: null,
                        stuffRequestItem: stuffRequestItem,
                        rowVersion: rowVersion,
                        qty: 0,
                        status: StuffRequestItemStatusType.Requested,
                        description: description);
      return stuffRequestItem;
    }

    #endregion

    #region CheckTask
    public void CheckConfirmStuffRequest(int stuffRequestId)
    {

      var stuffRequest = GetStuffRequest(id: stuffRequestId); ; CheckConfirmStuffRequest(stuffRequest: stuffRequest);
    }
    public void CheckConfirmStuffRequest(StuffRequest stuffRequest)
    {

      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
              baseEntityId: stuffRequest.Id,
              scrumTaskType: ScrumTaskTypes.CheckStuffRequest);
      if (projectWorkItem == null)
      {
        projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                  baseEntityId: stuffRequest.Id,
                  scrumTaskType: ScrumTaskTypes.CheckStuffRequest);
      }
      #endregion
      #region check CheckStuffRequests and DoneProjectWorkItem
      var stuffRequestItems = GetStuffRequestItems(
              selector: e => e,
              stuffRequestId: stuffRequest.Id,
              status: StuffRequestItemStatusType.Requested,
              isDelete: false);
      var taskIsDone = !stuffRequestItems.Any();
      if (taskIsDone && projectWorkItem != null && projectWorkItem.ScrumTaskStep != ScrumTaskStep.Done)
      {
        #region DoneTask

        App.Internals.ScrumManagement.DoneScrumTask(
                scrumTask: projectWorkItem,
                rowVersion: projectWorkItem.RowVersion);

        #endregion
      }
      else if (taskIsDone == false && projectWorkItem != null && projectWorkItem.ScrumTaskStep == ScrumTaskStep.Done)
      {
        #region  AddNewTask
        #region Get DescriptionForTask
        var fromWarehouseId = stuffRequest.FromWarehouseId;
        var fromWarehouse = App.Internals.WarehouseManagement.GetWarehouse(e => new
        {

          FromWarehouseName = e.Name,

        }, id: fromWarehouseId);

        var requestTo = "";
        if (stuffRequest.ToWarehouseId != null)
        {
          var toWarehouse = App.Internals.WarehouseManagement.GetWarehouse(e => new
          {
            ToWarehouseName = e.Name,

          }, id: stuffRequest.ToWarehouseId.Value);
          requestTo = toWarehouse.ToWarehouseName;

        }
        else
        {
          requestTo = stuffRequest.ToDepartment.Name;
        }

        #endregion
        var result = App.Internals.ProjectManagement.AddProjectWorkItem(
                projectWorkItem: null,
                name: $"بررسی درخواست کالا {stuffRequest.Code}",
                description: $"از انبار:{fromWarehouse.FromWarehouseName},  به :{requestTo} ",
                color: "",
                departmentId: (int)Departments.Warehouse,
                estimatedTime: 10800,
                isCommit: false,
                scrumTaskTypeId: (int)ScrumTaskTypes.CheckStuffRequest,
                userId: null,
                spentTime: 0,
                remainedTime: 0,
                scrumTaskStep: ScrumTaskStep.ToDo,
                projectWorkId: projectWorkItem.ScrumBackLogId,
                baseEntityId: stuffRequest.Id);
        #endregion
      }
      #region Check And Add WarehouseIssutTask
      #region GetWrehouseIssueTask
      var warehouseTask = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
          baseEntityId: stuffRequest.Id,
          scrumTaskType: ScrumTaskTypes.WarehouseIssue);
      #endregion

      var confirmedStuffRequestItems = GetStuffRequestItems(
              selector: e => e,
              stuffRequestId: stuffRequest.Id,
              status: StuffRequestItemStatusType.Blocked,
              isDelete: false);
      var addWarehouseTask = !stuffRequestItems.Any();

      if (addWarehouseTask && warehouseTask == null)
      {
        #region AddWarehouseIssueTask
        //check projectWork not null
        if (projectWorkItem != null)
        {
          #region Get DescriptionForTask
          var fromWarehouseId = stuffRequest.FromWarehouseId;
          var fromWarehouse = App.Internals.WarehouseManagement.GetWarehouse(e => new
          {
            FromWarehouseName = e.Name,

          }, id: fromWarehouseId);


          var requestTo = "";
          if (stuffRequest.ToWarehouseId != null)
          {
            var toWarehouse = App.Internals.WarehouseManagement.GetWarehouse(e => new
            {
              ToWarehouseName = e.Name,

            }, id: stuffRequest.ToWarehouseId.Value);
            requestTo = toWarehouse.ToWarehouseName;

          }
          else
          {
            requestTo = stuffRequest.ToDepartment.Name;
          }

          #endregion
          App.Internals.ProjectManagement.AddProjectWorkItem(
                  projectWorkItem: null,
                  name: $"ثبت حواله کالا {stuffRequest.Code}",
                  description: $"از انبار:{fromWarehouse.FromWarehouseName},  به :{requestTo} ",
                  color: "",
                  departmentId: (int)Departments.Warehouse,
                  estimatedTime: 10800,
                  isCommit: false,
                  scrumTaskTypeId: (int)ScrumTaskTypes.WarehouseIssue,
                  userId: null,
                  spentTime: 0,
                  remainedTime: 0,
                  scrumTaskStep: ScrumTaskStep.ToDo,
                  projectWorkId: projectWorkItem.ScrumBackLogId,
                  baseEntityId: stuffRequest.Id);
        }

        #endregion
      }
      if (addWarehouseTask == false && warehouseTask != null)
      {
        #region DeleteWarehouseIssueTask
        App.Internals.ScrumManagement.DeleteScrumTask(id: warehouseTask.Id);
        #endregion
      }
      #endregion

      #endregion
    }
    #endregion

    #region WarehouseIssueTask
    public void CheckStuffRequestWarehouseIssueTask(int stuffRequestId)
    {

      var stuffRequest = GetStuffRequest(id: stuffRequestId); ; CheckStuffRequestWarehouseIssueTask(stuffRequest: stuffRequest);
    }
    public void CheckStuffRequestWarehouseIssueTask(StuffRequest stuffRequest)
    {

      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
              baseEntityId: stuffRequest.Id,
              scrumTaskType: ScrumTaskTypes.WarehouseIssue);
      if (projectWorkItem == null)
      {
        projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                  baseEntityId: stuffRequest.Id,
                  scrumTaskType: ScrumTaskTypes.WarehouseIssue);
      }
      #endregion
      #region check StuffRequestItems and DoneProjectWorkItem

      var stuffRequestItems = GetStuffRequestItems(
              selector: e => e,
              stuffRequestId: stuffRequest.Id,
              isDelete: false);


      var taskIsDone = stuffRequestItems.All(i => i.Status == StuffRequestItemStatusType.Complated ||
                                                        i.Status == StuffRequestItemStatusType.RejectedIssue ||
                                                        i.Status == StuffRequestItemStatusType.Rejected);
      if (taskIsDone && projectWorkItem != null && projectWorkItem.ScrumTaskStep != ScrumTaskStep.Done)
      {
        #region DoneTask

        App.Internals.ScrumManagement.DoneScrumTask(
                scrumTask: projectWorkItem,
                rowVersion: projectWorkItem.RowVersion);

        #endregion
      }
      else if (taskIsDone == false && projectWorkItem != null && projectWorkItem.ScrumTaskStep == ScrumTaskStep.Done)
      {

        #region  AddNewTask
        App.Internals.ProjectManagement.AddProjectWorkItem(
                projectWorkItem: null,
                name: $"ثبت حواله کالا {stuffRequest.Code}",
                description: "",
                color: "",
                departmentId: (int)Departments.Warehouse,
                estimatedTime: 10800,
                isCommit: false,
                scrumTaskTypeId: (int)ScrumTaskTypes.WarehouseIssue,
                userId: null,
                spentTime: 0,
                remainedTime: 0,
                scrumTaskStep: ScrumTaskStep.ToDo,
                projectWorkId: projectWorkItem.ScrumBackLogId,
                baseEntityId: stuffRequest.Id);
        #endregion
      }
      #endregion
    }
    #endregion

    #region CheckStatus
    public void ComplateStuffRequestItem(
        int stuffRequestItemId,
        StuffRequestItemStatusType status)
    {


      var stuffRequestItem = GetStuffRequestItem(id: stuffRequestItemId); ; ComplateStuffRequestItem(stuffRequestItem: stuffRequestItem, status: status);
    }
    public void ComplateStuffRequestItem(
        StuffRequestItem stuffRequestItem,
        StuffRequestItemStatusType status)
    {

      #region Set ComplateStatus
      EditStuffRequestItem(stuffRequestItem: stuffRequestItem,
              rowVersion: stuffRequestItem.RowVersion,
              status: status);
      #endregion

      #region CheckStuffRequestWarehouseIssueTask
      CheckStuffRequestWarehouseIssueTask(stuffRequest: stuffRequestItem.StuffRequest);
      #endregion


    }
    #endregion
  }
}