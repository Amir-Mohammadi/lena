using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Planning.ProductionPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region AddProductionPlan
    public ProductionPlan AddProductionPlan(
        ProductionPlan productionPlan,
        TransactionBatch transactionBatch,
        string description,
        double qty,
        byte unitId,
        int billOfMaterialStuffId,
        short billOfMaterialVersion,
        int? productionRequestId,
        DateTime estimatedDate,
        bool isTemporary)
    {


      if (isTemporary == false && productionRequestId == null)
        throw new ProductionRequestIdIsNullException();
      productionPlan = productionPlan ?? repository.Create<ProductionPlan>();
      productionPlan.Qty = qty;
      productionPlan.BillOfMaterialStuffId = billOfMaterialStuffId;
      productionPlan.BillOfMaterialVersion = billOfMaterialVersion;
      productionPlan.ProductionRequestId = productionRequestId;
      productionPlan.IsTemporary = isTemporary;
      productionPlan.UnitId = unitId;
      productionPlan.EstimatedDate = estimatedDate;
      productionPlan.Status = ProductionPlanStatus.NotAction;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: productionPlan,
                    transactionBatch: transactionBatch,
                    description: description);
      return productionPlan;
    }
    #endregion
    #region EditProductionPlan
    public ProductionPlan EditProductionPlan(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<short> billOfMaterialVersion = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<int> productionRequestId = null,
        TValue<byte> unitId = null,
        TValue<DateTime> estimatedDate = null,
        TValue<ProductionPlanStatus> status = null)
    {

      var productionPlan = GetProductionPlan(id: id);
      return EditProductionPlan(
                    productionPlan: productionPlan,
                    rowVersion: rowVersion,
                    description: description,
                    qty: qty,
                    unitId: unitId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    billOfMaterialStuffId: billOfMaterialStuffId,
                    productionRequestId: productionRequestId,
                    estimatedDate: estimatedDate,
                    status: status);
    }
    public ProductionPlan EditProductionPlan(
        ProductionPlan productionPlan,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<short> billOfMaterialVersion = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<int> productionRequestId = null,
        TValue<DateTime> estimatedDate = null,
        TValue<ProductionPlanStatus> status = null)

    {

      if (productionRequestId != null)
        productionPlan.ProductionRequestId = productionRequestId;
      if (qty != null)
        productionPlan.Qty = qty;
      if (unitId != null)
        productionPlan.UnitId = unitId;
      if (billOfMaterialVersion != null)
        productionPlan.BillOfMaterialVersion = billOfMaterialVersion;
      if (billOfMaterialStuffId != null)
        productionPlan.BillOfMaterialStuffId = billOfMaterialStuffId;
      if (estimatedDate != null)
        productionPlan.EstimatedDate = estimatedDate;
      if (status != null)
        productionPlan.Status = status;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: productionPlan,
                    description: description,
                    rowVersion: rowVersion);
      return retValue as ProductionPlan;
    }
    #endregion
    #region Get
    public ProductionPlan GetProductionPlan(int id) => GetProductionPlan(selector: e => e, id: id);
    public TResult GetProductionPlan<TResult>(
        Expression<Func<ProductionPlan, TResult>> selector,
        int id)
    {

      var productionPlan = GetProductionPlans(
                selector: selector,
                    id: id)


                .FirstOrDefault();
      if (productionPlan == null)
        throw new ProductionPlanNotFoundException(id);
      return productionPlan;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionPlans<TResult>(
        Expression<Func<ProductionPlan, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> projectWorkItemId = null,
        TValue<int> productionRequestId = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<int> billOfMaterialVersion = null,
        TValue<bool> isTemporary = null,
        TValue<DateTime> estimatedDate = null,
        TValue<DateTime> FromestimatedDate = null,
        TValue<DateTime> ToestimatedDate = null,
        TValue<ProductionPlanStatus> status = null,
        TValue<ProductionPlanStatus[]> statuses = null,
        TValue<ProductionPlanStatus[]> notHasStatuses = null,
        TValue<int> plannerUserId = null)

    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var productionPlans = baseQuery.OfType<ProductionPlan>();
      if (qty != null)
        productionPlans = productionPlans.Where(i => Math.Abs(i.Qty - qty) < 0.00001);
      if (unitId != null)
        productionPlans = productionPlans.Where(i => i.UnitId == unitId);
      if (billOfMaterialVersion != null)
        productionPlans = productionPlans.Where(i => i.BillOfMaterialVersion == billOfMaterialVersion);
      if (billOfMaterialStuffId != null)
        productionPlans = productionPlans.Where(i => i.BillOfMaterialStuffId == billOfMaterialStuffId);
      if (isTemporary != null)
        productionPlans = productionPlans.Where(i => i.IsTemporary == isTemporary);
      if (productionRequestId != null)
        productionPlans = productionPlans.Where(i => i.ProductionRequestId == productionRequestId);
      if (estimatedDate != null)
        productionPlans = productionPlans.Where(i => i.EstimatedDate == estimatedDate);

      if (FromestimatedDate != null)
        productionPlans = productionPlans.Where(i => i.EstimatedDate >= FromestimatedDate);

      if (ToestimatedDate != null)
        productionPlans = productionPlans.Where(i => i.EstimatedDate <= ToestimatedDate);
      if (plannerUserId != null)
        productionPlans = productionPlans.Where(i => i.User.Id == plannerUserId);

      if (status != null)
        productionPlans = productionPlans.Where(i => i.Status.HasFlag(status));
      if (statuses != null)
      {
        var s = ProductionPlanStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        productionPlans = productionPlans.Where(i => (i.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = ProductionPlanStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        productionPlans = productionPlans.Where(i => (i.Status & s) == 0);
      }
      return productionPlans.Select(selector);
    }
    #endregion
    #region AddProductionPlanProcess
    public ProductionPlan AddProductionPlanProcess(
        int? productionRequestId,
        string description,
        double qty,
        byte unitId,
        short billOfMaterialVersion,
        int billOfMaterialStuffId,
        DateTime estimatedDate,
        bool isTemporary,
        bool resetProductionRequest = true)
    {

      ProductionRequest productionRequest = null;
      if (productionRequestId != null)
        productionRequest = App.Internals.SaleManagement.GetProductionRequest(id: productionRequestId.Value);
      return AddProductionPlanProcess(
                    productionRequest: productionRequest,
                    description: description,
                    qty: qty,
                    unitId: unitId,
                    billOfMaterialStuffId: billOfMaterialStuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    estimatedDate: estimatedDate,
                    resetProductionRequest: resetProductionRequest,
                    isTemporary: isTemporary);

    }
    public ProductionPlan AddProductionPlanProcess(
        ProductionRequest productionRequest,
        string description,
        double qty,
        byte unitId,
        int billOfMaterialStuffId,
        short billOfMaterialVersion,
        DateTime estimatedDate,
        bool resetProductionRequest,
        bool isTemporary)
    {

      if (!isTemporary)
      {
        var bom = GetBillOfMaterial(billOfMaterialStuffId, billOfMaterialVersion);
        if (!bom.IsActive)
          throw new BillOfMaterialNotActiveException(billOfMaterialStuffId, billOfMaterialVersion);
      }

      #region AddTransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region AddProductionPlan
      var productionPlan = AddProductionPlan(
              productionPlan: null,
              transactionBatch: transactionBatch,
              description: description,
              qty: qty,
              unitId: unitId,
              billOfMaterialStuffId: billOfMaterialStuffId,
              billOfMaterialVersion: billOfMaterialVersion,
              productionRequestId: productionRequest?.Id,
              estimatedDate: estimatedDate,
              isTemporary: isTemporary);
      #endregion
      #region AddProductionPlanSummary

      AddProductionPlanSummary(
              producedQty: 0,
              scheduledQty: 0,
              productionPlanId: productionPlan.Id);
      #endregion
      #region AddProductionPlanDetailRecursive
      AddProductionPlanDetailProcessRecursive(
              billOfMaterialVersion: billOfMaterialVersion,
              billOfMaterialStuffId: billOfMaterialStuffId,
              productionPlanId: productionPlan.Id,
              unitId: unitId,
              qty: qty);
      #endregion
      if (productionRequest != null)
      {
        #region Get ProjectWorkItem
        var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                baseEntityId: productionRequest.Id,
                scrumTaskType: ScrumTaskTypes.ProductionPlan);
        if (projectWorkItem == null)
        {
          projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                          baseEntityId: productionRequest.Id,
                          scrumTaskType: ScrumTaskTypes.ProductionPlan);

        }

        #endregion
        //check projectWork not null
        if (projectWorkItem != null)
        {
          #region AddProductionScheduleTask

          App.Internals.ProjectManagement.AddProjectWorkItem(
                  projectWorkItem: null,
                  name: $"زمانبندی تولید  {productionPlan.Code}",
                  description: "مراحل تولید را زمانبندی نمایید ",
                  color: "",
                  departmentId: (int)Departments.Planning,
                  estimatedTime: 10800,
                  isCommit: false,
                  scrumTaskTypeId: (int)ScrumTaskTypes.ProductionSchedule,
                  userId: null,
                  spentTime: 0,
                  remainedTime: 0,
                  scrumTaskStep: ScrumTaskStep.ToDo,
                  projectWorkId: projectWorkItem.ScrumBackLogId,
                  baseEntityId: productionPlan.Id);

          #endregion
        }

        #region Reset ProductionRequest
        if (resetProductionRequest)
        {
          App.Internals.SaleManagement.ResetProductionRequestStatus(productionRequest: productionRequest);
          #region check task is done and  done task
          var taskIsDone = productionRequest.ProductionRequestSummary.PlannedQty >= productionRequest.Qty;
          if (taskIsDone && projectWorkItem != null && projectWorkItem.ScrumTaskStep != ScrumTaskStep.Done)
          {
            #region DoneTask
            App.Internals.ScrumManagement.DoneScrumTask(
                    scrumTask: projectWorkItem,
                    rowVersion: projectWorkItem.RowVersion);
            #endregion
          }
          #endregion


        }
        #endregion
      }
      return productionPlan;
    }
    #endregion
    #region AddProductionPlans
    public List<ProductionPlan> SaveProductionPlans(
        int productionRequestId,
        byte[] rowVersion,
        string description,
        AddProductionPlanInput[] addProductionPlans,
        EditProductionPlanInput[] editProductionPlans,
        DeleteProductionPlanInput[] deleteProductionPlans)
    {

      #region Get ProductionRequest
      var productionRequest = App.Internals.SaleManagement.GetProductionRequest(id: productionRequestId);
      #endregion
      #region Add ProductionPlans
      var productionPlans = new List<ProductionPlan>();
      foreach (var addProductionPlan in addProductionPlans)
      {
        var productionPlan = AddProductionPlanProcess(
                  productionRequest: productionRequest,
                  description: addProductionPlan.Description,
                  qty: addProductionPlan.Qty,
                  unitId: addProductionPlan.UnitId,
                  billOfMaterialStuffId: addProductionPlan.BillOfMaterialStuffId,
                  billOfMaterialVersion: addProductionPlan.BillOfMaterialVersion,
                  estimatedDate: addProductionPlan.EstimatedDate,
                  isTemporary: false,
                  resetProductionRequest: false);
      }
      #endregion
      #region Edit ProductionPlans
      foreach (var editProductionPlan in editProductionPlans)
      {
        EditProductionPlanProcess(
                      id: editProductionPlan.Id,
                      rowVersion: editProductionPlan.RowVersion,
                      description: editProductionPlan.Description,
                      estimatedDate: editProductionPlan.EstimatedDate,
                      qty: editProductionPlan.Qty,
                      unitId: editProductionPlan.UnitId,
                      billOfMaterialVersion: editProductionPlan.BillOfMaterialVersion,
                      billOfMaterialStuffId: editProductionPlan.BillOfMaterialStuffId,
                      isTemporary: false
                  );
      }
      #endregion
      #region Delete ProductionPlans
      foreach (var deleteProductionPlan in deleteProductionPlans)
      {
        RemoveProductionPlanProcess(id: deleteProductionPlan.Id, rowVersion: deleteProductionPlan.RowVersion);
      }
      #endregion



      #region Reset ProductionRequest
      App.Internals.SaleManagement.ResetProductionRequestStatus(productionRequest: productionRequest);
      #endregion
      #region check task is-done and  done task
      var taskIsDone = productionRequest.ProductionRequestSummary.PlannedQty >= productionRequest.Qty;
      if (taskIsDone)
      {
        #region Get ProjectWorkItem
        var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                baseEntityId: productionRequestId,
                scrumTaskType: ScrumTaskTypes.ProductionPlan);
        #endregion
        #region DoneTask
        if (projectWorkItem != null)
          App.Internals.ScrumManagement.DoneScrumTask(
                        scrumTask: projectWorkItem,
                        rowVersion: projectWorkItem.RowVersion);
        #endregion
      }
      #endregion
      return productionPlans;
    }
    #endregion
    #region ToResult
    public IQueryable<ProductionPlanResult> ToProductionPlanResult(IQueryable<ProductionPlan> query, IQueryable<BillOfMaterialPublishRequest> billOfMaterialPublishRequests)
    {
      var resultQuery = from productionPlan in query
                        let latestBillOfMaterialPublishRequest = productionPlan.BillOfMaterial.LatestBillOfMaterialPublishRequest
                        select new ProductionPlanResult
                        {
                          Id = productionPlan.Id,
                          OrderItemCode = productionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Code,
                          CustomerCode = productionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Order.Customer.Code,
                          CustomerName = productionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Order.Customer.Name,
                          OrderTypeName = productionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Order.OrderType.Name,
                          StuffId = productionPlan.BillOfMaterialStuffId,
                          StuffCode = productionPlan.BillOfMaterial.Stuff.Code,
                          StuffName = productionPlan.BillOfMaterial.Stuff.Name,
                          RequestDate = productionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.RequestDate,
                          DeliveryDate = productionPlan.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.DeliveryDate,
                          EstimatedDate = productionPlan.EstimatedDate,
                          Qty = productionPlan.Qty,
                          UnitId = productionPlan.UnitId,
                          UnitName = productionPlan.Unit.Name,
                          Status = productionPlan.Status,
                          BillOfMaterialVersion = productionPlan.BillOfMaterial.Version,
                          IsActive = productionPlan.BillOfMaterial.IsActive,
                          IsPublished = productionPlan.BillOfMaterial.IsPublished,
                          BillOfMaterialPublishRequestStatus = latestBillOfMaterialPublishRequest.Status,
                          BillOfMaterialPublishRequestType = latestBillOfMaterialPublishRequest.Type,
                          BillOfMaterialTitle = productionPlan.BillOfMaterial.Title,
                          ProductionRequestId = productionPlan.ProductionRequestId,
                          EmployeeFullName = productionPlan.User.Employee.FirstName + " " + productionPlan.User.Employee.LastName,
                          DateTime = productionPlan.DateTime,
                          RowVersion = productionPlan.RowVersion,
                          DeadlineDate = productionPlan.ProductionRequest.DeadlineDate,
                          Description = productionPlan.Description,
                          ProductionRequestCode = productionPlan.ProductionRequest.Code
                        };

      return resultQuery;
    }
    #endregion
    #region Sort
    public IQueryable<ProductionPlanResult> SortProductionPlanResult(IQueryable<ProductionPlanResult> query, SortInput<ProductionPlanSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProductionPlanSortType.Code:
          query = query.OrderBy(x => x.Id, sort.SortOrder);
          break;
        case ProductionPlanSortType.StuffCode:
          query = query.OrderBy(x => x.StuffCode, sort.SortOrder);
          break;
        case ProductionPlanSortType.StuffName:
          query = query.OrderBy(x => x.StuffName, sort.SortOrder);
          break;
        case ProductionPlanSortType.BillOfMaterialVersion:
          query = query.OrderBy(x => x.BillOfMaterialVersion, sort.SortOrder);
          break;
        case ProductionPlanSortType.BillOfMaterialPublishRequestStatus:
          query = query.OrderBy(x => x.BillOfMaterialPublishRequestStatus, sort.SortOrder);
          break;
        case ProductionPlanSortType.BillOfMaterialPublishRequestType:
          query = query.OrderBy(x => x.BillOfMaterialPublishRequestType, sort.SortOrder);
          break;
        case ProductionPlanSortType.OrderItemCode:
          query = query.OrderBy(x => x.OrderItemCode, sort.SortOrder);
          break;
        case ProductionPlanSortType.OrderTypeName:
          query = query.OrderBy(x => x.OrderTypeName, sort.SortOrder);
          break;
        case ProductionPlanSortType.CustomerCode:
          query = query.OrderBy(x => x.CustomerCode, sort.SortOrder);
          break;
        case ProductionPlanSortType.CustomerName:
          query = query.OrderBy(x => x.CustomerName, sort.SortOrder);
          break;
        case ProductionPlanSortType.Qty:
          query = query.OrderBy(x => x.Qty, sort.SortOrder);
          break;
        case ProductionPlanSortType.UnitName:
          query = query.OrderBy(x => x.UnitName, sort.SortOrder);
          break;
        case ProductionPlanSortType.DeadlineDate:
          query = query.OrderBy(x => x.DeadlineDate, sort.SortOrder);
          break;
        case ProductionPlanSortType.RequestDate:
          query = query.OrderBy(x => x.RequestDate, sort.SortOrder);
          break;
        case ProductionPlanSortType.DeliveryDate:
          query = query.OrderBy(x => x.DeliveryDate, sort.SortOrder);
          break;
        case ProductionPlanSortType.EstimatedDate:
          query = query.OrderBy(x => x.EstimatedDate, sort.SortOrder);
          break;
        case ProductionPlanSortType.Description:
          query = query.OrderBy(x => x.Description, sort.SortOrder);
          break;
        case ProductionPlanSortType.ProductionRequestCode:
          query = query.OrderBy(x => x.ProductionRequestCode, sort.SortOrder);
          break;
        case ProductionPlanSortType.Status:
          query = query.OrderBy(x => x.Status, sort.SortOrder);
          break;
        case ProductionPlanSortType.EmployeeFullName:
          query = query.OrderBy(x => x.EmployeeFullName, sort.SortOrder);
          break;
        case ProductionPlanSortType.DateTime:
          query = query.OrderBy(x => x.DateTime, sort.SortOrder);
          break;
        case ProductionPlanSortType.IsActive:
          query = query.OrderBy(x => x.IsActive, sort.SortOrder);
          break;
        case ProductionPlanSortType.IsPublished:
          query = query.OrderBy(x => x.IsPublished, sort.SortOrder);
          break;
        default:
          query = query.OrderBy(x => x.Id, sort.SortOrder);
          break;
      }
      return query;
    }
    #endregion
    #region Search
    public IQueryable<ProductionPlanResult> SearchProductionPlanResultQuery(
        IQueryable<ProductionPlanResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
        int? stuffId,
        string orderItemCode,
        DateTime? fromEstimatedDate,
        DateTime? toEstimatedDate,
        DateTime? fromDeadlineDate,
        DateTime? toDeadlineDate,
        int? billOfMaterialVersion,
        bool? isActive,
        bool? isPublished,
        int? productionRequestId,
        BillOfMaterialPublishRequestStatus? billOfMaterialPublishRequestStatus,
        BillOfMaterialPublishRequestType? billOfMaterialPublishRequestType)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(i =>
                             i.StuffCode.Contains(searchText) ||
                             i.StuffName.Contains(searchText) ||
                             i.BillOfMaterialVersion.ToString().Contains(searchText) ||
                             i.UnitName.Contains(searchText) ||
                             i.EmployeeFullName.Contains(searchText) ||
                             i.Description.Contains(searchText));
      }

      if (stuffId != null)
      {
        query = query.Where(x => x.StuffId == stuffId);
      }
      if (!string.IsNullOrWhiteSpace(orderItemCode))
      {
        query = query.Where(x => x.OrderItemCode == orderItemCode);
      }
      if (fromEstimatedDate != null)
      {
        query = query.Where(x => x.EstimatedDate >= fromEstimatedDate);
      }
      if (toEstimatedDate != null)
      {
        query = query.Where(x => x.EstimatedDate <= toEstimatedDate);
      }
      if (fromDeadlineDate != null)
      {
        query = query.Where(x => x.DeadlineDate >= fromDeadlineDate);
      }
      if (toDeadlineDate != null)
      {
        query = query.Where(x => x.DeadlineDate <= toDeadlineDate);
      }
      if (billOfMaterialVersion != null)
      {
        query = query.Where(x => x.BillOfMaterialVersion == billOfMaterialVersion);
      }
      if (isActive != null)
      {
        query = query.Where(x => x.IsActive == isActive);
      }
      if (isPublished != null)
      {
        query = query.Where(x => x.IsPublished == isPublished);
      }
      if (productionRequestId != null)
      {
        query = query.Where(x => x.ProductionRequestId == productionRequestId);
      }
      if (billOfMaterialPublishRequestStatus != null)
      {
        query = query.Where(x => x.BillOfMaterialPublishRequestStatus == billOfMaterialPublishRequestStatus);
      }
      if (billOfMaterialPublishRequestType != null)
      {
        query = query.Where(x => x.BillOfMaterialPublishRequestType == billOfMaterialPublishRequestType);
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region ChangeProductionPlansEstimatedDate
    public void ChangeProductionPlansEstimatedDate(ChangeProductionPlansEstimatedDateInput changeProductionPlansEstimatedDate)
    {

      foreach (var changeItem in changeProductionPlansEstimatedDate.ChangeDetails)
      {
        ChangeProductionPlanEstimatedDateProcess(
                      deltaDay: changeProductionPlansEstimatedDate.DeltaDay,
                      productionPlanId: changeItem.ProductionPlanId,
                      rowVersion: changeItem.RowVersion);
      }
    }
    #endregion
    #region ChangeProductionPlanEstimatedDate
    public ProductionPlan ChangeProductionPlanEstimatedDateProcess(int deltaDay, int productionPlanId, byte[] rowVersion)
    {

      #region RemoveProductionPlan
      var productionPlan = RemoveProductionPlanProcess(id: productionPlanId,
              rowVersion: rowVersion);
      #endregion
      #region AddProductionPlan
      var newEstimatedDate = productionPlan.EstimatedDate.AddDays(deltaDay);
      return AddProductionPlanProcess(
                   productionRequestId: productionPlan.ProductionRequestId,
                   description: productionPlan.Description,
                   qty: productionPlan.Qty,
                   unitId: productionPlan.UnitId,
                   billOfMaterialStuffId: productionPlan.BillOfMaterialStuffId,
                   billOfMaterialVersion: productionPlan.BillOfMaterialVersion,
                   estimatedDate: newEstimatedDate,
                   isTemporary: productionPlan.IsTemporary);
      #endregion
    }
    #endregion
    #region EditProductionPlanProcess
    public void EditProductionPlanProcess(int id, byte[] rowVersion,
        TValue<string> description,
        TValue<double> qty,
        TValue<byte> unitId,
        TValue<short> billOfMaterialVersion,
        TValue<int> billOfMaterialStuffId,
        TValue<DateTime> estimatedDate,
        bool isTemporary
        )
    {

      #region RemoveProductionPlan
      var productionPlan = RemoveProductionPlanProcess(id: id,
              rowVersion: rowVersion);
      #endregion
      #region add production plan process
      if (description == null)
        description = productionPlan.Description;
      if (qty == null)
        qty = productionPlan.Qty;
      if (unitId == null)
        unitId = productionPlan.UnitId;
      if (billOfMaterialStuffId == null) billOfMaterialStuffId = productionPlan.BillOfMaterialStuffId;
      if (billOfMaterialVersion == null) billOfMaterialVersion = productionPlan.BillOfMaterialVersion;
      if (estimatedDate == null)
        estimatedDate = productionPlan.EstimatedDate;
      AddProductionPlanProcess(
                   productionRequestId: productionPlan.ProductionRequestId,
                   description: description,
                   qty: qty,
                   unitId: unitId,
                   billOfMaterialVersion: billOfMaterialVersion,
                   billOfMaterialStuffId: billOfMaterialStuffId,
                   estimatedDate: estimatedDate,
                   isTemporary: isTemporary);
      #endregion
    }
    #endregion
    #region RemoveProductionPlanProcess

    public void RemoveProductionPlansProcess(DeleteProductionPlansInput deleteProductionPlansInput)
    {

      foreach (var deleteProductionPlanInput in deleteProductionPlansInput.DeleteProductionPlansInputs)
      {
        RemoveProductionPlanProcess(
                      id: deleteProductionPlanInput.Id,
                      rowVersion: deleteProductionPlanInput.RowVersion);
      }
    }


    public ProductionPlan RemoveProductionPlanProcess(
        int id,
        byte[] rowVersion)
    {

      #region GetProductionPlan
      var productionPlan = GetProductionPlan(id: id);
      #endregion
      #region RemoveProductionPlan
      return RemoveProductionPlanProcess(
              productionPlan: productionPlan,
              rowVersion: rowVersion);
      #endregion
    }
    public ProductionPlan RemoveProductionPlanProcess(
        ProductionPlan productionPlan,
        byte[] rowVersion)
    {

      #region Remove productionPlan BaseEntity
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
              transactionBatchId: null,
              baseEntity: productionPlan,
              rowVersion: rowVersion);
      #endregion
      #region GetProudctionPlanDetails
      var productionPlanDetails = GetProductionPlanDetails(
              e => e,
              productionPlanId: productionPlan.Id,
              isDelete: false);
      #endregion
      #region Remove productionPlanDetails 
      foreach (var item in productionPlanDetails)
      {
        RemoveProductionPlanDetailProcess(
                      productionPlanDetail: item,
                      rowVersion: item.RowVersion);
      }
      #endregion
      return productionPlan;
    }
    #endregion
    #region ResetStatus
    public ProductionPlan ResetProductionPlanStatus(int id)
    {

      #region GetProductionPlan
      var productionPlan = GetProductionPlan(id: id);
      #endregion
      return ResetProductionPlanStatus(productionPlan: productionPlan);
    }
    public ProductionPlan ResetProductionPlanStatus(ProductionPlan productionPlan)
    {


      #region ResetProductionPlanSummary
      var productionPlanSummary = ResetProductionPlanSummaryByProductionPlanId(productionPlanId: productionPlan.Id);
      #endregion
      #region Define Status
      var status = ProductionPlanStatus.None;
      if (productionPlanSummary.ScheduledQty > 0)
      {
        if (productionPlanSummary.ScheduledQty >= productionPlanSummary.ProductionPlan.Qty)
          status = status | ProductionPlanStatus.Scheduled;
        else
          status = status | ProductionPlanStatus.Scheduling;
      }

      if (productionPlanSummary.ProducedQty > 0)
      {
        if (productionPlanSummary.ProducedQty >= productionPlanSummary.ProductionPlan.Qty)
          status = status | ProductionPlanStatus.Produced;
        else
          status = status | ProductionPlanStatus.InProduction;
      }
      if (status == ProductionPlanStatus.None)
        status = ProductionPlanStatus.NotAction;
      #endregion
      #region Edit ProductionPlan
      if (status != productionPlan.Status)
        EditProductionPlan(
                      productionPlan: productionPlan,
                      rowVersion: productionPlan.RowVersion,
                      status: status);
      #endregion
      return productionPlan;
    }
    #endregion
  }
}
