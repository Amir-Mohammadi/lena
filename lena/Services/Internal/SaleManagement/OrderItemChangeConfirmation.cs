using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using lena.Domains.Enums;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.SaleManagement.OrderItemChangeConfirmations;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Add
    public OrderItemChangeConfirmation AddOrderItemChangeConfirmation(
        string description,
        bool confirmed,
        int orderItemChangeRequestId,
        OrderItemChangeStatus currentChangeStatus)

    {

      var orderItemchangeRequest = GetOrderItemChangeRequest(orderItemChangeRequestId);
      if (orderItemchangeRequest.OrderItemChangeStatus != currentChangeStatus)
        throw new StatusNotEqualException(currentChangeStatus);
      var orderItemChangeConfirmation = repository.Create<OrderItemChangeConfirmation>();
      orderItemChangeConfirmation.Confirmed = confirmed;
      orderItemChangeConfirmation.OrderItemChangeRequestId = orderItemChangeRequestId;
      var retValue = App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: orderItemChangeConfirmation,
                    transactionBatch: null,
                    description: description);
      return retValue as OrderItemChangeConfirmation;
    }
    #endregion
    #region Edit
    public OrderItemChangeConfirmation EditOrderItemChangeConfirmation(
        int id,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<bool> confirmed = null,
        TValue<int> orderItemChangeRequestId = null)
    {

      var orderItemChangeConfirmation = GetOrderItemChangeConfirmation(id);
      return EditOrderItemChangeConfirmation(
                    orderItemChangeConfirmation: orderItemChangeConfirmation,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description,
                    confirmed: confirmed,
                    orderItemChangeRequestId: orderItemChangeRequestId);
    }
    public OrderItemChangeConfirmation EditOrderItemChangeConfirmation(
        OrderItemChangeConfirmation orderItemChangeConfirmation,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<bool> confirmed = null,
        TValue<int> orderItemChangeRequestId = null)
    {

      if (confirmed != null)
        orderItemChangeConfirmation.Confirmed = confirmed;
      if (orderItemChangeRequestId != null)
        orderItemChangeConfirmation.OrderItemChangeRequestId = orderItemChangeRequestId;
      App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: orderItemChangeConfirmation,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description);
      return orderItemChangeConfirmation;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetOrderItemChangeConfirmations<TResult>(
        Expression<Func<OrderItemChangeConfirmation, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<bool> confirmed = null,
        TValue<int> orderItemChangeRequestId = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var orderItemChangeConfirmationQuery = baseQuery.OfType<OrderItemChangeConfirmation>();
      if (confirmed != null)
        orderItemChangeConfirmationQuery = orderItemChangeConfirmationQuery.Where(i => i.Confirmed == confirmed);
      if (orderItemChangeRequestId != null)
        orderItemChangeConfirmationQuery = orderItemChangeConfirmationQuery.Where(i => i.OrderItemChangeRequestId == orderItemChangeRequestId);
      return orderItemChangeConfirmationQuery.Select(selector);
    }
    #endregion
    #region Get
    public OrderItemChangeConfirmation GetOrderItemChangeConfirmation(int id) => GetOrderItemChangeConfirmation(selector: e => e, id: id);
    public TResult GetOrderItemChangeConfirmation<TResult>(
        Expression<Func<OrderItemChangeConfirmation, TResult>> selector,
        int id)
    {

      var orderItemChangeConfirmation = GetOrderItemChangeConfirmations(
                selector: selector,
                id: id).SingleOrDefault();
      if (orderItemChangeConfirmation == null)
        throw new OrderItemChangeConfirmationNotFoundException(id);
      return orderItemChangeConfirmation;
    }
    #endregion
    #region Delete
    public void DeleteOrderItemChangeConfirmation(int id)
    {

      var orderItemChangeConfirmation = GetOrderItemChangeConfirmation(id);
      repository.Delete(orderItemChangeConfirmation);
    }
    #endregion
    #region Search
    public IQueryable<OrderItemChangeConfirmationResult> SearchOrderItemChangeConfirmation(
        IQueryable<OrderItemChangeConfirmationResult> query,
        string searchText)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.Code.Contains(searchText) ||
                item.Description.Contains(searchText) ||
                item.EmployeeFullName.Contains(searchText)
                select item;
      return query;


    }
    #endregion
    #region Sort
    public IOrderedQueryable<OrderItemChangeConfirmationResult> SortOrderItemChangeConfirmationResult(
        IQueryable<OrderItemChangeConfirmationResult> query, SortInput<OrderItemChangeConfirmationSortType> sort)
    {
      switch (sort.SortType)
      {
        case OrderItemChangeConfirmationSortType.OrderItemChangeRequestId:
          return query.OrderBy(a => a.OrderItemChangeRequestId, sort.SortOrder);
        case OrderItemChangeConfirmationSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case OrderItemChangeConfirmationSortType.Confirmed:
          return query.OrderBy(a => a.Confirmed, sort.SortOrder);
        case OrderItemChangeConfirmationSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case OrderItemChangeConfirmationSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    public Expression<Func<OrderItemChangeConfirmation, OrderItemChangeConfirmationResult>> ToOrderItemChangeConfirmationResult =>
        orderItemChangeConfirmation => new OrderItemChangeConfirmationResult
        {
          Id = orderItemChangeConfirmation.Id,
          Code = orderItemChangeConfirmation.Code,
          Description = orderItemChangeConfirmation.Description,
          Confirmed = orderItemChangeConfirmation.Confirmed,
          OrderItemChangeRequestId = orderItemChangeConfirmation.OrderItemChangeRequestId,
          EmployeeFullName = orderItemChangeConfirmation.User.Employee.FirstName + " " + orderItemChangeConfirmation.User.Employee.LastName,
          DateTime = orderItemChangeConfirmation.DateTime,
          RowVersion = orderItemChangeConfirmation.RowVersion
        };
    #endregion
    #region AddProcess
    public OrderItemChangeConfirmation AddOrderItemChangeConfirmationProcess(
        string description,
        bool confirmed,
        int orderItemChangeRequestId,
        OrderItemChangeStatus currentChangeStatus,
        byte[] orderItemChangeRequestRowVersion)
    {

      #region  Add OrderItemChangeConfirmation
      var orderItemChangeConfirmation = AddOrderItemChangeConfirmation(
              description: description,
              confirmed: confirmed,
              orderItemChangeRequestId: orderItemChangeRequestId,
              currentChangeStatus: currentChangeStatus);
      #endregion
      #region GetOrderItemChangeRequest
      var orderItemChangeRequest = GetOrderItemChangeRequest(id: orderItemChangeRequestId);
      #endregion
      #region Define scrumTaskType and nextOrderItemChangeStatus
      ScrumTaskTypes? scrumTaskType = null;
      OrderItemChangeStatus? nextOrderItemChangeStatus = null;
      if (orderItemChangeRequest.OrderItemChangeStatus == OrderItemChangeStatus.NotAction ||
                orderItemChangeRequest.OrderItemChangeStatus == OrderItemChangeStatus.PlanRejected
            )
      {
        scrumTaskType = ScrumTaskTypes.OrderItemChangeSaleConfirmation;
        nextOrderItemChangeStatus = confirmed ? OrderItemChangeStatus.SaleConfirmed : OrderItemChangeStatus.SaleRejected;

      }
      else if (orderItemChangeRequest.OrderItemChangeStatus == OrderItemChangeStatus.SaleConfirmed)
      {
        scrumTaskType = ScrumTaskTypes.OrderItemChangePlanConfirmation;
        nextOrderItemChangeStatus = confirmed ? OrderItemChangeStatus.PlanConfirmed : OrderItemChangeStatus.PlanRejected;
      }
      #endregion
      #region Edit OrderItemChangeRequest Status
      EditOrderItemChangeRequest(
              orderItemChangeRequest: orderItemChangeRequest,
              rowVersion: orderItemChangeRequestRowVersion,
              orderItemChangeStatus: nextOrderItemChangeStatus);
      #endregion

      #region GetOrderItem
      var orderItem = GetOrderItem(id: orderItemChangeRequest.OrderItemId);
      #endregion

      #region Set OrderItemStatus

      if (orderItemChangeRequest.IsActive && confirmed)
      {
        orderItem.Status &= ~OrderItemStatus.Deactive;
      }
      if (!orderItemChangeRequest.IsActive && confirmed)
      {
        orderItem.Status |= OrderItemStatus.Deactive;
      }

      #endregion


      #region EditProcess




      orderItem = EditOrderItem(orderItem: orderItem,
          rowVersion: orderItem.RowVersion,
          orderItemChangeStatus: nextOrderItemChangeStatus);

      #endregion
      #region EditOrderItem Set HasChange false if SaleRejected or PlanConfirmed
      #region Define OrderItem HasChange and EditValues
      var hasChange = nextOrderItemChangeStatus != OrderItemChangeStatus.SaleRejected;
      double? editQty = null;
      byte? editUnitId = null;
      DateTime? editDeliveryDate = null;
      DateTime? editRequestDate = null;
      if (nextOrderItemChangeStatus == OrderItemChangeStatus.PlanConfirmed)
      {
        #region GetPlannedQty
        var plannedQty = GetOrderItem(
            selector: i => i.OrderItemSummary.PlannedQty * i.Unit.ConversionRatio,
            id: orderItem.Id);
        #endregion
        #region Get New qty
        var newQty = GetOrderItemChangeRequest(
                selector: e => e.Qty * e.Unit.ConversionRatio,
                id: orderItemChangeRequestId);
        #endregion
        if (plannedQty > newQty)
          throw new ExtraAmountPlannedException();
        else
        {
          hasChange = false;
          editQty = orderItemChangeRequest.Qty;
          editUnitId = orderItemChangeRequest.UnitId;
          editDeliveryDate = orderItemChangeRequest.DeliveryDate;
          editRequestDate = orderItemChangeRequest.RequestDate;
        }
      }
      #endregion
      #region EditOrderItem
      if (!hasChange)
      {
        ResetOrderItemProcess(
                        orderItem: orderItem,
                        rowVersion: orderItem.RowVersion,
                        qty: editQty,
                        unitId: editUnitId,
                        deliveryDate: editDeliveryDate,
                        requestDate: editRequestDate);
      }
      #endregion
      #endregion-
      #region GetProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                  baseEntityId: orderItemChangeRequest.Id,
                  scrumTaskType: scrumTaskType.Value);
      #endregion
      #region DoneTask
      if (projectWorkItem != null)
      {
        App.Internals.ScrumManagement.DoneScrumTask(
                      scrumTask: projectWorkItem,
                      rowVersion: projectWorkItem.RowVersion);
      }
      #endregion
      #region Add ProjectWorkItem
      if (orderItemChangeRequest.OrderItemChangeStatus == OrderItemChangeStatus.PlanRejected)
      {
        #region Add OrderItemChangeSaleConfirmation Task
        //check projectWork not null
        if (projectWorkItem != null)
        {
          projectWorkItem = App.Internals.ProjectManagement.AddProjectWorkItem(
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
                        projectWorkId: projectWorkItem.ScrumBackLogId,
                        baseEntityId: orderItemChangeRequest.Id);
        }

        #endregion
      }
      else if (orderItemChangeRequest.OrderItemChangeStatus == OrderItemChangeStatus.SaleConfirmed)
      {
        #region Add OrderItemChangePlanConfirmation Task
        //check projectWork not null
        if (projectWorkItem != null)
        {
          projectWorkItem = App.Internals.ProjectManagement.AddProjectWorkItem(
                        projectWorkItem: null,
                        name: $"تایید اصلاحیه سفارش {orderItemChangeRequest.Code}",
                        description: "اصلاحیه سفارش را بررسی نمایید",
                        color: "",
                        departmentId: (int)Departments.Planning,
                        estimatedTime: 10800,
                        isCommit: false,
                        scrumTaskTypeId: (int)ScrumTaskTypes.OrderItemChangePlanConfirmation,
                        userId: null,
                        spentTime: 0,
                        remainedTime: 0,
                        scrumTaskStep: ScrumTaskStep.ToDo,
                        projectWorkId: projectWorkItem.ScrumBackLogId,
                        baseEntityId: orderItemChangeRequest.Id);
        }

        #endregion
      }
      #endregion
      return orderItemChangeConfirmation;
    }
    #endregion

  }
}
