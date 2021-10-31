using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.SaleManagement.ProductionRequest;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Planning.ProductionPlan;
using lena.Models.Common;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums.SortTypes;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.UserManagement.Exception;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Get
    public ProductionRequest GetProductionRequest(int id) => GetProductionRequest(selector: e => e, id: id);
    public TResult GetProductionRequest<TResult>(
        Expression<Func<ProductionRequest, TResult>> selector,
            int id)
    {

      var productionRequest = GetProductionRequests(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (productionRequest == null)
        throw new ProductionRequestNotFoundException(id);
      return productionRequest;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionRequests<TResult>(
        Expression<Func<ProductionRequest, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> orderItemId = null,
        TValue<int> checkOrderItemId = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<DateTime> deadlineDate = null,
        TValue<DateTime> FromdeadlineDate = null,
        TValue<DateTime> TodeadlineDate = null,
        TValue<ProductionRequestStatus> status = null,
        TValue<ProductionRequestStatus[]> statuses = null,
        TValue<ProductionRequestStatus[]> notHasStatuses = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<ProductionRequest>();
      if (qty != null)
        query = query.Where(i => Math.Abs(i.Qty - qty) < 0.000001);
      if (unitId != null)
        query = query.Where(i => i.UnitId == unitId);
      if (checkOrderItemId != null)
        query = query.Where(i => i.CheckOrderItemId == checkOrderItemId);
      if (orderItemId != null)
        query = query.Where(i => i.CheckOrderItem.OrderItemConfirmation.OrderItemId == orderItemId);
      if (deadlineDate != null)
        query = query.Where(i => i.DeadlineDate == deadlineDate);
      if (FromdeadlineDate != null)
        query = query.Where(i => i.DeadlineDate >= FromdeadlineDate);
      if (TodeadlineDate != null)
        query = query.Where(i => i.DeadlineDate <= TodeadlineDate);
      if (status != null)
        query = query.Where(i => i.Status.HasFlag(status));
      if (statuses != null)
      {
        var s = ProductionRequestStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        query = query.Where(i => (i.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = ProductionRequestStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        query = query.Where(i => (i.Status & s) == 0);
      }
      return query.Select(selector);
    }
    #endregion
    #region Add
    public ProductionRequest AddProductionRequest(
        ProductionRequest productionRequest,
        TransactionBatch transactionBatch,
        string description,
        double qty,
        byte unitId,
        DateTime deadlineDate,
        int checkOrderItemId)
    {

      productionRequest = productionRequest ?? repository.Create<ProductionRequest>();
      if (qty <= 0)
        throw new QtyInvalidException(qty);
      productionRequest.Qty = qty;
      productionRequest.DeadlineDate = deadlineDate;
      productionRequest.CheckOrderItemId = checkOrderItemId;
      productionRequest.UnitId = unitId;
      productionRequest.Status = ProductionRequestStatus.NotAction;
      App.Internals.ApplicationBase.AddBaseEntity(
                            baseEntity: productionRequest,
                            transactionBatch: transactionBatch,
                            description: description);
      return productionRequest;
    }
    #endregion
    #region Edit
    public ProductionRequest EditProductionRequest(
        int id,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<DateTime> deadlineDate = null,
        TValue<int> checkOrderItemId = null,
        TValue<byte> unitId = null,
        TValue<double> plannedQty = null,
        TValue<double> scheduledQty = null,
        TValue<double> producedQty = null,
        TValue<ProductionRequestStatus> status = null)
    {

      var productionRequest = GetProductionRequest(id: id);
      return EditProductionRequest(
                    productionRequest: productionRequest,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description,
                    qty: qty,
                    unitId: unitId,
                    checkOrderItemId: checkOrderItemId,
                    deadlineDate: deadlineDate,
                    status: status);
    }
    public ProductionRequest EditProductionRequest(
        ProductionRequest productionRequest,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<int> checkOrderItemId = null,
        TValue<DateTime> deadlineDate = null,
        TValue<ProductionRequestStatus> status = null)
    {

      if (checkOrderItemId != null)
        productionRequest.CheckOrderItemId = checkOrderItemId;
      if (qty != null)
      {
        if (qty <= 0)
          throw new QtyInvalidException(qty);
        else
          productionRequest.Qty = qty;
      }
      if (unitId != null)
        productionRequest.UnitId = unitId;
      if (status != null)
        productionRequest.Status = status;
      if (deadlineDate != null)
        productionRequest.DeadlineDate = deadlineDate;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: productionRequest,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description);
      return retValue as ProductionRequest;
    }
    #endregion
    #region RemoveProcess
    public ProductionRequest RemoveProductionRequestProcess(
        int id,
        byte[] rowVersion)
    {

      #region GetProductionRequest
      var productionRequest = GetProductionRequest(id: id);
      #endregion
      #region RemoveProductionRequest
      return RemoveProductionRequestProcess(
              productionRequest: productionRequest,
              rowVersion: rowVersion);
      #endregion
    }
    public ProductionRequest RemoveProductionRequestProcess(
        ProductionRequest productionRequest,
        byte[] rowVersion)
    {

      #region Remove productionRequest BaseEntity
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
              transactionBatchId: null,
              baseEntity: productionRequest,
              rowVersion: rowVersion);
      #endregion
      #region GetProudctionPlan
      var productionPlans = App.Internals.Planning.GetProductionPlans(
              selector: e => e,
              productionRequestId: productionRequest.Id);
      #endregion
      #region Remove productionPlans 
      foreach (var item in productionPlans)
      {
        App.Internals.Planning.RemoveProductionPlanProcess(
                      id: item.Id,
                      rowVersion: item.RowVersion);
      }
      #endregion
      #region productionRequestInfo 
      var productionRequestInfo = GetProductionRequest(
              selector: e => new
              {
                ProductionRequestId = e.Id,
                CheckOrderItemId = e.CheckOrderItemId,
                OrderItemConfirmationId = e.CheckOrderItem.OrderItemConfirmationId,
                OrderItemId = e.CheckOrderItem.OrderItemConfirmation.OrderItemId,
              },
              id: productionRequest.Id);

      #endregion
      #region ResetOrderItemStatus
      var orderItem = ResetOrderItemStatus(id: productionRequestInfo.OrderItemId);
      #endregion
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                  baseEntityId: productionRequestInfo.OrderItemConfirmationId,
                  scrumTaskType: ScrumTaskTypes.CheckOrderItem);

      if (projectWorkItem == null)
      {
        #region GetDoneTask
        projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                baseEntityId: productionRequestInfo.OrderItemConfirmationId,
                scrumTaskType: ScrumTaskTypes.CheckOrderItem);

        #endregion
      }

      #endregion
      #region Add CheckOrderItemTask 
      if (projectWorkItem != null)
      {

        #region Add CheckOrderItemTask if need 
        App.Internals.ProjectManagement.AddProjectWorkItem(
                projectWorkItem: null,
                name: $"بررسی سفارش {orderItem.Code} {orderItem.Stuff.Name}",
                description: "بررسی سفارش از نظر مقدار موجود در انبار و ثبت رزرو  و درخواست تولید",
                color: "",
                departmentId: (int)Departments.Planning,
                estimatedTime: 10800,
                isCommit: false,
                scrumTaskTypeId: (int)ScrumTaskTypes.CheckOrderItem,
                userId: null,
                spentTime: 0,
                remainedTime: 0,
                scrumTaskStep: ScrumTaskStep.ToDo,
                projectWorkId: projectWorkItem.ScrumBackLogId,
                baseEntityId: productionRequestInfo.OrderItemConfirmationId);
        #endregion
      }
      #endregion
      return productionRequest;
    }
    #endregion
    #region AddProcess
    public ProductionRequest AddProductionRequestProcess(
        TransactionBatch transactionBatch,
        CheckOrderItem checkOrderItem,
        string description,
        double qty,
        byte unitId,
        DateTime deadlineDate)
    {

      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
              baseEntityId: checkOrderItem.OrderItemConfirmationId,
              scrumTaskType: ScrumTaskTypes.CheckOrderItem);
      #endregion
      #region Add TransactionBatch
      transactionBatch = transactionBatch ?? App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region AddProductionRequest
      var productionRequest = AddProductionRequest(
              productionRequest: null,
              transactionBatch: transactionBatch,
              description: description,
              qty: qty,
              unitId: unitId,
              deadlineDate: deadlineDate,
              checkOrderItemId: checkOrderItem.Id);
      #endregion
      #region AddProductionRequestSummary
      AddProductionRequestSummary(
              plannedQty: 0,
              producedQty: 0,
              scheduledQty: 0,
              productionRequestId: productionRequest.Id);
      #endregion
      #region AddProductionPlanTask
      //check projectWork not null
      if (projectWorkItem != null)
      {
        App.Internals.ProjectManagement.AddProjectWorkItem(
                      projectWorkItem: null,
                      name: $"برنامه ریزی {productionRequest.Code}",
                      description: "تعیین نسخه محصول و برنامه کار ",
                      color: "",
                      departmentId: (int)Departments.Planning,
                      estimatedTime: 1800,
                      isCommit: false,
                      scrumTaskTypeId: (int)ScrumTaskTypes.ProductionPlan,
                      userId: null,
                      spentTime: 0,
                      remainedTime: 1800,
                      scrumTaskStep: ScrumTaskStep.ToDo,
                      projectWorkId: projectWorkItem.ScrumBackLogId,
                      baseEntityId: productionRequest.Id);
      }

      #endregion
      return productionRequest;
    }
    #endregion
    #region ToResult
    public Expression<Func<ProductionRequest, ProductionRequestResult>> ToProductionRequestResult =
        productionRequest => new ProductionRequestResult()
        {
          Id = productionRequest.Id,
          Code = productionRequest.Code,
          Description = productionRequest.Description,
          OrderDescription = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Order.Description,
          OrderItemId = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItemId,
          OrderItemCode = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Code,
          CustomerCode = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Order.Customer.Code,
          CustomerName = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Order.Customer.Name,
          OrderTypeName = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Order.OrderType.Name,
          StuffId = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.StuffId,
          StuffCode = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Stuff.Code,
          StuffName = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Stuff.Name,
          StuffNoun = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Stuff.Noun,
          BillOfMaterialVersion = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.BillOfMaterialVersion,
          BillOfMaterialTitle = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.BillOfMaterial.Title,
          RequestDate = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.RequestDate,
          DeliveryDate = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.DeliveryDate,
          Qty = productionRequest.Qty,
          UnitId = productionRequest.UnitId,
          UnitName = productionRequest.Unit.Name,
          DeadlineDate = productionRequest.DeadlineDate,
          CheckOrderItemId = productionRequest.CheckOrderItemId,
          Status = productionRequest.Status,
          PlannedQty = productionRequest.ProductionRequestSummary.PlannedQty,
          ScheduledQty = productionRequest.ProductionRequestSummary.ScheduledQty,
          ProducedQty = productionRequest.ProductionRequestSummary.ProducedQty,
          RowVersion = productionRequest.RowVersion
        };
    #endregion
    #region ToFullResult
    public Expression<Func<ProductionRequest, FullProductionRequestResult>> ToFullProductionRequestResult =
        productionRequest => new FullProductionRequestResult
        {
          Id = productionRequest.Id,
          Code = productionRequest.Code,
          Description = productionRequest.Description,
          RowVersion = productionRequest.RowVersion,
          Qty = productionRequest.Qty,
          UnitId = productionRequest.UnitId,
          DeadlineDate = productionRequest.DeadlineDate,
          UnitName = productionRequest.Unit.Name,
          StuffCode = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Stuff.Code,
          StuffName = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Stuff.Name,
          StuffNoun = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.Stuff.Noun,
          CheckOrderItemId = productionRequest.CheckOrderItemId,
          Status = productionRequest.Status,
          PlannedQty = productionRequest.ProductionRequestSummary.PlannedQty,
          ScheduledQty = productionRequest.ProductionRequestSummary.ScheduledQty,
          ProducedQty = productionRequest.ProductionRequestSummary.ProducedQty,
          //OrderId = productionRequest.CheckOrderItem.OrderItemConfirmation.OrderItem.OrderId,
          //Version = bom?.Version ?? 0,
          //ProductionPlans = productionRequest.ProductionPlans.Select(planning.ToProductionPlanResult)
          //    .ToArray(),
          //BillOfMaterials = planning
          //    .ToBillOfMaterialComboResult(planning.GetBillOfMaterials(stuffId: bom.StuffId)
          //        
          //        )
          //    .ToArray(),
          //StuffUnits = warehouse.GetStuffUnits(bom.Stuff.Id).ToArray(),
          //WorkPlans = planning
          //    .ToWorkPlanComboResultQuery(planning.GetWorkPlans(billOfMaterialStuffId: bom.StuffId)
          //        
          //        )
          //    .ToArray()
          //{
          //    var bom = (productionRequest.ScrumTask.ScrumBackLog.ScrumSprint.ScrumProject as OrderItem)?.BillOfMaterial;
          //    var warehouse = App.Internals.WarehouseManagement;
          //    var planning = App.Internals.Planning;
          //    var unit = productionRequest.Unit;
          //    var product = bom?.Stuff;
          //    return 
        };
    #endregion
    #region ToResultQuery
    public IQueryable<ProductionRequestResult> ToProductionRequestResultQuery(
        IQueryable<ProductionRequest> query)

    {
      return from item in query
             let unit = item.Unit
             let orderItem = item.CheckOrderItem.OrderItemConfirmation.OrderItem
             let bom = orderItem.BillOfMaterial
             let product = bom.Stuff
             select new ProductionRequestResult
             {
               Id = item.Id,
               Description = item.Description,
               RowVersion = item.RowVersion,
               Qty = item.Qty,
               UnitId = item.UnitId,
               DeadlineDate = item.DeadlineDate,
               UnitName = unit.Name,
               StuffCode = product.Code,
               StuffName = product.Name,
               OrderItemId = orderItem.Id,
               OrderItemCode = orderItem.Code,
               BillOfMaterialVersion = bom.Version,
               BillOfMaterialTitle = bom == null ? "" : bom.Title,
               CheckOrderItemId = item.CheckOrderItemId,
             };
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ProductionRequestResult> SortProductionRequestResult(
        IQueryable<ProductionRequestResult> query, SortInput<ProductionRequestSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProductionRequestSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ProductionRequestSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case ProductionRequestSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case ProductionRequestSortType.BillOfMaterialVersion:
          return query.OrderBy(a => a.BillOfMaterialVersion, sort.SortOrder);
        case ProductionRequestSortType.OrderItemCode:
          return query.OrderBy(a => a.OrderItemCode, sort.SortOrder);
        case ProductionRequestSortType.OrderTypeName:
          return query.OrderBy(a => a.OrderTypeName, sort.SortOrder);
        case ProductionRequestSortType.CustomerCode:
          return query.OrderBy(a => a.CustomerCode, sort.SortOrder);
        case ProductionRequestSortType.CustomerName:
          return query.OrderBy(a => a.CustomerName, sort.SortOrder);
        case ProductionRequestSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case ProductionRequestSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case ProductionRequestSortType.DeadlineDate:
          return query.OrderBy(a => a.DeadlineDate, sort.SortOrder);
        case ProductionRequestSortType.RequestDate:
          return query.OrderBy(a => a.RequestDate, sort.SortOrder);
        case ProductionRequestSortType.DeliveryDate:
          return query.OrderBy(a => a.DeliveryDate, sort.SortOrder);
        case ProductionRequestSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case ProductionRequestSortType.PlannedQty:
          return query.OrderBy(a => a.PlannedQty, sort.SortOrder);
        case ProductionRequestSortType.ScheduledQty:
          return query.OrderBy(a => a.ScheduledQty, sort.SortOrder);
        case ProductionRequestSortType.ProducedQty:
          return query.OrderBy(a => a.ProducedQty, sort.SortOrder);
        case ProductionRequestSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search

    public IQueryable<ProductionRequestResult> SearchProductionRequestResult(
        IQueryable<ProductionRequestResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
        int? stuffId = null,
        int? orderItemId = null,
        DateTime? deadLine = null,
        ProductionRequestStatus? status = null,
        int? version = null)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(i => i.Code.Contains(searchText) ||
                                 i.StuffCode.Contains(searchText) ||
                                 i.StuffName.Contains(searchText) ||
                                 i.OrderItemCode.Contains(searchText) ||
                                 i.OrderTypeName.Contains(searchText) ||
                                 i.CustomerCode.Contains(searchText) ||
                                 i.CustomerName.Contains(searchText) ||
                                 i.Description.Contains(searchText) ||
                                 i.UnitName.Contains(searchText) ||
                                 i.BillOfMaterialVersion.ToString().Contains(searchText));
      }

      if (stuffId != null)
        query = query.Where(x => x.StuffId == stuffId);
      if (orderItemId != null)
        query = query.Where(x => x.OrderItemId == orderItemId);
      if (deadLine != null)
        query = query.Where(x => x.DeadlineDate >= deadLine);
      if (status != null)
        query = query.Where(x => x.Status <= status);
      if (version != null)
        query = query.Where(x => x.BillOfMaterialVersion == version);

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }

    #endregion
    #region ResetStatus
    public ProductionRequest ResetProductionRequestStatus(int id)
    {

      #region GetProductionRequest
      var productionRequest = GetProductionRequest(id: id);
      #endregion
      return ResetProductionRequestStatus(productionRequest: productionRequest);
    }
    public ProductionRequest ResetProductionRequestStatus(ProductionRequest productionRequest)
    {

      #region ResetOrderItemSummary
      var productionRequestSummary = ResetProductionRequestSummaryByProductionRequestId(productionRequestId: productionRequest.Id);
      #endregion
      #region Define Status
      var status = ProductionRequestStatus.None;
      if (productionRequestSummary.PlannedQty > 0)
      {
        if (productionRequestSummary.PlannedQty >= productionRequestSummary.ProductionRequest.Qty)
          status = status | ProductionRequestStatus.Planned;
        else
          status = status | ProductionRequestStatus.Planning;
      }

      if (productionRequestSummary.ScheduledQty > 0)
      {
        if (productionRequestSummary.ScheduledQty >= productionRequestSummary.ProductionRequest.Qty)
          status = status | ProductionRequestStatus.Scheduled;
        else
          status = status | ProductionRequestStatus.Scheduling;
      }

      if (productionRequestSummary.ProducedQty > 0)
      {
        if (productionRequestSummary.ProducedQty >= productionRequestSummary.ProductionRequest.Qty)
          status = status | ProductionRequestStatus.Produced;
        else
          status = status | ProductionRequestStatus.InProduction;
      }

      if (status == ProductionRequestStatus.None)
        status = ProductionRequestStatus.NotAction;
      #endregion
      #region Edit ProductionRequest
      if (status != productionRequest.Status)
        EditProductionRequest(
                      productionRequest: productionRequest,
                      rowVersion: productionRequest.RowVersion,
                      status: status);
      #endregion

      return productionRequest;
    }
    #endregion
  }
}
