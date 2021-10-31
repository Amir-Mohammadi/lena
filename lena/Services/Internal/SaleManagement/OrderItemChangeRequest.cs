using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Domains.Enums;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.StaticData;
using lena.Models.Common;
using lena.Models.SaleManagement.OrderItemChangeRequest;
using lena.Models.UserManagement.SecurityAction;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Add
    public OrderItemChangeRequest AddOrderItemChangeRequest(
        string description,
        double qty,
        int orderItemId,
        byte unitId,
        DateTime deliveryDate,
        DateTime requestDate,
        OrderItemChangeStatus orderItemChangeStatus,
        bool isActive)
    {

      var orderItemChangeRequest = repository.Create<OrderItemChangeRequest>();
      orderItemChangeRequest.OrderItemId = orderItemId;
      orderItemChangeRequest.Qty = qty;
      orderItemChangeRequest.UnitId = unitId;
      orderItemChangeRequest.DeliveryDate = deliveryDate;
      orderItemChangeRequest.RequestDate = requestDate;
      orderItemChangeRequest.OrderItemChangeStatus = orderItemChangeStatus;
      orderItemChangeRequest.IsActive = isActive;
      var retValue = App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: orderItemChangeRequest,
                    transactionBatch: null,
                    description: description);
      return retValue as OrderItemChangeRequest;
    }
    #endregion
    #region AddProcess
    public OrderItemChangeRequest AddOrderItemChangeRequestProcess(
        string description,
        double qty,
        int orderItemId,
        byte unitId,
        DateTime deliveryDate,
        DateTime requestDate,
        bool isActive)
    {

      #region GetOrderItem
      var orderItem = GetOrderItem(id: orderItemId);
      #endregion
      #region  Add OrderItemChangeRequest

      var status = OrderItemChangeStatus.NotAction;
      if (orderItem.Status == OrderItemStatus.Order || orderItem.Status == OrderItemStatus.SaleRejected)
        status = OrderItemChangeStatus.DirectConfirmed;

      var orderItemChangeRequest = AddOrderItemChangeRequest(
                    description: description,
                    qty: qty,
                    orderItemId: orderItemId,
                    unitId: unitId,
                    deliveryDate: deliveryDate,
                    requestDate: requestDate,
                    orderItemChangeStatus: status,
                    isActive: isActive);
      #endregion
      #region Reset OrderItemChangeStatus
      EditOrderItem(orderItem: orderItem,
              rowVersion: orderItem.RowVersion,
              orderItemChangeStatus: status);
      #endregion
      //todo orderitemstatus check
      if (status == OrderItemChangeStatus.NotAction)
      {
        #region Edit OrderItem Set HasChange
        EditOrderItem(orderItem: orderItem,
                rowVersion: orderItem.RowVersion,
                hasChange: true);
        #endregion
        #region Get ConfirmOrderItem Task
        var confirmOrderItemTask = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                baseEntityId: orderItemId,
                scrumTaskType: ScrumTaskTypes.ConfirmOrderItem);
        #endregion
        #region Add ProjectStep
        var projectWork = App.Internals.ProjectManagement.AddProjectWork(
                projectWork: null,
                name: "اصلاحیه سفارش " + orderItem.Stuff.Name,
                description: "اصلاحیه سفارش " + orderItem.Order.Customer.Name,
                color: "",
                departmentId: (int)Departments.Sales,
                estimatedTime: 10800,
                isCommit: false,
                projectStepId: confirmOrderItemTask.ScrumBackLog.ScrumSprintId,
                baseEntityId: null);
        #endregion
        #region Add ProjectWorkItem
        //check projectWork not null
        if (projectWork != null)
        {
          var projectWorkItem = App.Internals.ProjectManagement.AddProjectWorkItem(
                        projectWorkItem: null,
                        name: $"تایید اصلاحیه سفارش {orderItemChangeRequest.Code}",
                        description: "اصلاحیه سفارش را بررسی نمایید",
                        color: "",
                        departmentId: (int)Departments.Sales,
                        estimatedTime: 10800,
                        isCommit: false,
                        scrumTaskTypeId: (int)ScrumTaskTypes.OrderItemChangeSaleConfirmation,
                        userId: null,
                        spentTime: 0,
                        remainedTime: 0,
                        scrumTaskStep: ScrumTaskStep.ToDo,
                        projectWorkId: projectWork.Id,
                        baseEntityId: orderItemChangeRequest.Id);
        }

        #endregion
        #region Check Confirm Permission
        var parameter = new ActionParameterInput() { Key = "CurrentChangeStatus", Value = "0" };
        var checkPermissionResult = App.Internals.UserManagement.CheckPermission(
                      actionName: StaticActionName.SaleOrderItemChangeConfirmationAction,
                      actionParameters: new ActionParameterInput[] { parameter });
        #endregion
        #region Confirm ChangeRequest If Allowed
        if (checkPermissionResult.AccessType == AccessType.Allowed)
        {
          AddOrderItemChangeConfirmationProcess(
                        description: null,
                        confirmed: true,
                        orderItemChangeRequestId: orderItemChangeRequest.Id,
                        currentChangeStatus: orderItemChangeRequest.OrderItemChangeStatus,
                        orderItemChangeRequestRowVersion: orderItemChangeRequest.RowVersion);

        }
        #endregion
      }
      #region AddOrderItemConfirmationRequest if Rejected
      if (orderItem.Status == OrderItemStatus.SaleRejected)
      {
        AddOrderItemConfirmationRequest(
                      orderItem: orderItem,
                      rowVersion: orderItem.RowVersion);
      }
      #endregion




      return orderItemChangeRequest;
    }
    #endregion
    #region Edit
    public OrderItemChangeRequest EditOrderItemChangeRequest(
        int id,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<int> orderItemId = null,
        TValue<byte> unitId = null,
        TValue<DateTime> deliveryDate = null,
        TValue<DateTime> requestDate = null,
        TValue<OrderItemChangeStatus> orderItemChangeStatus = null)
    {

      var orderItemChangeRequest = GetOrderItemChangeRequest(id);
      return EditOrderItemChangeRequest(
                    orderItemChangeRequest: orderItemChangeRequest,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description,
                    qty: qty,
                    orderItemId: orderItemId,
                    unitId: unitId,
                    deliveryDate: deliveryDate,
                    requestDate: requestDate,
                    orderItemChangeStatus: orderItemChangeStatus);
    }
    public OrderItemChangeRequest EditOrderItemChangeRequest(
        OrderItemChangeRequest orderItemChangeRequest,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<int> orderItemId = null,
        TValue<byte> unitId = null,
        TValue<DateTime> deliveryDate = null,
        TValue<DateTime> requestDate = null,
        TValue<OrderItemChangeStatus> orderItemChangeStatus = null)
    {

      if (qty != null)
        orderItemChangeRequest.Qty = qty;
      if (orderItemId != null)
        orderItemChangeRequest.OrderItemId = orderItemId;
      if (unitId != null)
        orderItemChangeRequest.UnitId = unitId;
      if (deliveryDate != null)
        orderItemChangeRequest.DeliveryDate = deliveryDate;
      if (requestDate != null)
        orderItemChangeRequest.RequestDate = requestDate;
      if (orderItemChangeStatus != null)
        orderItemChangeRequest.OrderItemChangeStatus = orderItemChangeStatus;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: orderItemChangeRequest,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description);
      return retValue as OrderItemChangeRequest;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetOrderItemChangeRequests<TResult>(
        Expression<Func<OrderItemChangeRequest, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<int> orderItemId = null,
        TValue<int> stuffId = null,
        TValue<int> unitId = null,
        TValue<DateTime> deliveryDate = null,
        TValue<DateTime> requestDate = null,
        TValue<OrderItemChangeStatus> orderItemChangeStatus = null,
        TValue<OrderItemChangeStatus[]> orderItemChangeStatuses = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var orderItemChangeRequestQuery = baseQuery.OfType<OrderItemChangeRequest>();
      if (qty != null)
        orderItemChangeRequestQuery = orderItemChangeRequestQuery.Where(i => Math.Abs(i.Qty - qty) < 0.000001);
      if (orderItemId != null)
        orderItemChangeRequestQuery = orderItemChangeRequestQuery.Where(i => i.OrderItemId == orderItemId);
      if (unitId != null)
        orderItemChangeRequestQuery = orderItemChangeRequestQuery.Where(i => i.UnitId == unitId);
      if (deliveryDate != null)
        orderItemChangeRequestQuery = orderItemChangeRequestQuery.Where(i => i.DeliveryDate == deliveryDate);
      if (requestDate != null)
        orderItemChangeRequestQuery = orderItemChangeRequestQuery.Where(i => i.RequestDate == requestDate);
      if (orderItemChangeStatus != null)
        orderItemChangeRequestQuery = orderItemChangeRequestQuery.Where(i => i.OrderItemChangeStatus == orderItemChangeStatus);
      if (orderItemChangeStatuses != null)
        orderItemChangeRequestQuery = orderItemChangeRequestQuery.Where(i => orderItemChangeStatuses.Value.Contains(i.OrderItemChangeStatus));
      return orderItemChangeRequestQuery.Select(selector);
    }
    #endregion
    #region Get
    public OrderItemChangeRequest GetOrderItemChangeRequest(int id) => GetOrderItemChangeRequest(selector: e => e, id: id);
    public TResult GetOrderItemChangeRequest<TResult>(
        Expression<Func<OrderItemChangeRequest, TResult>> selector,
        int id)
    {

      var orderItemChangeRequest = GetOrderItemChangeRequests(
                selector: selector,
                id: id).SingleOrDefault();
      if (orderItemChangeRequest == null)
        throw new OrderItemChangeRequestNotFoundException(id);
      return orderItemChangeRequest;
    }
    public OrderItemChangeRequest GetActiveOrderItemChangeRequest(int orderItemId) => GetActiveOrderItemChangeRequest(selector: e => e, orderItemId: orderItemId);
    public TResult GetActiveOrderItemChangeRequest<TResult>(
        Expression<Func<OrderItemChangeRequest, TResult>> selector,
        int orderItemId)
    {

      var orderItemChangeRequest = GetOrderItemChangeRequests(
                    selector: selector,
                    orderItemId: orderItemId,
                    isDelete: false,
                    orderItemChangeStatuses: new[]
                    {
                            OrderItemChangeStatus.NotAction ,
                            OrderItemChangeStatus.PlanRejected ,
                            OrderItemChangeStatus.SaleConfirmed
                })


                .SingleOrDefault();
      return orderItemChangeRequest;
    }
    #endregion
    //#region EditProcess
    //public OrderItemChangeRequest EditOrderItemChangeRequestProcess(
    //    int id,
    //    byte[] rowVersion,
    //    string description,
    //    double qty,
    //    byte UnitId,
    //    DateTime deliveryDate,
    //    DateTime requestDate,
    //    OrderItemChangeStatus orderItemChangeStatus)
    //{
    //    
    //        var orderItemChangeRequest = GetOrderItemChangeRequest(id);
    //        return EditOrderItemChangeRequest(
    //                orderItemChangeRequest: orderItemChangeRequest,
    //                rowVersion: rowVersion,
    //                description: description,
    //                qty: qty,
    //                unitId: unitId,
    //                deliveryDate: deliveryDate,
    //                requestDate: requestDate,
    //                orderItemChangeStatus: orderItemChangeStatus)
    //            
    //;
    //    });
    //}
    //public OrderItemChangeRequest EditOrderItemChangeRequestProcess(
    //    OrderItemChangeRequest orderItemChangeRequest,
    //    byte[] rowVersion,
    //    string description,
    //    double qty,
    //    byte UnitId,
    //    DateTime deliveryDate,
    //    DateTime requestDate,
    //    OrderItemChangeStatus orderItemChangeStatus)
    //{
    //    
    //        #region Edit OrderItemChangeRequest
    //        EditOrderItemChangeRequest(
    //                orderItemChangeRequest: orderItemChangeRequest,
    //                rowVersion: rowVersion,
    //                description: description,
    //                qty: qty,
    //                unitId: unitId,
    //                deliveryDate: deliveryDate,
    //                requestDate: requestDate,
    //                orderItemChangeStatus: orderItemChangeStatus)
    //            
    //;
    //        #endregion
    //        return orderItemChangeRequest;
    //    });
    //}
    //#endregion
    //#region RemoveProcess
    //public void RemoveOrderItemChangeRequestProcess(
    //    int id,
    //    byte[] rowVersion)
    //{
    //    
    //        var orderItemChangeRequest = App.Internals.SaleManagement.GetOrderItemChangeRequest(id: id)
    //            
    //;
    //        RemoveOrderItemChangeRequestProcess(
    //            orderItemChangeRequest: orderItemChangeRequest,
    //                rowVersion: rowVersion)
    //            
    //;
    //    });
    //}
    //public void RemoveOrderItemChangeRequestProcess(
    //    OrderItemChangeRequest orderItemChangeRequest,
    //    byte[] rowVersion)
    //{
    //    
    //        #region TransactionBatch
    //        var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch()
    //            
    //;
    //        #endregion
    //        #region Remove OrderItemChangeRequest 
    //        App.Internals.ApplicationBase.RemoveBaseEntityProcess(
    //               transactionBatchId: transactionBatch.Id,
    //               baseEntity: orderItemChangeRequest,
    //               rowVersion: rowVersion)
    //           
    //;
    //        #endregion
    //        #region Remove OrderItemChangeRequestConfirmation
    //        var orderItemChangeRequestConfirmations = GetOrderItemChangeRequestConfirmations(
    //                selector: e => e,
    //                orderItemChangeRequestId: orderItemChangeRequest.Id)
    //            
    //;
    //        foreach (var orderItemChangeRequestConfirmation in orderItemChangeRequestConfirmations)
    //            if (orderItemChangeRequestConfirmation.IsDelete == false)
    //                RemoveOrderItemChangeRequestConfirmationProcess(
    //                        transactionBatchId: transactionBatch.Id,
    //                        orderItemChangeRequestConfirmation: orderItemChangeRequestConfirmation,
    //                        rowVersion: orderItemChangeRequestConfirmation.RowVersion)
    //                    
    //;
    //        #endregion
    //    });
    //}
    //#endregion
    #region Search
    public IQueryable<OrderItemChangeRequestResult> SearchOrderItemChangeRequest(IQueryable<OrderItemChangeRequestResult> query,
        string searchText)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.Code.Contains(searchText) ||
                item.Orderer.Contains(searchText) ||
                item.StuffCode.Contains(searchText) ||
                item.StuffName.Contains(searchText) ||
                item.Description.Contains(searchText) ||
                item.CustomerName.Contains(searchText) ||
                item.CustomerCode.Contains(searchText) ||
                item.OrderTypeName.Contains(searchText) ||
                item.EmployeeFullName.Contains(searchText)
                select item;
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<OrderItemChangeRequestResult> SortOrderItemChangeRequestResult(IQueryable<OrderItemChangeRequestResult> query, SortInput<OrderItemChangeRequestSortType> sort)
    {
      switch (sort.SortType)
      {
        case OrderItemChangeRequestSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case OrderItemChangeRequestSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case OrderItemChangeRequestSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case OrderItemChangeRequestSortType.RequestDate:
          return query.OrderBy(a => a.RequestDate, sort.SortOrder);
        case OrderItemChangeRequestSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case OrderItemChangeRequestSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case OrderItemChangeRequestSortType.DeliveryDate:
          return query.OrderBy(a => a.DeliveryDate, sort.SortOrder);
        case OrderItemChangeRequestSortType.OrderItemChangeStatus:
          return query.OrderBy(a => a.OrderItemChangeStatus, sort.SortOrder);
        case OrderItemChangeRequestSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    public Expression<Func<OrderItemChangeRequest, OrderItemChangeRequestResult>> ToOrderItemChangeRequestResult =>
        orderItemChangeRequest => new OrderItemChangeRequestResult
        {
          Id = orderItemChangeRequest.Id,
          Code = orderItemChangeRequest.Code,
          OrderItemId = orderItemChangeRequest.OrderItemId,
          Description = orderItemChangeRequest.Description,
          Qty = orderItemChangeRequest.Qty,
          UnitId = orderItemChangeRequest.UnitId,
          UnitName = orderItemChangeRequest.Unit.Name,
          RequestDate = orderItemChangeRequest.RequestDate,
          DeliveryDate = orderItemChangeRequest.DeliveryDate,
          DateTime = orderItemChangeRequest.DateTime,
          OrderItemQty = orderItemChangeRequest.OrderItem.Qty,
          OrderItemUnitId = orderItemChangeRequest.OrderItem.UnitId,
          OrderItemUnitName = orderItemChangeRequest.OrderItem.Unit.Name,
          OrderItemRequestDate = orderItemChangeRequest.OrderItem.RequestDate,
          OrderItemDeliveryDate = orderItemChangeRequest.OrderItem.DeliveryDate,
          OrderItemCode = orderItemChangeRequest.OrderItem.Code,
          OrderItemDescription = orderItemChangeRequest.OrderItem.Description,
          OrderId = orderItemChangeRequest.OrderItemId,
          StuffCode = orderItemChangeRequest.OrderItem.Stuff.Code,
          StuffName = orderItemChangeRequest.OrderItem.Stuff.Title,
          StuffId = orderItemChangeRequest.OrderItem.StuffId,
          CustomerId = orderItemChangeRequest.OrderItem.Order.CustomerId,
          CustomerName = orderItemChangeRequest.OrderItem.Order.Customer.Name,
          CustomerCode = orderItemChangeRequest.OrderItem.Order.Customer.Code,
          Orderer = orderItemChangeRequest.OrderItem.Order.Orderer,
          OrderItemChangeStatus = orderItemChangeRequest.OrderItemChangeStatus,
          OrderTypeId = orderItemChangeRequest.OrderItem.Order.OrderTypeId,
          OrderTypeName = orderItemChangeRequest.OrderItem.Order.OrderType.Name,
          EmployeeFullName = orderItemChangeRequest.User.Employee.FirstName + " " + orderItemChangeRequest.User.Employee.LastName,
          BillOfMaterialVersion = orderItemChangeRequest.OrderItem.BillOfMaterialVersion,
          BillOfMaterialTitle = orderItemChangeRequest.OrderItem.BillOfMaterial.Title,
          IsActive = orderItemChangeRequest.IsActive,
          RowVersion = orderItemChangeRequest.RowVersion,
        };
    #endregion
    #region ToFullResult
    public Expression<Func<OrderItemChangeRequest, FullOrderItemChangeRequestResult>> ToFullOrderItemChangeRequestResult =
        orderItemChangeRequest => new FullOrderItemChangeRequestResult()
        {
          Id = orderItemChangeRequest.Id,
          Code = orderItemChangeRequest.Code,
          OrderItemId = orderItemChangeRequest.OrderItemId,
          Description = orderItemChangeRequest.Description,
          Qty = orderItemChangeRequest.Qty,
          UnitId = orderItemChangeRequest.UnitId,
          UnitName = orderItemChangeRequest.Unit.Name,
          RequestDate = orderItemChangeRequest.RequestDate,
          DeliveryDate = orderItemChangeRequest.DeliveryDate,
          DateTime = orderItemChangeRequest.DateTime,
          OrderItemQty = orderItemChangeRequest.OrderItem.Qty,
          OrderItemUnitId = orderItemChangeRequest.OrderItem.UnitId,
          OrderItemUnitName = orderItemChangeRequest.OrderItem.Unit.Name,
          OrderItemRequestDate = orderItemChangeRequest.OrderItem.RequestDate,
          OrderItemDeliveryDate = orderItemChangeRequest.OrderItem.DeliveryDate,
          OrderItemCode = orderItemChangeRequest.OrderItem.Code,
          OrderItemDescription = orderItemChangeRequest.OrderItem.Description,
          OrderId = orderItemChangeRequest.OrderItemId,
          StuffCode = orderItemChangeRequest.OrderItem.Stuff.Code,
          StuffName = orderItemChangeRequest.OrderItem.Stuff.Title,
          StuffId = orderItemChangeRequest.OrderItem.StuffId,
          CustomerId = orderItemChangeRequest.OrderItem.Order.CustomerId,
          CustomerName = orderItemChangeRequest.OrderItem.Order.Customer.Name,
          CustomerCode = orderItemChangeRequest.OrderItem.Order.Customer.Code,
          Orderer = orderItemChangeRequest.OrderItem.Order.Orderer,
          OrderItemChangeStatus = orderItemChangeRequest.OrderItemChangeStatus,
          OrderTypeId = orderItemChangeRequest.OrderItem.Order.OrderTypeId,
          OrderTypeName = orderItemChangeRequest.OrderItem.Order.OrderType.Name,
          EmployeeFullName = orderItemChangeRequest.User.Employee.FirstName + " " + orderItemChangeRequest.User.Employee.LastName,
          OrderItemChangeConfirmations = orderItemChangeRequest.OrderItemChangeConfirmations.AsQueryable().Select(App.Internals.SaleManagement.ToOrderItemChangeConfirmationResult),
          BillOfMaterialVersion = orderItemChangeRequest.OrderItem.BillOfMaterialVersion,
          BillOfMaterialTitle = orderItemChangeRequest.OrderItem.BillOfMaterial.Title,
          IsActive = orderItemChangeRequest.IsActive,
          OrderItemIsActive = !orderItemChangeRequest.OrderItem.Status.HasFlag(OrderItemStatus.Deactive),
          RowVersion = orderItemChangeRequest.RowVersion,
        };
    #endregion
  }
}
