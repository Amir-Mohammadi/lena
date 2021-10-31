using System;
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
using lena.Models.WarehouseManagement.StuffRequest;
using lena.Models.WarehouseManagement.StuffRequestItem;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public StuffRequest AddStuffRequest(
        StuffRequest stuffRequest,
        TransactionBatch transactionBatch,
        short fromWarehouseId,
        short? toWarehouseId,
        int? toEmployeeId,
        short? toDepartmentId,
        int? scrumEntityId,
        int? productionMaterialRequestId,
        StuffRequestType stuffRequestType,
        string description)
    {

      stuffRequest = stuffRequest ?? repository.Create<StuffRequest>();
      stuffRequest.FromWarehouseId = fromWarehouseId;
      stuffRequest.ToWarehouseId = toWarehouseId;
      stuffRequest.ScrumEntityId = scrumEntityId;
      stuffRequest.StuffRequestType = stuffRequestType;
      stuffRequest.ToEmployeeId = toEmployeeId;
      stuffRequest.ToDepartmentId = toDepartmentId;
      stuffRequest.ProductionMaterialRequestId = productionMaterialRequestId;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: stuffRequest,
                    transactionBatch: transactionBatch,
                    description: description);
      return stuffRequest;
    }
    #endregion
    #region Edit
    public StuffRequest EditStuffRequest(
        int id,
        byte[] rowVersion,
        TValue<short> fromWarehouseId = null,
        TValue<short?> toWarehouseId = null,
        TValue<int?> scrumEntityId = null,
        TValue<StuffRequestType> stuffRequestType = null,
        TValue<string> description = null)
    {

      var stuffRequest = GetStuffRequest(id: id);
      return EditStuffRequest(
                    stuffRequest: stuffRequest,
                    rowVersion: rowVersion,
                    fromWarehouseId: fromWarehouseId,
                    toWarehouseId: toWarehouseId,
                    stuffRequestType: stuffRequestType,
                    scrumEntityId: scrumEntityId,
                    description: description);
    }
    public StuffRequest EditStuffRequest(
        StuffRequest stuffRequest,
        byte[] rowVersion,
        TValue<short> fromWarehouseId = null,
        TValue<short?> toWarehouseId = null,
        TValue<int?> toEmployeeId = null,
        TValue<short?> toDepartmentId = null,
        TValue<StuffRequestType> stuffRequestType = null,
        TValue<int?> scrumEntityId = null,
        TValue<string> description = null)
    {

      if (fromWarehouseId != null)
        stuffRequest.FromWarehouseId = fromWarehouseId;
      if (toWarehouseId != null)
        stuffRequest.ToWarehouseId = toWarehouseId;
      if (toEmployeeId != null)
        stuffRequest.ToEmployeeId = toEmployeeId;
      if (toDepartmentId != null)
        stuffRequest.ToDepartmentId = toDepartmentId;
      if (stuffRequestType != null)
        stuffRequest.StuffRequestType = stuffRequestType;
      if (scrumEntityId != null)
        stuffRequest.ScrumEntityId = scrumEntityId;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: stuffRequest,
                    rowVersion: rowVersion,
                    description: description);
      return retValue as StuffRequest;
    }
    #endregion
    #region Get
    public StuffRequest GetStuffRequest(int id) => GetStuffRequest(selector: e => e, id: id);
    public TResult GetStuffRequest<TResult>(
        Expression<Func<StuffRequest, TResult>> selector,
        int id)
    {

      var result = GetStuffRequests(
                selector: selector,
                id: id).FirstOrDefault();
      if (result == null)
        throw new StuffRequestNotFoundException(id);
      return result;
    }
    public StuffRequest GetStuffRequest(string code) => GetStuffRequest(selector: e => e, code: code);
    public TResult GetStuffRequest<TResult>(
        Expression<Func<StuffRequest, TResult>> selector,
        string code)
    {

      var result = GetStuffRequests(
                selector: selector,
                code: code).FirstOrDefault();
      if (result == null)
        throw new StuffRequestNotFoundException(code);
      return result;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStuffRequests<TResult>(
        Expression<Func<StuffRequest, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> fromWarehouseId = null,
        TValue<int?> toWarehouseId = null,
        TValue<int?> productionMaterialRequestId = null,
        TValue<StuffRequestType> stuffRequestType = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null
        )
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var stuffRequest = baseQuery.OfType<StuffRequest>();
      if (fromWarehouseId != null)
        stuffRequest = stuffRequest.Where(r => r.FromWarehouseId == fromWarehouseId);
      if (toWarehouseId != null)
        stuffRequest = stuffRequest.Where(r => r.ToWarehouseId == toWarehouseId);
      if (stuffRequestType != null)
        stuffRequest = stuffRequest.Where(r => r.StuffRequestType == stuffRequestType);
      if (productionMaterialRequestId != null)
        stuffRequest = stuffRequest.Where(r => r.ProductionMaterialRequestId == productionMaterialRequestId);
      if (fromDateTime != null)
        stuffRequest = stuffRequest.Where(r => r.DateTime >= fromDateTime);
      if (toDateTime != null)
        stuffRequest = stuffRequest.Where(r => r.DateTime <= toDateTime);
      return stuffRequest.Select(selector);
    }
    #endregion
    #region AddProcess
    public StuffRequest AddStuffRequestProcess(
        TransactionBatch transactionBatch,
        StuffRequest stuffRequest,
        short fromWarehouseId,
        short? toWarehouseId,
        int? employeeId,
        short? departmentId,
        int? scrumEntityId,
        int? productionMaterialRequestId,
        StuffRequestType stuffRequestType,
        AddStuffRequestItemInput[] stuffRequestItems,
        string description)
    {

      if (employeeId != null &&
                departmentId != null &&
                (
                stuffRequestType == StuffRequestType.Consume ||
                stuffRequestType == StuffRequestType.Property
                )
                )
      {
        var employee = App.Internals.UserManagement.GetEmployee(
                  id: employeeId.Value);
        if (departmentId != employee.DepartmentId)
          throw new SelectedEmployeeIsNotMembersOfTheSelectedDepartmentException();
      }
      if (departmentId != null && stuffRequestType == StuffRequestType.Property && employeeId == null)
        throw new SelectEmployeeException();
      #region AddTransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      transactionBatch = transactionBatch ?? warehouseManagement.AddTransactionBatch();
      #endregion
      #region AddStuffRequest
      stuffRequest = AddStuffRequest(
              stuffRequest: stuffRequest,
              transactionBatch: transactionBatch,
              fromWarehouseId: fromWarehouseId,
              toWarehouseId: toWarehouseId,
              toEmployeeId: employeeId,
              toDepartmentId: departmentId,
              scrumEntityId: scrumEntityId,
              productionMaterialRequestId: productionMaterialRequestId,
              stuffRequestType: stuffRequestType,
              description: description);
      #endregion
      #region Add StuffRequestItems
      foreach (var stuffRequestItem in stuffRequestItems)
      {
        AddStuffRequestItemProcess(
                  stuffRequestItem: null,
                  transactionBatch: transactionBatch,
                  stuffRequestId: stuffRequest.Id,
                  stuffId: stuffRequestItem.StuffId,
                  billOfMaterialVersion: stuffRequestItem.BillOfMaterialVersion,
                  qty: stuffRequestItem.Qty,
                  unitId: stuffRequestItem.UnitId,
                  description: stuffRequestItem.Description);
      }
      #endregion
      #region Add CheckStuffRequestTask
      var projectWorkItem = AddCheckStuffRequestTask(stuffRequest: stuffRequest);
      #endregion
      #region EditStuffRequest
      if (scrumEntityId == null)
        stuffRequest = EditStuffRequest(
                      stuffRequest: stuffRequest,
                      rowVersion: stuffRequest.RowVersion,
                      scrumEntityId: projectWorkItem.ScrumBackLog.ScrumSprintId);
      #endregion
      return stuffRequest;
    }
    #endregion
    #region EditProcess
    public StuffRequest EditStuffRequestProcess(
        int id,
        byte[] rowVersion,
        int fromWarehouseId,
        int? toWarehouseId,
        int? employeeId,
        int? departmentId,
        int? scrumEntityId,
        StuffRequestType stuffRequestType,
        AddStuffRequestItemInput[] addStuffRequestItems,
        EditStuffRequestItemInput[] editStuffRequestItems,
        DeleteStuffRequestItemInput[] deleteStuffRequestItems,
        string description)
    {

      var stuffRequest = GetStuffRequest(id: id);
      return stuffRequest;
    }
    public StuffRequest EditStuffRequestProcess(
        StuffRequest stuffRequest,
        byte[] rowVersion,
        short fromWarehouseId,
        int? employeeId,
        int? departmentId,
        short? toWarehouseId,
        int? scrumEntityId,
        StuffRequestType stuffRequestType,
        AddStuffRequestItemInput[] addStuffRequestItems,
        EditStuffRequestItemInput[] editStuffRequestItems,
        DeleteStuffRequestItemInput[] deleteStuffRequestItems,
        string description)
    {

      #region AddTransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      var transactionBatch = warehouseManagement.AddTransactionBatch();
      #endregion
      #region EditStuffRequest
      stuffRequest = EditStuffRequest(
             stuffRequest: stuffRequest,
             rowVersion: rowVersion,
             fromWarehouseId: fromWarehouseId,
             toWarehouseId: toWarehouseId,
             scrumEntityId: scrumEntityId,
             stuffRequestType: stuffRequestType,
             description: description);
      #endregion
      #region Add StuffRequestItems
      foreach (var stuffRequestItem in addStuffRequestItems)
      {
        AddStuffRequestItemProcess(
                      stuffRequestItem: null,
                      transactionBatch: transactionBatch,
                      stuffRequestId: stuffRequest.Id,
                      stuffId: stuffRequestItem.StuffId,
                      billOfMaterialVersion: stuffRequestItem.BillOfMaterialVersion,
                      qty: stuffRequestItem.Qty,
                      unitId: stuffRequestItem.UnitId,
                      description: stuffRequestItem.Description);
      }
      #endregion
      #region Edit StuffRequestItems
      foreach (var editStuffRequestItem in editStuffRequestItems)
      {
        EditStuffRequestItem(
                      id: editStuffRequestItem.Id,
                      rowVersion: editStuffRequestItem.RowVersion,
                      stuffRequestId: stuffRequest.Id,
                      stuffId: editStuffRequestItem.StuffId,
                      unitId: editStuffRequestItem.UnitId,
                      qty: editStuffRequestItem.Qty,
                      description: editStuffRequestItem.Description);
      }
      #endregion
      #region Delete StuffRequestItems
      foreach (var deleteStuffRequestItem in deleteStuffRequestItems)
      {
        RemoveStuffRequestItemProcess(
                  transactionBatchId: transactionBatch.Id,
                  id: deleteStuffRequestItem.Id,
                  rowVersion: deleteStuffRequestItem.RowVersion);
      }
      #endregion
      return stuffRequest;
    }
    #endregion
    #region Search
    public IQueryable<StuffRequestFullResult> SearchStuffRequestResult(IQueryable<StuffRequestFullResult> query,
        string search,
        int? fromWarehouseId,
        int? toWarehouseId,
        int? stuffRequestTypeId,
        int? stuffRequestStatusTypeId,
        int? scrumProjectId,
        DateTime? fromDateTime,
        DateTime? toDateTime,
        int? stuffId,
        int? employeeId,
        int? toDepartmentId,
        int? toEmployeeId,
        string productionOrderCode,
        string code
        )
    {

      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.Code.Contains(search) ||
                    item.EmployeeFullName.Contains(search) ||
                    item.FromWarehouseName.Contains(search) ||
                    item.ScrumProjectName.Contains(search) ||
                    item.ToDepartmentName.Contains(search) ||
                    item.ToEmployeeFullName.Contains(search) ||
                    item.ToWarehouseName.Contains(search) ||
                    item.ScrumProjectCode.Contains(search) ||
                    item.ProductionOrderCode.Contains(search)
                select item;
      if (fromWarehouseId != null)
        query = query.Where(i => i.FromWarehouseId == fromWarehouseId);
      if (employeeId != null)
        query = query.Where(i => i.EmployeeId == employeeId);
      if (toDepartmentId != null)
        query = query.Where(i => i.ToDepartmentId == toDepartmentId);
      if (toWarehouseId != null)
        query = query.Where(i => i.ToWarehouseId == toWarehouseId);
      if (stuffRequestTypeId != null)
        query = query.Where(i => (int)i.StuffRequestType == stuffRequestTypeId);
      if (stuffRequestStatusTypeId != null)
        query = query.Where(i => (int)i.Status == stuffRequestStatusTypeId);
      if (scrumProjectId != null)
        query = query.Where(i => i.ScrumProjectId == scrumProjectId);
      if (fromDateTime != null && toDateTime != null)
        query = query.Where(i => fromDateTime <= i.DateTime && i.DateTime <= toDateTime);
      if (toEmployeeId != null)
        query = query.Where(i => i.ToEmployeeId == toEmployeeId);
      if (!string.IsNullOrEmpty(code))
        query = query.Where(i => i.Code == code);
      if (stuffId != null)
        query = query.Where(i => i.StuffRequestItemStuffId == stuffId);
      if (!string.IsNullOrEmpty(productionOrderCode))
        query = query.Where(i => i.ProductionOrderCode == productionOrderCode || i.ProductionOrderCodes.Any(p => p == productionOrderCode));
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffRequestFullResult> SortStuffRequestFullResult(IQueryable<StuffRequestFullResult> query,
        SortInput<StuffRequestSortType> sort)
    {
      switch (sort.SortType)
      {
        case StuffRequestSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case StuffRequestSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case StuffRequestSortType.FromWarehouse:
          return query.OrderBy(a => a.FromWarehouseName, sort.SortOrder);
        case StuffRequestSortType.ScrumProjectCode:
          return query.OrderBy(a => a.ScrumProjectCode, sort.SortOrder);
        case StuffRequestSortType.ScrumProjectName:
          return query.OrderBy(a => a.ScrumProjectName, sort.SortOrder);
        case StuffRequestSortType.StuffRequestType:
          return query.OrderBy(a => a.StuffRequestType, sort.SortOrder);
        case StuffRequestSortType.ToWarehouse:
          return query.OrderBy(a => a.ToWarehouseName, sort.SortOrder);
        case StuffRequestSortType.ToDepartmentName:
          return query.OrderBy(a => a.ToDepartmentName, sort.SortOrder);
        case StuffRequestSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case StuffRequestSortType.ToEmployeeFullName:
          return query.OrderBy(a => a.ToEmployeeFullName, sort.SortOrder);
        case StuffRequestSortType.ProductionOrderCode:
          return query.OrderBy(a => a.ProductionOrderCode, sort.SortOrder);
        case StuffRequestSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case StuffRequestSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToStuffRequestResult
    public Expression<Func<StuffRequest, StuffRequestResult>> ToStuffRequestResult =
        stuffRequest => new StuffRequestResult
        {
          Id = stuffRequest.Id,
          Code = stuffRequest.Code,
          //SendPermissionId = stuffRequest.SendPermissionId,
          //SendPermissionCode = stuffRequest.SendPermission.Code,
          //CustomerId = stuffRequest.SendPermission.OrderItemBlock.OrderItem.Order.CustomerId,
          //OrderItemId = stuffRequest.SendPermission.OrderItemBlock.OrderItemId,
          //OrderItemCode = stuffRequest.SendPermission.OrderItemBlock.OrderItem.Code,
          //OrderItemUnitId = stuffRequest.SendPermission.OrderItemBlock.OrderItem.UnitId,
          //OrderItemUnitName = stuffRequest.SendPermission.OrderItemBlock.OrderItem.Unit.Name,
          //OrderItemQty = stuffRequest.SendPermission.OrderItemBlock.Qty,
          //OrderItemBlockId = stuffRequest.SendPermission.OrderItemBlockId,
          //OrderItemBlockQty = stuffRequest.SendPermission.OrderItemBlock.Qty,
          //OrderItemBlockUnitId = stuffRequest.SendPermission.OrderItemBlock.UnitId,
          //OrderItemBlockUnitName = stuffRequest.SendPermission.OrderItemBlock.Unit.Name,
          //StuffId = stuffRequest.SendPermission.OrderItemBlock.OrderItem.StuffId,
          //StuffCode = stuffRequest.SendPermission.OrderItemBlock.OrderItem.Stuff.Code,
          //StuffName = stuffRequest.SendPermission.OrderItemBlock.OrderItem.Stuff.Name,
          //Qty = stuffRequest.SendPermission.Qty,
          //UnitId = stuffRequest.SendPermission.UnitId,
          //UnitName = stuffRequest.SendPermission.Unit.Name,
          //DateTime = stuffRequest.DateTime,
          //SendProductId = stuffRequest.SendProduct.Id,
          //ExitReceiptId = stuffRequest.SendProduct.ExitReceiptId,
          //ExitReceiptDateTime = stuffRequest.SendProduct.ExitReceipt.DateTime,
          //ExitReceiptConfirm = stuffRequest.SendProduct.ExitReceipt.Confirmed,
          RowVersion = stuffRequest.RowVersion
        };
    #endregion
    #region ToStuffRequestFullResult
    public IQueryable<StuffRequestFullResult> ToStuffRequestFullResult(IQueryable<StuffRequest> stuffRequestResults,
        IQueryable<StuffRequestItem> stuffRequestItems,
        IQueryable<StockPlace> stockPlaces,
        IQueryable<StuffStockPlace> stuffStockPlaces,
        IQueryable<Stuff> stuffs)
    {

      var result = from stuffRequestItem in stuffRequestItems
                   join stuffRequest in stuffRequestResults
                   on stuffRequestItem.StuffRequestId equals stuffRequest.Id
                   join stuff in stuffs
                   on stuffRequestItem.StuffId equals stuff.Id
                   join stuffStockPlace in stuffStockPlaces
                   on stuff.Id equals stuffStockPlace.StuffId into stPlace
                   from stuffStPalce in stPlace.DefaultIfEmpty()
                   join stockPlace in stockPlaces
                   on stuffStPalce.StockPlaceId equals stockPlace.Id into sPlaces
                   from sPlace in sPlaces.DefaultIfEmpty()



                   select new StuffRequestFullResult
                   {
                     Id = stuffRequest.Id,
                     Code = stuffRequest.Code,
                     StuffName = stuffRequest.ProductionMaterialRequest.ProductionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Stuff.Name,
                     StuffCode = stuffRequest.ProductionMaterialRequest.ProductionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Stuff.Code,
                     StuffCodes = stuffRequest.ProductionMaterialRequest.ProductionMaterialRequestDetails.Select(i => i.ProductionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.Stuff.Code),
                     FromWarehouseId = stuffRequest.FromWarehouseId,
                     FromWarehouseName = stuffRequest.FromWarehouse.Name,
                     ToWarehouseId = stuffRequest.ToWarehouseId,
                     ToWarehouseName = stuffRequest.ToWarehouse.Name,
                     StuffRequestType = stuffRequest.StuffRequestType,
                     DateTime = stuffRequest.DateTime,
                     IsDelete = stuffRequest.IsDelete,
                     Description = stuffRequest.Description,
                     ToEmployeeId = stuffRequest.ToEmployeeId,
                     ToEmployeeFullName = stuffRequest.ToEmployee.FirstName + " " + stuffRequest.ToEmployee.LastName,
                     EmployeeId = stuffRequest.User.Employee.Id,
                     EmployeeFullName = stuffRequest.User.Employee.FirstName + " " + stuffRequest.User.Employee.LastName,
                     ToDepartmentId = stuffRequest.ToDepartmentId,
                     ToDepartmentName = stuffRequest.ToDepartment.Name,
                     ScrumProjectId = stuffRequest.ScrumEntity.Id,
                     ScrumProjectCode = stuffRequest.ScrumEntity.Code,
                     ScrumProjectName = stuffRequest.ScrumEntity.Name,
                     ProductionOrderId = stuffRequest.ProductionMaterialRequest.ProductionOrderId,
                     ProductionOrderCode = stuffRequest.ProductionMaterialRequest.ProductionOrder.Code,
                     ProductionOrderIds = stuffRequest.ProductionMaterialRequest.ProductionMaterialRequestDetails.Select(i => i.ProductionOrderId),
                     ProductionOrderCodes = stuffRequest.ProductionMaterialRequest.ProductionMaterialRequestDetails.Select(i => i.ProductionOrder.Code),
                     RowVersion = stuffRequest.RowVersion,
                     StuffRequestItemId = stuffRequestItem.Id,
                     StuffRequestItemCode = stuffRequestItem.Code,
                     StuffRequestItemStuffId = stuffRequestItem.StuffId,
                     StuffType = stuffRequestItem.Stuff.StuffType,
                     BillOfMaterialVersion = stuffRequestItem.BillOfMaterialVersion ?? stuffRequestItem.ResponseStuffRequestItems.OrderByDescending(r => r.Id).FirstOrDefault().BillOfMaterialVersion,
                     BillOfMaterialTitle = stuffRequestItem.BillOfMaterialVersion != null ? stuffRequestItem.BillOfMaterial.Title : stuffRequestItem.ResponseStuffRequestItems.OrderByDescending(r => r.Id).FirstOrDefault().BillOfMaterial.Title,
                     StuffRequestItemStuffCode = stuffRequestItem.Stuff.Code,
                     StuffRequestItemStuffName = stuffRequestItem.Stuff.Name,
                     Qty = stuffRequestItem.Qty,
                     AvailableAmount = 0,
                     UnitId = stuffRequestItem.UnitId,
                     UnitName = stuffRequestItem.Unit.Name,
                     ResponsedQty = stuffRequestItem.ResponsedQty,
                     Status = stuffRequestItem.Status,
                     StuffRequestItemDescription = stuffRequestItem.Description,
                     StuffRequestItemIsDelete = stuffRequestItem.IsDelete,
                     StuffRequestId = stuffRequestItem.StuffRequestId,
                     StuffRequestItemRowVersion = stuffRequestItem.RowVersion,
                     StockPlaceCode = sPlace.Code,
                     StockPlaceTitle = sPlace.Title
                   };



      return result;
    }
    #endregion
    #region AddStuffRequestTask
    public ProjectWorkItem AddCheckStuffRequestTask(int stuffRequestId)
    {

      var stuffRequest = App.Internals.WarehouseManagement.GetStuffRequest(id: stuffRequestId); ; return AddCheckStuffRequestTask(stuffRequest: stuffRequest);
    }
    public ProjectWorkItem AddCheckStuffRequestTask(StuffRequest stuffRequest)
    {

      #region GetOrAddProjectWork
      int? projectWorkId = null;
      if (stuffRequest.ProductionMaterialRequestId != null)
      {
        #region Get ProjectWorkItem
        var materialRequestDetails = stuffRequest.ProductionMaterialRequest.ProductionMaterialRequestDetails;
        foreach (var detail in materialRequestDetails)
        {

          var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                        baseEntityId: detail.ProductionOrderId, //stuffRequest.ProductionMaterialRequest.ProductionOrderId,
                        scrumTaskType: ScrumTaskTypes.ProductionMaterialRequest);
          if (projectWorkItem == null)
          {
            projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                          baseEntityId: detail.ProductionOrderId, // stuffRequest.ProductionMaterialRequest.ProductionOrderId,
                          scrumTaskType: ScrumTaskTypes.ProductionMaterialRequest);
          }
          else
          {
            #region DoneScrumTask

            App.Internals.ScrumManagement.DoneScrumTask(
                    scrumTask: projectWorkItem,
                    rowVersion: projectWorkItem.RowVersion);

            #endregion
          }
          #endregion
          //check projectWork not null
          if (projectWorkItem != null)
          {
            projectWorkId = projectWorkItem.ScrumBackLogId;
          }
        }

      }
      else
      {
        #region GetOrAddCommonProjectGroup
        var projectGroup = App.Internals.ScrumManagement.GetOrAddCommonScrumProjectGroup(
                departmentId: (int)Departments.Warehouse);
        #endregion
        #region GetOrAddCommonProject
        var project = App.Internals.ProjectManagement.GetOrAddCommonProject(
                departmentId: (int)Departments.Warehouse);
        #endregion
        #region GetOrAddCommonProjectStep
        var projectStep = App.Internals.ProjectManagement.GetOrAddCommonProjectStep(
                departmentId: (int)Departments.Warehouse);
        #endregion
        #region Add ProjectWork
        var projectWork = App.Internals.ProjectManagement.AddProjectWork(
                projectWork: null,
                name: $"پروسه درخواست کالا {stuffRequest.Code}",
                description: "",
                color: "",
                departmentId: (int)Departments.Warehouse,
                estimatedTime: 18000,
                isCommit: false,
                projectStepId: projectStep.Id,
                baseEntityId: stuffRequest.Id
            );
        #endregion
        //check projectWork not null
        if (projectWork != null)
        {
          projectWorkId = projectWork.Id;
        }

      }
      #endregion
      #region Add CheckStuffRequestTask

      ProjectWorkItem result = null;
      //check projectWork not null
      if (projectWorkId != null)
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
        result = App.Internals.ProjectManagement.AddProjectWorkItem(
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
                        projectWorkId: projectWorkId.Value,
                        baseEntityId: stuffRequest.Id);
      }

      #endregion


      return result;
    }
    #endregion
  }
}
