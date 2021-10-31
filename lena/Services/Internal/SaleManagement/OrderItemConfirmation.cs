using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.SaleManagement.OrderItemConfirmation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Gets
    public IQueryable<TResult> GetOrderItemConfirmations<TResult>(
        Expression<Func<OrderItemConfirmation, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> orderItemId = null,
        TValue<bool> confirmed = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<OrderItemConfirmation>();
      if (confirmed != null)
        query = query.Where(i => i.Confirmed == confirmed);
      if (orderItemId != null)
        query = query.Where(i => i.OrderItemId == orderItemId);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public OrderItemConfirmation GetOrderItemConfirmation(int id) => GetOrderItemConfirmation(e => e, id: id);
    public TResult GetOrderItemConfirmation<TResult>(
        Expression<Func<OrderItemConfirmation, TResult>> selector,
        int id)
    {

      var orderItemConfirmation = GetOrderItemConfirmations(
                selector: selector,
                id: id)


                .FirstOrDefault();
      if (orderItemConfirmation == null)
        throw new OrderItemConfirmationNotFoundException(id);
      return orderItemConfirmation;
    }
    #endregion
    #region Get With OrderItemId
    public OrderItemConfirmation GetOrderItemConfirmationWithOrderItemId(int orderItemId) => GetOrderItemConfirmationWithOrderItemId(e => e, orderItemId: orderItemId);
    public TResult GetOrderItemConfirmationWithOrderItemId<TResult>(
        Expression<Func<OrderItemConfirmation, TResult>> selector,
        int orderItemId)
    {

      var orderItemConfirmation = GetOrderItemConfirmations(
                selector: selector,
                orderItemId: orderItemId,
                confirmed: true)


                .FirstOrDefault();
      return orderItemConfirmation;
    }
    #endregion
    #region Add
    public OrderItemConfirmation AddOrderItemConfirmation(
        TransactionBatch transactionBatch,
        int orderItemId,
        bool confirmed,
        string description)
    {

      var orderItemConfirmation = repository.Create<OrderItemConfirmation>();
      orderItemConfirmation.Confirmed = confirmed;
      orderItemConfirmation.OrderItemId = orderItemId;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: orderItemConfirmation,
                    transactionBatch: transactionBatch,
                    description: description);
      return orderItemConfirmation;
    }
    #endregion
    #region AddProcess
    public OrderItemConfirmation AddOrderItemConfirmationProcess(
        int orderItemId,
        byte[] orderItemRowVersion,
        bool confirmed,
        string description)
    {

      var orderItem = GetOrderItem(orderItemId);
      return AddOrderItemConfirmationProcess(
                    orderItem: orderItem,
                    orderItemRowVersion: orderItemRowVersion,
                    confirmed: confirmed,
                    description: description);
    }
    public OrderItemConfirmation AddOrderItemConfirmationProcess(
        OrderItem orderItem,
        byte[] orderItemRowVersion,
        bool confirmed,
        string description)
    {

      #region Add TransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region Add OrderItemConfirmation
      var orderItemConfirmation = AddOrderItemConfirmation(
              transactionBatch: transactionBatch,
              orderItemId: orderItem.Id,
              confirmed: confirmed,
              description: description);
      #endregion
      #region Edit OrderItem
      var editOrderItem = EditOrderItem(orderItem: orderItem,
                          rowVersion: orderItemRowVersion,
                          orderItemConfirmationConfirmed: orderItemConfirmation.Confirmed,
                          orderItemConfirmationDateTime: orderItemConfirmation.DateTime);
      #endregion
      #region Reset OrderItemStatus
      ResetOrderItemStatus(orderItem: orderItem);
      #endregion
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
          baseEntityId: orderItem.Id,
          scrumTaskType: ScrumTaskTypes.ConfirmOrderItem);
      #endregion
      #region DoneTask   
      //check projectWork not null
      if (projectWorkItem != null)
      {
        App.Internals.ScrumManagement.DoneScrumTask(
                      scrumTask: projectWorkItem,
                      rowVersion: projectWorkItem.RowVersion);
      }
      #endregion
      if (confirmed)
      {
        #region Add TransactionPlan For OrderItem
        var transactionPlan = App.Internals.WarehouseManagement.AddTransactionPlanProcess(
                transactionBatchId: transactionBatch.Id,
                effectDateTime: orderItem.DeliveryDate,
                stuffId: orderItem.StuffId,
                billOfMaterialVersion: orderItem.BillOfMaterialVersion,
                stuffSerialCode: null,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportSalePlan.Id,
                amount: orderItem.Qty,
                unitId: orderItem.UnitId,
                description: orderItem.Description,
                isEstimated: false,
                referenceTransaction: null);
        #endregion
        //check projectWork not null
        if (projectWorkItem != null)
        {
          #region Add PlanningProjectWork

          var planningProjectWork = App.Internals.ProjectManagement.AddProjectWork(
                  projectWork: null,
                  name: "برنامه ریزی",
                  description: "",
                  color: "",
                  departmentId: (int)Departments.Planning,
                  estimatedTime: 1800,
                  isCommit: false,
                  projectStepId: projectWorkItem.ScrumBackLog.ScrumSprintId,
                  baseEntityId: orderItemConfirmation.Id);

          #endregion

          #region Add ProjectWorkItem for CheckOrderItem

          App.Internals.ProjectManagement.AddProjectWorkItem(
                  projectWorkItem: null,
                  name:
                  $"بررسی سفارش {orderItemConfirmation.OrderItem.Code} {orderItemConfirmation.OrderItem.Stuff.Name}",
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
                  baseEntityId: orderItemConfirmation.Id);

          #endregion
        }
      }
      else
      {
        var employee = orderItemConfirmation.User.Employee;
        var name = (employee == null) ? orderItemConfirmation.User.UserName : employee.FirstName + " " + employee.LastName;
        App.Internals.Notification.NotifyToUser(
                  userId: orderItem.UserId,
                  title: $"{ orderItem.Code } رد سفارش",
                  description: $"سفارش {orderItem.Code} برای محصول `{ orderItem.Stuff.Code} - { orderItem.Stuff.Name}` توسط {name} رد شد.",
                  scrumEntityId: null
                  );
      }
      return orderItemConfirmation;
    }
    #endregion
    #region ToResult
    public Expression<Func<OrderItemConfirmation, OrderItemConfirmationResult>> ToOrderItemConfirmationResult =
        orderItemConfirmation => new OrderItemConfirmationResult
        {
          Id = orderItemConfirmation.Id,
          Code = orderItemConfirmation.Code,
          DateTime = orderItemConfirmation.DateTime,
          UserId = orderItemConfirmation.UserId,
          UserName = orderItemConfirmation.User.UserName,
          EmployeeCode = orderItemConfirmation.User.Employee.Code,
          EmployeeName = orderItemConfirmation.User.Employee.FirstName + " " + orderItemConfirmation.User.Employee.LastName,
          IsDelete = orderItemConfirmation.IsDelete,
          Confirmed = orderItemConfirmation.Confirmed,
          OrderItemId = orderItemConfirmation.OrderItemId,
          OrderItemCode = orderItemConfirmation.OrderItem.Code,
          Description = orderItemConfirmation.Description,
          Qty = orderItemConfirmation.OrderItem.Qty,
          PlannedQty = orderItemConfirmation.OrderItem.OrderItemSummary.PlannedQty,
          BlockedQty = orderItemConfirmation.OrderItem.OrderItemSummary.BlockedQty,
          ProducedQty = orderItemConfirmation.OrderItem.OrderItemSummary.ProducedQty,
          PermissionQty = orderItemConfirmation.OrderItem.OrderItemSummary.PermissionQty,
          SendedQty = orderItemConfirmation.OrderItem.OrderItemSummary.SendedQty,
          UnitId = orderItemConfirmation.OrderItem.UnitId,
          UnitName = orderItemConfirmation.OrderItem.Unit.Name,
          CustomerId = orderItemConfirmation.OrderItem.Order.CustomerId,
          CustomerCode = orderItemConfirmation.OrderItem.Order.Customer.Code,
          CustomerName = orderItemConfirmation.OrderItem.Order.Customer.Name,
          OrderTypeId = orderItemConfirmation.OrderItem.Order.OrderTypeId,
          OrderTypeName = orderItemConfirmation.OrderItem.Order.OrderType.Name,
          StuffId = orderItemConfirmation.OrderItem.StuffId,
          StuffCode = orderItemConfirmation.OrderItem.Stuff.Code,
          StuffName = orderItemConfirmation.OrderItem.Stuff.Name,
          StuffNoun = orderItemConfirmation.OrderItem.Stuff.Noun,
          StuffType = orderItemConfirmation.OrderItem.Stuff.StuffType,
          BillOfMaterialVersion = orderItemConfirmation.OrderItem.BillOfMaterialVersion,
          BillOfMaterialTitle = orderItemConfirmation.OrderItem.BillOfMaterial.Title,
          RequestDate = orderItemConfirmation.OrderItem.RequestDate,
          DeliveryDate = orderItemConfirmation.OrderItem.DeliveryDate,
          OrderItemDescription = orderItemConfirmation.OrderItem.Description,
          OrderDocumentType = orderItemConfirmation.OrderItem.Order.DocumentType,
          OrderDocumentNumber = orderItemConfirmation.OrderItem.Order.DocumentNumber,
          RowVersion = orderItemConfirmation.RowVersion
        };
    #endregion
    #region RemoveProcess
    public OrderItemConfirmation RemoveOrderItemConfirmationProcess(
        int? transactionBatchId,
        int id,
        byte[] rowVersion)
    {

      var orderItemConfirmation = GetOrderItemConfirmation(id: id);
      return RemoveOrderItemConfirmationProcess(
                    transactionBatchId: transactionBatchId,
                    orderItemConfirmation: orderItemConfirmation,
                    rowVersion: rowVersion);
    }
    public OrderItemConfirmation RemoveOrderItemConfirmationProcess(
        int? transactionBatchId,
        OrderItemConfirmation orderItemConfirmation,
        byte[] rowVersion)
    {

      #region Remove OrderItemConfirmation
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
              transactionBatchId: transactionBatchId,
              baseEntity: orderItemConfirmation,
              rowVersion: rowVersion);
      #endregion
      return orderItemConfirmation;
    }
    #endregion
  }
}
