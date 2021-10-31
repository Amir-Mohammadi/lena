using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Models;
using lena.Domains.Enums;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Models.SaleManagement.OrderItem;
using lena.Services.Core.Exceptions;
using lena.Services.Common.Helpers;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Add
    public OrderItem AddOrderItem(
        int? productPackStuffId,
        short? productPackVersion,
        string description,
        double qty,
        int orderId,
        int stuffId,
        byte unitId,
        DateTime deliveryDate,
        DateTime requestDate,
        short? billOfMaterialVersion)
    {
      if (requestDate.Date > DateTime.Now.Date)
        // تاریخ سفارش نامعتبر می باشد، تاریخ سفارش نباید از تاریخ روز جاری بزرگتر باشد
        throw new InvalidOrderRequestDateException(requestDate.Date);
      var orderItem = repository.Create<OrderItem>();
      if (qty <= 0)
        throw new QtyInvalidException(qty);
      else
        orderItem.Qty = qty;
      orderItem.OrderId = orderId;
      orderItem.StuffId = stuffId;
      orderItem.UnitId = unitId;
      orderItem.DeliveryDate = deliveryDate;
      orderItem.RequestDate = requestDate;
      orderItem.BillOfMaterialVersion = billOfMaterialVersion;
      orderItem.OrderItemChangeStatus = OrderItemChangeStatus.InitialInsertion;
      orderItem.IsArchive = false;
      orderItem.Status = OrderItemStatus.Order;
      orderItem.ProductPackBillOfMaterialStuffId = productPackStuffId;
      orderItem.ProductPackBillOfMaterialVersion = productPackVersion;
      var retValue = App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: orderItem,
                    transactionBatch: null,
                    description: description);
      return retValue as OrderItem;
    }
    #endregion
    #region Edit
    public OrderItem EditOrderItem(
        int id,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<double> canceledQty = null,
        TValue<int> orderId = null,
        TValue<byte> unitId = null,
        TValue<DateTime> deliveryDate = null,
        TValue<DateTime> requestDate = null,
        TValue<short?> billOfMaterialVersion = null,
        TValue<OrderItemStatus> status = null,
        TValue<bool> hasChange = null,
        TValue<bool?> orderItemConfirmationConfirmed = null,
        TValue<bool?> orderItemHasActivated = null,
        TValue<DateTime?> orderItemConfirmationDateTime = null,
        TValue<bool?> checkOrderItemConfirmed = null,
        TValue<DateTime?> checkOrderItemDateTime = null,
        TValue<OrderItemChangeStatus> orderItemChangeStatus = null,
        TValue<bool> isArchive = null,
        TValue<bool> isDeleted = null
        )
    {
      var orderItem = GetOrderItem(id);
      return EditOrderItem(
                    orderItem: orderItem,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description,
                    qty: qty,
                    canceledQty: canceledQty,
                    orderId: orderId,
                    unitId: unitId,
                    deliveryDate: deliveryDate,
                    requestDate: requestDate,
                    billOfMaterialVersion: billOfMaterialVersion,
                    status: status,
                    hasChange: hasChange,
                    orderItemConfirmationConfirmed: orderItemConfirmationConfirmed,
                    orderItemHasActivated: orderItemHasActivated,
                    orderItemConfirmationDateTime: orderItemConfirmationDateTime,
                    checkOrderItemConfirmed: checkOrderItemConfirmed,
                    checkOrderItemDateTime: checkOrderItemDateTime,
                    orderItemChangeStatus: orderItemChangeStatus,
                    isArchive: isArchive,
                    isDeleted: isDeleted
                    );
    }
    public OrderItem EditOrderItem(
        OrderItem orderItem,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<double> canceledQty = null,
        TValue<int> orderId = null,
        TValue<byte> unitId = null,
        TValue<DateTime> deliveryDate = null,
        TValue<DateTime> requestDate = null,
        TValue<short?> billOfMaterialVersion = null,
        TValue<OrderItemStatus> status = null,
        TValue<bool> hasChange = null,
        TValue<bool?> orderItemConfirmationConfirmed = null,
        TValue<bool?> orderItemHasActivated = null,
        TValue<DateTime?> orderItemConfirmationDateTime = null,
        TValue<bool?> checkOrderItemConfirmed = null,
        TValue<DateTime?> checkOrderItemDateTime = null,
        TValue<OrderItemChangeStatus> orderItemChangeStatus = null,
        TValue<bool> isArchive = null,
        TValue<bool> isDeleted = null
        )
    {
      if (requestDate != null && requestDate.Value.Date > DateTime.Now.Date)
        throw new InternalException("تاریخ سفارش نامعتبر می باشد، تاریخ سفارش نباید از تاریخ روز جاری بزرگتر باشد!");
      if (qty != null)
        if (qty < 0)
          throw new QtyInvalidException(qty);
        else
          orderItem.Qty = qty;
      if (canceledQty != null)
      {
        if (canceledQty + orderItem.OrderItemSummary.SendedQty <= orderItem.Qty)
          orderItem.CanceledQty = canceledQty;
      }
      if (orderId != null)
        orderItem.OrderId = orderId;
      if (unitId != null)
        orderItem.UnitId = unitId;
      if (deliveryDate != null)
        orderItem.DeliveryDate = deliveryDate;
      if (requestDate != null)
        orderItem.RequestDate = requestDate;
      if (billOfMaterialVersion != null)
        orderItem.BillOfMaterialVersion = billOfMaterialVersion;
      if (status != null)
        orderItem.Status = status;
      if (hasChange != null)
        orderItem.HasChange = hasChange;
      if (orderItemConfirmationConfirmed != null)
        orderItem.OrderItemConfirmationConfirmed = orderItemConfirmationConfirmed;
      if (orderItemHasActivated != null)
        orderItem.OrderItemHasActivated = orderItemHasActivated;
      if (orderItemConfirmationDateTime != null)
        orderItem.OrderItemConfirmationDateTime = orderItemConfirmationDateTime;
      if (checkOrderItemConfirmed != null)
        orderItem.CheckOrderItemConfirmed = checkOrderItemConfirmed;
      if (checkOrderItemDateTime != null)
        orderItem.CheckOrderItemDateTime = checkOrderItemDateTime;
      if (orderItemChangeStatus != null)
        orderItem.OrderItemChangeStatus = orderItemChangeStatus;
      if (isArchive != null)
        orderItem.IsArchive = isArchive;
      if (isDeleted != null)
        orderItem.IsDelete = isDeleted;
      App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: orderItem,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description);
      return orderItem;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetOrderItems<TResult>(
        Expression<Func<OrderItem, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> customerId = null,
        TValue<double> qty = null,
        TValue<int> orderId = null,
        TValue<int> stuffId = null,
        TValue<int> unitId = null,
        TValue<DateTime> deliveryDate = null,
        TValue<DateTime> requestDate = null,
        TValue<DateTime> FromRequestDate = null,
        TValue<DateTime> ToRequestDate = null,
        TValue<DateTime> FromdeliveryDate = null,
        TValue<DateTime> TodeliveryDate = null,
        TValue<int?> billOfMaterialVersion = null,
        TValue<bool?> hasChange = null,
        TValue<OrderDocumentType> documentType = null,
        TValue<string> documentNumber = null,
        TValue<OrderItemStatus> status = null,
        TValue<OrderItemStatus[]> statuses = null,
        TValue<OrderItemStatus[]> notHasStatuses = null,
        TValue<int> productPackBillOfMaterialStuffId = null,
        TValue<int> productPackBillOfMaterialVersion = null,
        TValue<bool> isArchive = null)
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var orderItemQuery = baseQuery.OfType<OrderItem>();
      if (ids != null)
        orderItemQuery = orderItemQuery.Where(i => ids.Value.Contains(i.Id));
      if (qty != null)
        orderItemQuery = orderItemQuery.Where(i => Math.Abs(i.Qty - qty) < 0.000001);
      if (documentType != null)
        orderItemQuery = orderItemQuery.Where(i => i.Order.DocumentType == documentType);
      if (documentNumber != null)
        orderItemQuery = orderItemQuery.Where(i => i.Order.DocumentNumber == documentNumber);
      if (orderId != null)
        orderItemQuery = orderItemQuery.Where(i => i.OrderId == orderId);
      if (stuffId != null)
        orderItemQuery = orderItemQuery.Where(i => i.StuffId == stuffId);
      if (unitId != null)
        orderItemQuery = orderItemQuery.Where(i => i.UnitId == unitId);
      if (deliveryDate != null)
        orderItemQuery = orderItemQuery.Where(i => i.DeliveryDate == deliveryDate);
      if (requestDate != null)
        orderItemQuery = orderItemQuery.Where(i => i.RequestDate == requestDate);
      if (FromRequestDate != null)
        orderItemQuery = orderItemQuery.Where(i => i.RequestDate >= FromRequestDate);
      if (ToRequestDate != null)
        orderItemQuery = orderItemQuery.Where(i => i.RequestDate <= ToRequestDate);
      if (FromdeliveryDate != null)
        orderItemQuery = orderItemQuery.Where(i => i.DeliveryDate >= requestDate);
      if (TodeliveryDate != null)
        orderItemQuery = orderItemQuery.Where(i => i.DeliveryDate <= requestDate);
      if (billOfMaterialVersion != null)
        orderItemQuery = orderItemQuery.Where(i => i.BillOfMaterialVersion == billOfMaterialVersion);
      if (customerId != null)
        orderItemQuery = orderItemQuery.Where(i => i.Order.CustomerId == customerId);
      if (productPackBillOfMaterialStuffId != null)
        orderItemQuery = orderItemQuery.Where(i => i.ProductPackBillOfMaterialStuffId == productPackBillOfMaterialStuffId);
      if (productPackBillOfMaterialVersion != null)
        orderItemQuery = orderItemQuery.Where(i => i.ProductPackBillOfMaterialVersion == productPackBillOfMaterialVersion);
      if (isDelete != null)
        orderItemQuery = orderItemQuery.Where(i => i.IsDelete == isDelete);
      if (status != null)
        orderItemQuery = orderItemQuery.Where(i => i.Status.HasFlag(status));
      if (statuses != null)
      {
        var s = OrderItemStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        orderItemQuery = orderItemQuery.Where(i => (i.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = OrderItemStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        orderItemQuery = orderItemQuery.Where(i => (i.Status & s) == 0);
      }
      if (hasChange != null)
        orderItemQuery = orderItemQuery.Where(i => i.HasChange == hasChange);
      if (isArchive != null)
        orderItemQuery = orderItemQuery.Where(i => i.IsArchive == isArchive);
      return orderItemQuery.Select(selector);
    }
    #endregion
    #region Get
    public OrderItem GetOrderItem(int id) => GetOrderItem(selector: e => e, id: id);
    public TResult GetOrderItem<TResult>(Expression<Func<OrderItem, TResult>> selector, int id)
    {
      var orderItem = GetOrderItems(
                selector: selector,
                id: id).SingleOrDefault();
      if (orderItem == null)
        throw new OrderItemNotFoundException(id);
      return orderItem;
    }
    #endregion
    #region Delete
    public void DeleteOrderItem(int id)
    {
      var orderItem = GetOrderItem(id);
      if (orderItem.Status != (OrderItemStatus.Order | OrderItemStatus.SaleConfirmed | OrderItemStatus.SaleRejected))
        throw new OrderItemNotInDeletableStatusException(orderItem.Id);
      repository.Delete(orderItem);
    }
    #endregion
    #region Search
    public IQueryable<OrderItemResult> SearchOrderItem(IQueryable<OrderItemResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
        DateTime? fromDeliveryDate = null,
        DateTime? toDeliveryDate = null,
        DateTime? fromRequestDate = null,
        DateTime? toRequestDate = null,
        int? customerId = null
        )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
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
      var isFromDeliveryDateNull = fromDeliveryDate == null;
      var isToDeliveryDateNull = toDeliveryDate == null;
      var isFromRequestDateNull = fromRequestDate == null;
      var isToRequestDateNull = toRequestDate == null;
      var isCustomerIdNull = customerId == null;
      query = from orderItem in query
              where (isFromDeliveryDateNull || orderItem.DeliveryDate >= fromDeliveryDate) &&
                    (isToDeliveryDateNull || orderItem.DeliveryDate <= toDeliveryDate) &&
                    (isFromRequestDateNull || orderItem.RequestDate >= fromRequestDate) &&
                    (isToRequestDateNull || orderItem.RequestDate <= toRequestDate) &&
                    (isCustomerIdNull || orderItem.CustomerId == customerId)
              select orderItem;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<OrderItemResult> SortOrderItemResult(IQueryable<OrderItemResult> query, SortInput<OrderItemSortType> sort)
    {
      switch (sort.SortType)
      {
        case OrderItemSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case OrderItemSortType.StuffNoun:
          return query.OrderBy(a => a.StuffNoun, sort.SortOrder);
        case OrderItemSortType.BillOfMaterialVersion:
          return query.OrderBy(a => a.BillOfMaterialVersion, sort.SortOrder);
        case OrderItemSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case OrderItemSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case OrderItemSortType.RequestDate:
          return query.OrderBy(a => a.RequestDate, sort.SortOrder);
        case OrderItemSortType.DeliveryDate:
          return query.OrderBy(a => a.DeliveryDate, sort.SortOrder);
        case OrderItemSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case OrderItemSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case OrderItemSortType.CustomerName:
          return query.OrderBy(a => a.CustomerName, sort.SortOrder);
        case OrderItemSortType.CustomerCode:
          return query.OrderBy(a => a.CustomerCode, sort.SortOrder);
        case OrderItemSortType.OrderTypeName:
          return query.OrderBy(a => a.OrderTypeName, sort.SortOrder);
        case OrderItemSortType.ProducedQty:
          return query.OrderBy(a => a.ProducedQty, sort.SortOrder);
        case OrderItemSortType.SendedQty:
          return query.OrderBy(a => a.SendedQty, sort.SortOrder);
        case OrderItemSortType.SentQtyOtherCustomers:
          return query.OrderBy(a => a.SentToOtherCustomersQty, sort.SortOrder);
        case OrderItemSortType.NotPostedQty:
          return query.OrderBy(a => a.NotPostedQty, sort.SortOrder);
        case OrderItemSortType.NotPostedCanceledQty:
          return query.OrderBy(a => a.NotPostedCanceledQty, sort.SortOrder);
        case OrderItemSortType.BlockedQty:
          return query.OrderBy(a => a.BlockedQty, sort.SortOrder);
        case OrderItemSortType.BlockedQtyOtherCustomers:
          return query.OrderBy(a => a.BlockedQtyOtherCustomers, sort.SortOrder);
        case OrderItemSortType.PlannedQty:
          return query.OrderBy(a => a.PlannedQty, sort.SortOrder);
        case OrderItemSortType.PermissionQty:
          return query.OrderBy(a => a.PermissionQty, sort.SortOrder);
        case OrderItemSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case OrderItemSortType.HasChange:
          return query.OrderBy(a => a.HasChange, sort.SortOrder);
        case OrderItemSortType.OrderId:
          return query.OrderBy(a => a.OrderId, sort.SortOrder);
        case OrderItemSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case OrderItemSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case OrderItemSortType.OrderItemConfirmationConfirmed:
          return query.OrderBy(a => a.OrderItemConfirmationConfirmed, sort.SortOrder);
        case OrderItemSortType.OrderItemConfirmationDateTime:
          return query.OrderBy(a => a.OrderItemConfirmationDateTime, sort.SortOrder);
        case OrderItemSortType.CheckOrderItemConfirmed:
          return query.OrderBy(a => a.CheckOrderItemConfirmed, sort.SortOrder);
        case OrderItemSortType.CheckOrderItemDateTime:
          return query.OrderBy(a => a.CheckOrderItemDateTime, sort.SortOrder);
        case OrderItemSortType.OrderItemChangeStatus:
          return query.OrderBy(a => a.OrderItemChangeStatus, sort.SortOrder);
        case OrderItemSortType.Orderer:
          return query.OrderBy(a => a.Orderer, sort.SortOrder);
        case OrderItemSortType.IsArchive:
          return query.OrderBy(a => a.IsArchive, sort.SortOrder);
        case OrderItemSortType.OrderItemHasActivated:
          return query.OrderBy(a => a.OrderItemHasActivated, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    public Expression<Func<OrderItem, OrderItemResult>> ToOrderItemResult =>
        orderItem => new OrderItemResult
        {
          Id = orderItem.Id,
          Code = orderItem.Code,
          OrderDescription = orderItem.Order.Description,
          Description = orderItem.Description,
          StuffCode = orderItem.Stuff.Code,
          StuffName = orderItem.Stuff.Title,
          StuffNoun = orderItem.Stuff.Noun,
          StuffUnitTypeId = orderItem.Stuff.UnitTypeId,
          StuffId = orderItem.StuffId,
          DeliveryDate = orderItem.DeliveryDate,
          OrderId = orderItem.OrderId,
          CustomerId = orderItem.Order.CustomerId,
          CustomerName = orderItem.Order.Customer.Name,
          ContactResults = orderItem.Order.Customer.Contacts.Select(i =>
                  new ContactResult { Title = i.Title, ContactText = i.ContactText }),
          CustomerCode = orderItem.Order.Customer.Code,
          Orderer = orderItem.Order.Orderer,
          BillOfMaterialVersion = orderItem.BillOfMaterialVersion,
          BillOfMaterialTitle = orderItem.BillOfMaterial == null ? "" : orderItem.BillOfMaterial.Title,
          Qty = orderItem.Qty,
          CanceledQty = orderItem.CanceledQty,
          UnitId = orderItem.UnitId,
          UnitName = orderItem.Unit.Name,
          OrderTypeId = orderItem.Order.OrderTypeId,
          OrderTypeName = orderItem.Order.OrderType.Name,
          RequestDate = orderItem.RequestDate,
          DateTime = orderItem.DateTime,
          EmployeeFullName = orderItem.User.Employee.FirstName + " " + orderItem.User.Employee.LastName,
          ProducedQty = orderItem.OrderItemSummary.ProducedQty,
          PermissionQty = orderItem.OrderItemSummary.PermissionQty,
          BlockedQty = orderItem.OrderItemSummary.BlockedQty,
          BlockedQtyOtherCustomers = orderItem.OrderItemSummary.BlockedQtyOtherCustomers,
          SendedQty = orderItem.OrderItemSummary.SendedQty,
          SentToOtherCustomersQty = orderItem.OrderItemSummary.SentToOtherCustomersQty,
          NotPostedQty = (orderItem.Qty - orderItem.CanceledQty) - (orderItem.OrderItemSummary.SendedQty - orderItem.OrderItemSummary.SentToOtherCustomersQty),
          NotPostedCanceledQty = orderItem.CanceledQty - orderItem.OrderItemSummary.SentToOtherCustomersQty,
          PlannedQty = orderItem.OrderItemSummary.PlannedQty,
          Status = orderItem.Status,
          DocumentNumber = orderItem.Order.DocumentNumber,
          DocumentType = orderItem.Order.DocumentType,
          HasChange = orderItem.HasChange,
          OrderItemConfirmationConfirmed = orderItem.OrderItemConfirmationConfirmed,
          OrderItemHasActivated = !orderItem.Status.HasFlag(OrderItemStatus.Deactive),
          OrderItemConfirmationDateTime = orderItem.OrderItemConfirmationDateTime,
          CheckOrderItemConfirmed = orderItem.CheckOrderItemConfirmed,
          CheckOrderItemDateTime = orderItem.CheckOrderItemDateTime,
          OrderItemChangeStatus = orderItem.OrderItemChangeStatus,
          IsArchive = orderItem.IsArchive,
          ProductPackBillOfMaterialStuffId = orderItem.ProductPackBillOfMaterialStuffId,
          ProductPackBillOfMaterialStuffCode = orderItem.ProductPackBillOfMaterial == null ? "" : orderItem.ProductPackBillOfMaterial.Stuff.Code,
          ProductPackBillOfMaterialVersion = orderItem.ProductPackBillOfMaterialVersion,
          ProductPackBillOfMaterialIsPublished = orderItem.ProductPackBillOfMaterial.IsPublished,
          RowVersion = orderItem.RowVersion,
          OrderRowVersion = orderItem.Order.RowVersion
        };
    #endregion
    #region ToFullResult
    public Expression<Func<OrderItem, FullOrderItemResult>> ToFullOrderItemResult =
        orderItem => new FullOrderItemResult()
        {
          Id = orderItem.Id,
          Code = orderItem.Code,
          StuffCode = orderItem.Stuff.Code,
          OrderDescription = orderItem.Order.Description,
          Description = orderItem.Description,
          BillOfMaterialVersion = orderItem.BillOfMaterialVersion,
          BillOfMaterialTitle = orderItem.BillOfMaterial == null ? "" : orderItem.BillOfMaterial.Title,
          StuffName = orderItem.Stuff.Name,
          StuffNoun = orderItem.Stuff.Noun,
          StuffId = orderItem.StuffId,
          DeliveryDate = orderItem.DeliveryDate,
          OrderId = orderItem.OrderId,
          Qty = orderItem.Qty,
          UnitId = orderItem.UnitId,
          UnitName = orderItem.Unit.Name,
          RequestDate = orderItem.RequestDate,
          BlockedQty = orderItem.OrderItemSummary.BlockedQty,
          PermissionQty = orderItem.OrderItemSummary.PermissionQty,
          PlannedQty = orderItem.OrderItemSummary.PlannedQty,
          ProducedQty = orderItem.OrderItemSummary.ProducedQty,
          SendedQty = orderItem.OrderItemSummary.SendedQty,
          ProductPackBillOfMaterialStuffId = orderItem.ProductPackBillOfMaterialStuffId,
          ProductPackBillOfMaterialStuffCode = orderItem.ProductPackBillOfMaterial == null ? "" : orderItem.ProductPackBillOfMaterial.Stuff.Code,
          ProductPackBillOfMaterialStuffType = orderItem.ProductPackBillOfMaterial == null ? default(int) : orderItem.ProductPackBillOfMaterial.Stuff.StuffType,
          ProductPackBillOfMaterialVersion = orderItem.ProductPackBillOfMaterialVersion,
          Status = orderItem.Status,
          HasChange = orderItem.HasChange,
          RowVersion = orderItem.RowVersion,
          OrderRowVersion = orderItem.Order.RowVersion
        };
    #endregion
    #region ToOrderItemComboResult
    public Expression<Func<OrderItem, OrderItemComboResult>> ToCargoItemComboResult =>
        orderItem => new OrderItemComboResult
        {
          Id = orderItem.Id,
          Code = orderItem.Code,
          Name = orderItem.Code,
        };
    #endregion
    #region AddProcess
    public OrderItem AddOrderItemProcess(
        int? productPackStuffId,
        short? productPackVersion,
        string description,
        double qty,
        int orderId,
        int stuffId,
        byte unitId,
        DateTime deliveryDate,
        DateTime requestDate,
        short? billOfMaterialVersion)
    {
      #region  Add OrderItem
      var orderItem = AddOrderItem(
              productPackStuffId: productPackStuffId,
              productPackVersion: productPackVersion,
              description: description,
              qty: qty,
              orderId: orderId,
              stuffId: stuffId,
              unitId: unitId,
              deliveryDate: deliveryDate,
              requestDate: requestDate,
              billOfMaterialVersion: billOfMaterialVersion);
      #endregion
      #region AddOrderItemSummary
      AddOrderItemSummary(
              plannedQty: 0,
              producedQty: 0,
              blockedQty: 0,
              permissionQty: 0,
              preparingSendingQty: 0,
              sendedQty: 0,
              orderItemId: orderItem.Id
          );
      #endregion
      #region  Add OrderItemChangeRequest
      var orderItemChangeRequest = AddOrderItemChangeRequest(
              description: description,
              qty: qty,
              orderItemId: orderItem.Id,
              unitId: unitId,
              deliveryDate: deliveryDate,
              requestDate: requestDate,
              orderItemChangeStatus: OrderItemChangeStatus.InitialInsertion,
              isActive: true);
      #endregion
      #region Get ProjectHeader
      var stuff = App.Internals.SaleManagement.GetStuff(stuffId);
      if (stuff.ProjectHeader == null)
        throw new StuffProjectHeaderIsNullException(stuff.Code);
      var projectHeaderId = stuff.ProjectHeader.Id;
      #endregion
      #region Add Project
      var project = App.Internals.ProjectManagement.AddProject(
              project: null,
              name: "سفارش " + orderItem.Stuff.Name,
              description: "به سفارش " + orderItem.Order.Customer.Name,
              color: "",
              departmentId: (int)Departments.Sales,
              estimatedTime: 10800,
              isCommit: false,
              projectHeaderId: projectHeaderId,
              baseEntityId: orderItem.Id);
      #endregion
      #region Add ProjectStep
      var projectStep = App.Internals.ProjectManagement.AddProjectStep(
              projectStep: null,
              name: "تکمیل سفارش " + orderItem.Stuff.Name,
              description: "به سفارش " + orderItem.Order.Customer.Name,
              color: "",
              departmentId: (int)Departments.Sales,
              estimatedTime: 10800,
              isCommit: false,
              projectId: project.Id,
              baseEntityId: orderItem.Id);
      #endregion
      #region Add ProjectWork
      var projectWork = App.Internals.ProjectManagement.AddProjectWork(
              projectWork: null,
              name: "تایید سفارش " + orderItem.Stuff.Name,
              description: "به سفارش " + orderItem.Order.Customer.Name,
              color: "",
              departmentId: (int)Departments.Sales,
              estimatedTime: 180,
              isCommit: false,
              projectStepId: projectStep.Id,
              baseEntityId: orderItem.Id
          );
      #endregion
      #region Add ProjectWorkItem
      //check projectWork not null
      if (projectWork != null)
      {
        var projectWorkItem = App.Internals.ProjectManagement.AddProjectWorkItem(
                      projectWorkItem: null,
                      name: $"تایید سفارش شماره {orderItem.Code} {orderItem.Stuff.Name}",
                      description: $"بررسی نهایی و تایید سفارش کالای {orderItem.Stuff.Noun}",
                      color: "",
                      departmentId: (int)Departments.Sales,
                      estimatedTime: 10800,
                      isCommit: false,
                      scrumTaskTypeId: (int)ScrumTaskTypes.ConfirmOrderItem,
                      userId: null,
                      spentTime: 0,
                      remainedTime: 0,
                      scrumTaskStep: ScrumTaskStep.ToDo,
                      projectWorkId: projectWork.Id,
                      baseEntityId: orderItem.Id);
      }
      #endregion
      return orderItem;
    }
    #endregion
    #region EditProcess
    public OrderItem EditOrderItemProcess(
        int id,
        byte[] rowVersion,
        string description,
        double qty,
        double canceledQty,
        byte unitId,
        DateTime deliveryDate,
        DateTime requestDate,
        short? billOfMaterialVersion
       )
    {
      var orderItem = GetOrderItem(id: id);
      var isActive = true;
      if (orderItem.Status.HasFlag(OrderItemStatus.Deactive))
        isActive = false;
      return EditOrderItemProcess(
                    orderItem: orderItem,
                    rowVersion: rowVersion,
                    description: description,
                    qty: qty,
                    canceledQty: canceledQty,
                    unitId: unitId,
                    deliveryDate: deliveryDate,
                    requestDate: requestDate,
                    billOfMaterialVersion: billOfMaterialVersion,
                    isActive: isActive);
    }
    public OrderItem EditOrderItemProcess(
        OrderItem orderItem,
        byte[] rowVersion,
        string description,
        double qty,
        double canceledQty,
        byte unitId,
        DateTime deliveryDate,
        DateTime requestDate,
        short? billOfMaterialVersion,
        bool isActive)
    {
      if (orderItem.HasChange)
        throw new OrderItemHasChangeException(orderItemId: orderItem.Id);
      var addRequest = orderItem.Qty != qty ||
            orderItem.UnitId != unitId ||
            orderItem.DeliveryDate != deliveryDate;
      if (addRequest == false ||
                orderItem.Status == OrderItemStatus.Order ||
                orderItem.Status == OrderItemStatus.SaleRejected)
      {
        #region Edit OrderItem
        EditOrderItem(
                orderItem: orderItem,
                rowVersion: rowVersion,
                description: description,
                qty: qty,
                canceledQty: canceledQty,
                unitId: unitId,
                deliveryDate: deliveryDate,
                requestDate: requestDate,
                billOfMaterialVersion: billOfMaterialVersion);
        #endregion
      }
      else
      {
        #region Edit OrderItem
        EditOrderItem(
                orderItem: orderItem,
                rowVersion: rowVersion,
                description: description,
                requestDate: requestDate,
                billOfMaterialVersion: billOfMaterialVersion,
                canceledQty: canceledQty);
        #endregion
      }
      #region AddOrderItemChangeRequest
      AddOrderItemChangeRequestProcess(
              description: description,
              qty: qty,
              orderItemId: orderItem.Id,
              unitId: unitId,
              deliveryDate: deliveryDate,
              requestDate: requestDate,
              isActive: isActive);
      #endregion
      return orderItem;
    }
    #endregion
    #region Reset
    public OrderItem ResetOrderItemProcess(
        int id,
        byte[] rowVersion,
        double? qty,
        byte? unitId,
        DateTime? deliveryDate,
        DateTime? requestDate)
    {
      var orderItem = GetOrderItem(id: id);
      return ResetOrderItemProcess(
                    orderItem: orderItem,
                    rowVersion: rowVersion,
                    qty: qty,
                    unitId: unitId,
                    deliveryDate: deliveryDate,
                    requestDate: requestDate);
    }
    public OrderItem ResetOrderItemProcess(
        OrderItem orderItem,
        byte[] rowVersion,
        double? qty,
        byte? unitId,
        DateTime? deliveryDate,
        DateTime? requestDate)
    {
      #region Edit OrderItem
      EditOrderItem(
              orderItem: orderItem,
              rowVersion: rowVersion,
              qty: qty,
              unitId: unitId,
              deliveryDate: deliveryDate,
              requestDate: requestDate,
              hasChange: false);
      #endregion
      #region ResetOrderItemStatus
      ResetOrderItemStatus(orderItem: orderItem);
      #endregion
      #region GetLastOrderItemConfirmation
      var orderItemConfirmation = GetOrderItemConfirmations(
              selector: e => e,
              isDelete: false,
              orderItemId: orderItem.Id)


          .OrderByDescending(i => i.DateTime)
          .FirstOrDefault();
      #endregion
      if (orderItemConfirmation != null)
      {
        if (!orderItemConfirmation.Confirmed)
        {
          #region Get Done ConfirmOrderItemTask
          var doneConfirmOrderItemTask = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                  baseEntityId: orderItem.Id,
                  scrumTaskType: ScrumTaskTypes.ConfirmOrderItem);
          #endregion
          #region Add ConfirmOrderItem Task
          //check projectWork not null
          if (doneConfirmOrderItemTask != null)
          {
            var projectWorkItem = App.Internals.ProjectManagement.AddProjectWorkItem(
                          projectWorkItem: null,
                          name: $"تایید سفارش شماره {orderItem.Code} {orderItem.Stuff.Name}",
                          description: $" بررسی مشتری از نظر حساب مالی و تایید سفارش کالای {orderItem.Stuff.Noun}",
                          color: "",
                          departmentId: (int)Departments.Sales,
                          estimatedTime: 10800,
                          isCommit: false,
                          scrumTaskTypeId: (int)ScrumTaskTypes.ConfirmOrderItem,
                          userId: null,
                          spentTime: 0,
                          remainedTime: 0,
                          scrumTaskStep: ScrumTaskStep.ToDo,
                          projectWorkId: doneConfirmOrderItemTask.ScrumBackLogId,
                          baseEntityId: orderItem.Id);
          }
          #endregion
        }
        else
        {
          #region Add CheckOrderItemTask if need
          if (orderItem.Qty > orderItem.OrderItemSummary.PlannedQty)
          {
            if (orderItemConfirmation != null)
            {
              #region Get CheckOrderItemTask
              var checkOrderItemTask = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                      baseEntityId: orderItemConfirmation.Id,
                      scrumTaskType: ScrumTaskTypes.CheckOrderItem);
              #endregion
              if (checkOrderItemTask == null)
              {
                #region Get Done CheckOrderItemTask
                checkOrderItemTask = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                        baseEntityId: orderItemConfirmation.Id,
                        scrumTaskType: ScrumTaskTypes.CheckOrderItem);
                #endregion
                #region Add CheckOrderItemTask
                //check projectWork not null
                if (checkOrderItemTask != null)
                {
                  App.Internals.ProjectManagement.AddProjectWorkItem(
                                projectWorkItem: null,
                                name: $"بررسی سفارش {orderItem.Code} {orderItem.Stuff.Name}",
                                description:
                                "بررسی سفارش از نظر مقدار موجود در انبار و ثبت رزرو  و درخواست تولید",
                                color: "",
                                departmentId: (int)Departments.Planning,
                                estimatedTime: 10800,
                                isCommit: false,
                                scrumTaskTypeId: (int)ScrumTaskTypes.CheckOrderItem,
                                userId: null,
                                spentTime: 0,
                                remainedTime: 0,
                                scrumTaskStep: ScrumTaskStep.ToDo,
                                projectWorkId: checkOrderItemTask.ScrumBackLogId,
                                baseEntityId: orderItemConfirmation.Id);
                }
                #endregion
              }
            }
          }
          #endregion
          #region GetSalePlanTransactions
          var transactionPlans = App.Internals.WarehouseManagement.GetTransactionPlans(
                  selector: e => e,
                  transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportSalePlan.Id,
                  isDelete: false,
                  baseEntityId: orderItemConfirmation.Id);
          #endregion
          #region Remove TransactionPlans
          foreach (var transactionPlan in transactionPlans)
          {
            #region Remove TransactionPlan
            App.Internals.WarehouseManagement.RemoveBaseTransactionProcess(
                baseTransaction: transactionPlan,
                rowVersion: transactionPlan.RowVersion);
            #endregion
          }
          #endregion
          #region Add TransactionPlan For OrderItem
          App.Internals.WarehouseManagement.AddTransactionPlanProcess(
                  transactionBatchId: orderItemConfirmation.TransactionBatch.Id,
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
        }
      }
      return orderItem;
    }
    #endregion
    #region RemoveProcess
    public void RemoveOrderItemProcess(
        int id,
        byte[] rowVersion)
    {
      var orderItem = App.Internals.SaleManagement.GetOrderItem(id: id);
      RemoveOrderItemProcess(
                orderItem: orderItem,
                    rowVersion: rowVersion);
    }
    public void RemoveOrderItemProcess(
        OrderItem orderItem,
        byte[] rowVersion)
    {
      if (orderItem.Status == OrderItemStatus.Order)
      {
        #region TransactionBatch
        var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
        #endregion
        #region Remove OrderItem
        App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                transactionBatchId: transactionBatch.Id,
                baseEntity: orderItem,
                rowVersion: rowVersion);
        #endregion
      }
      #region OrderItemChangeRequestProcess
      var isActive = true;
      if (orderItem.Status.HasFlag(OrderItemStatus.Deactive))
        isActive = false;
      AddOrderItemChangeRequestProcess(
                    description: "",
                    qty: 0,
                    orderItemId: orderItem.Id,
                    unitId: orderItem.UnitId,
                    deliveryDate: orderItem.DeliveryDate,
                    requestDate: orderItem.RequestDate,
                    isActive: isActive);
      #endregion
    }
    #endregion
    #region ResetOrderItemStatus
    public OrderItem ResetOrderItemStatus(int id)
    {
      var orderItem = GetOrderItem(id: id); ; return ResetOrderItemStatus(orderItem: orderItem);
    }
    public OrderItem ResetOrderItemStatus(OrderItem orderItem)
    {
      var orderItemUnitConversionRatio = orderItem.Unit.ConversionRatio;
      #region ResetOrderItemSummary
      var orderItemSummary = ResetOrderItemSummaryByOrderItemId(orderItemId: orderItem.Id);
      #endregion
      #region Define Status
      var status = OrderItemStatus.None;
      #region GetLastOrderItemConfirmation
      var orderItemConfirmation = GetOrderItemConfirmations(
          selector: e => e,
          isDelete: false,
          orderItemId: orderItem.Id)


      .OrderByDescending(i => i.DateTime)
      .FirstOrDefault();
      #endregion
      if (orderItemConfirmation == null)
        status = OrderItemStatus.Order;
      else
      {
        status = orderItemConfirmation.Confirmed
                    ? OrderItemStatus.SaleConfirmed
                    : OrderItemStatus.SaleRejected;
        #region GetLastCheckOrderItem
        var checkOrderItem = GetCheckOrderItems(
                selector: e => e,
                isDelete: false,
                orderItemConfirmationId: orderItemConfirmation.Id)


            .OrderByDescending(i => i.DateTime)
            .FirstOrDefault();
        #endregion
        if (checkOrderItem != null)
          status = checkOrderItem.Confirmed
                    ? OrderItemStatus.PlanningConfirmed
                    : OrderItemStatus.PlanningRejected;
      }
      if (orderItemSummary.PlannedQty > 0)
      {
        status = status | OrderItemStatus.Planning;
      }
      //if(orderItemSummary.SendedQty == orderItem.Qty)
      //{
      //    status = status | OrderItemStatus.Completed;
      //}
      if (orderItemSummary.ProducedQty > 0)
      {
        status = status | OrderItemStatus.InProduction;
      }
      if (orderItemSummary.BlockedQty > 0)
      {
        if (orderItemSummary.BlockedQty >= orderItemSummary.OrderItem.Qty)
          status = status | OrderItemStatus.Blocked;
        else
          status = status | OrderItemStatus.Blocking;
      }
      if (orderItemSummary.SendedQty > 0)
      {
        if (orderItemSummary.SendedQty >= orderItemSummary.OrderItem.Qty)
          status = status | OrderItemStatus.Sent;
        else
          status = status | OrderItemStatus.Sending;
      }
      if (orderItem.Status.HasFlag(OrderItemStatus.Deactive))
        status = status | OrderItemStatus.Deactive;
      if (orderItem.Status.HasFlag(OrderItemStatus.Completed))
        status = status | OrderItemStatus.Completed;
      #endregion
      #region Edit OrderItem
      if (orderItem.Status != status)
        EditOrderItem(
                      orderItem: orderItem,
                      rowVersion: orderItem.RowVersion,
                      status: status);
      #endregion
      return orderItem;
    }
    #endregion
    #region Add OrderItemConfirmationRequest
    public OrderItem AddOrderItemConfirmationRequest(
        int orderItemId,
        byte[] rowVersion)
    {
      #region GetOrderItem
      var orderItem = GetOrderItem(id: orderItemId);
      #endregion
      #region  AddOrderItemProcess
      AddOrderItemConfirmationRequest(
              orderItem: orderItem,
              rowVersion: rowVersion);
      #endregion
      return orderItem;
    }
    public OrderItem AddOrderItemConfirmationRequest(
            OrderItem orderItem,
            byte[] rowVersion)
    {
      #region CheckOrderItemStatus
      if (orderItem.Status != OrderItemStatus.SaleRejected)
        throw new OrderItemNotInRejectedStatusException(orderItem.Id);
      #endregion
      #region Get Done OrderItemConfiramtionTask
      var orderItemConfirmationTask = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
              baseEntityId: orderItem.Id,
              scrumTaskType: ScrumTaskTypes.ConfirmOrderItem);
      #endregion
      #region Add ProjectWorkItem
      //check projectWork not null
      if (orderItemConfirmationTask != null)
      {
        var projectWorkItem = App.Internals.ProjectManagement.AddProjectWorkItem(
                      projectWorkItem: null,
                      name: $"تایید سفارش شماره {orderItem.Code} {orderItem.Stuff.Name}",
                      description: $" بررسی نهایی و تایید سفارش کالای {orderItem.Stuff.Noun}",
                      color: "",
                      departmentId: (int)Departments.Sales,
                      estimatedTime: 10800,
                      isCommit: false,
                      scrumTaskTypeId: (int)ScrumTaskTypes.ConfirmOrderItem,
                      userId: null,
                      spentTime: 0,
                      remainedTime: 0,
                      scrumTaskStep: ScrumTaskStep.ToDo,
                      projectWorkId: orderItemConfirmationTask.ScrumBackLogId,
                      baseEntityId: orderItem.Id);
      }
      #endregion
      #region EditOrderItem
      EditOrderItem(orderItem: orderItem,
              rowVersion: rowVersion,
              status: OrderItemStatus.Order,
              orderItemConfirmationConfirmed: new TValue<bool?>(null));
      #endregion
      return orderItem;
    }
    #endregion
    public OrderItem DeleteOrderItem(int orderItemId, byte[] rowVersion)
    {
      var orderItem = GetOrderItem(id: orderItemId);
      DeleteOrderItem(orderItem: orderItem,
                rowVersion: rowVersion);
      return orderItem;
    }
    #region ArchiveOrderItem
    public OrderItem ArchiveOrderItem(int orderItemId, byte[] rowVersion)
    {
      var orderItem = GetOrderItem(id: orderItemId);
      ArchiveOrderItem(orderItem: orderItem,
                rowVersion: rowVersion);
      return orderItem;
    }
    public OrderItem DeleteOrderItem(
       OrderItem orderItem,
       byte[] rowVersion)
    {
      if (orderItem.Status != OrderItemStatus.Order && orderItem.Status != OrderItemStatus.SaleConfirmed && orderItem.Status != OrderItemStatus.SaleRejected)
        throw new OrderItemNotInDeletableStatusException(orderItem.Id);
      EditOrderItem(orderItem: orderItem,
                isDelete: true,
                rowVersion: rowVersion);
      return orderItem;
    }
    public OrderItem ArchiveOrderItem(
        OrderItem orderItem,
        byte[] rowVersion)
    {
      EditOrderItem(orderItem: orderItem,
                isArchive: true,
                rowVersion: rowVersion);
      return orderItem;
    }
    #endregion
    #region RestoreOrderItem
    public OrderItem RestoreOrderItem(int orderItemId, byte[] rowVersion)
    {
      var orderItem = GetOrderItem(id: orderItemId);
      RestoreOrderItem(orderItem: orderItem,
                rowVersion: rowVersion);
      return orderItem;
    }
    public OrderItem RestoreOrderItem(
        OrderItem orderItem,
        byte[] rowVersion)
    {
      EditOrderItem(orderItem: orderItem,
                isArchive: false,
                rowVersion: rowVersion);
      return orderItem;
    }
    #endregion
    #region DeactiveOrderItem
    public OrderItem DeactiveOrderItem(int orderItemId)
    {
      var orderItem = GetOrderItem(id: orderItemId); ; DeactiveOrderItem(orderItem: orderItem);
      return orderItem;
    }
    public OrderItem CompletedOrderItem(int orderItemId)
    {
      var orderItem = GetOrderItem(id: orderItemId); ; CompletedOrderItem(orderItem: orderItem);
      return orderItem;
    }
    public OrderItem DeactiveOrderItem(
        OrderItem orderItem)
    {
      AddOrderItemChangeRequestProcess(
                   description: orderItem.Description,
                   qty: orderItem.Qty,
                   orderItemId: orderItem.Id,
                   unitId: orderItem.UnitId,
                   deliveryDate: orderItem.DeliveryDate,
                   requestDate: orderItem.RequestDate,
                   isActive: false);
      return orderItem;
    }
    public OrderItem CompletedOrderItem(
        OrderItem orderItem)
    {
      orderItem.Status = OrderItemStatus.Completed;
      repository.Update(entity: orderItem, rowVersion: orderItem.RowVersion);
      return orderItem;
    }
    #endregion
    #region ActiveOrderItem
    public OrderItem ActiveOrderItem(int orderItemId)
    {
      var orderItem = GetOrderItem(id: orderItemId); ; ActiveOrderItem(orderItem: orderItem);
      return orderItem;
    }
    public OrderItem ActiveOrderItem(
        OrderItem orderItem)
    {
      AddOrderItemChangeRequestProcess(
                   description: orderItem.Description,
                   qty: orderItem.Qty,
                   orderItemId: orderItem.Id,
                   unitId: orderItem.UnitId,
                   deliveryDate: orderItem.DeliveryDate,
                   requestDate: orderItem.RequestDate,
                   isActive: true);
      return orderItem;
    }
    #endregion
    #region Set OrderItems Activity Status
    public void SetOrderItemsActivityStatus(SetOrderItemActivityStatusInput[] inputs)
    {
      foreach (var item in inputs)
      {
        if (item.Activated == true)
        {
          ActiveOrderItem(orderItemId: item.OrderItemId);
        }
        if (item.Activated == false)
        {
          DeactiveOrderItem(orderItemId: item.OrderItemId);
        };
      }
    }
    #endregion
  }
}