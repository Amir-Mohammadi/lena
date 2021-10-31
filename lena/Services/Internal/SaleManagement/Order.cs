using System;
//using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.SaleManagement.Order;
using lena.Models.SaleManagement.OrderDocument;
using lena.Models.SaleManagement.OrderItem;
using lena.Models.SaleManagement.PaymentDueDate;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region AddProcess
    public Order AddOrderProcess(
        int customerId,
        int orderTypeId,
        string orderer,
        string description,
        string documentNumber,
        double? totalAmount,
        OrderDocumentType documentType,
        AddOrderItemInput[] orderItems,
        AddPaymentDueDateInput[] PaymentDueDates,
        AddOrderDocumentInput[] addOrderDocumentInputs)
    {
      #region Add Order
      var order = AddOrder(customerId: customerId,
              orderTypeId: orderTypeId,
              orderer: orderer,
              description: description,
              documentNumber: documentNumber,
              documentType: documentType,
              totalAmount: totalAmount
              );
      #endregion
      #region OrderDocument
      if (addOrderDocumentInputs.Any())
      {
        foreach (var item in addOrderDocumentInputs)
        {
          var UploadFile = Core.App.Providers.Session.GetAs<UploadFileData>(item.FileKey);
          var document = App.Internals.ApplicationBase.AddDocument(name: UploadFile.FileName, fileStream: UploadFile.FileData);
          AddOrderDocument(orderId: order.Id, documentId: document.Id, description: item.Description);
        }
      }
      #endregion
      #region Add OrderItems
      foreach (var item in orderItems)
      {
        #region Add OrderItem
        AddOrderItemProcess(
                productPackStuffId: item.ProductPackStuffId,
                productPackVersion: item.ProductPackVersion,
                description: item.Description,
                qty: item.Qty,
                orderId: order.Id,
                stuffId: item.StuffId,
                unitId: item.UnitId,
                deliveryDate: item.DeliveryDate,
                requestDate: item.RequestDate,
                billOfMaterialVersion: item.BillOfMaterialVersion);
        #endregion
      }
      #endregion
      #region Add PaymentDueDate
      foreach (var item in PaymentDueDates)
      {
        var paymentDueDate = AddPaymentDueDate(
                  orderId: order.Id,
                  paymentTypeId: item.PaymentTypeId,
                  paymentDate: item.PaymentDate,
                  amount: item.Amount
                 );
      }
      #endregion
      return order;
    }
    #endregion
    #region EditProcess
    public Order EditOrderProcess(
        byte[] rowVersion, int id,
        TValue<int> customerId = null,
        TValue<int> orderTypeId = null,
        TValue<string> orderer = null,
        TValue<string> description = null,
        TValue<string> documentNumber = null,
        TValue<OrderDocumentType> documentType = null,
        TValue<bool> isDeleted = null,
        AddOrderItemInput[] addOrderItems = null,
        EditOrderItemInput[] editOrderItems = null,
        RemoveOrderItemInput[] removeOrderItems = null,
        AddPaymentDueDateInput[] addPaymentDueDates = null,
        DeletePaymentDueDateInput[] deletePaymentDueDates = null,
        EditPaymentDueDateInput[] editPaymentDueDates = null,
        AddOrderDocumentInput[] addOrderDocumentInputs = null,
        EditOrderDocumentInput[] editOrderDocumentInputs = null,
        DeleteOrderDocumentInput[] deleteOrderDocumentInputs = null
        )
    {
      #region Edit Order
      var order = EditOrder(
              id: id,
              rowVersion: rowVersion,
              customerId: customerId,
              orderTypeId: orderTypeId,
              orderer: orderer,
              description: description,
              documentNumber: documentNumber,
              documentType: documentType,
              isDeleted: isDeleted);
      #endregion
      #region AddOrderDocumentInput
      if (addOrderDocumentInputs.Any())
      {
        foreach (var item in addOrderDocumentInputs)
        {
          var UploadFile = Core.App.Providers.Session.GetAs<UploadFileData>(item.FileKey);
          var document = App.Internals.ApplicationBase.AddDocument(name: UploadFile.FileName, fileStream: UploadFile.FileData);
          AddOrderDocument(orderId: order.Id, documentId: document.Id, description: item.Description);
        }
      }
      #endregion
      #region EditOrderDocumentInput
      if (editOrderDocumentInputs.Any())
      {
        foreach (var item in editOrderDocumentInputs)
        {
          EditOrderDocument(orderId: order.Id, documentId: item.DocumentId, description: item.Description, rowversion: item.RowVersion);
        }
      }
      #endregion
      #region DeleteOrderDocumentInput
      if (deleteOrderDocumentInputs.Any())
      {
        foreach (var item in deleteOrderDocumentInputs)
        {
          DeleteOrderDocument(orderId: order.Id, documentId: item.DocumentId, rowVersion: item.RowVersion);
        }
      }
      #endregion
      #region  AddPaymentDueDate 
      if (addPaymentDueDates != null)
      {
        foreach (var addPaymentItem in addPaymentDueDates)
        {
          var PaymentdueDate = AddPaymentDueDate(
                    paymentDate: addPaymentItem.PaymentDate,
                    paymentTypeId: addPaymentItem.PaymentTypeId,
                    amount: addPaymentItem.Amount,
                    orderId: order.Id
                   );
        }
      }
      #endregion
      #region  EditPaymentDueDate 
      if (editPaymentDueDates != null)
      {
        foreach (var editPaymentItem in editPaymentDueDates)
        {
          var PaymentdueDate = EditPaymentDueDate(
                    paymentDate: editPaymentItem.PaymentDate,
                    paymentTypeId: editPaymentItem.PaymentTypeId,
                    amount: editPaymentItem.Amount,
                    rowVersion: editPaymentItem.RowVersion,
                    id: editPaymentItem.Id
                   );
        }
      }
      #endregion
      #region  deletePaymentDueDate 
      if (deletePaymentDueDates != null)
      {
        foreach (var deletePaymentItem in deletePaymentDueDates)
        {
          var PaymentdueDate = DeletePaymentDueDate(
                    rowVersion: deletePaymentItem.RowVersion,
                    id: deletePaymentItem.Id
                   );
        }
      }
      #endregion
      #region Remove OrderItems
      if (removeOrderItems != null)
        foreach (var item in removeOrderItems)
          App.Internals.SaleManagement.RemoveOrderItemProcess(
                        id: item.Id,
                        rowVersion: item.RowVersion);
      #endregion
      #region Add OrderItems
      if (addOrderItems != null)
        foreach (var item in addOrderItems)
        {
          #region Check ProjectHeader
          var stuff = App.Internals.SaleManagement.GetStuff(item.StuffId);
          if (stuff.ProjectHeader == null)
            throw new StuffProjectHeaderIsNullException(stuff.Code);
          var projectHeaderId = stuff.ProjectHeader.Id;
          #endregion
          #region Add OrderItemProcess
          AddOrderItemProcess(
                  productPackStuffId: item.ProductPackStuffId,
                  productPackVersion: item.ProductPackVersion,
                  description: item.Description,
                  qty: item.Qty,
                  orderId: order.Id,
                  stuffId: item.StuffId,
                  unitId: item.UnitId,
                  deliveryDate: item.DeliveryDate,
                  requestDate: item.RequestDate,
                  billOfMaterialVersion: item.BillOfMaterialVersion);
          #endregion
        }
      #endregion
      #region Edit OrderItems
      if (editOrderItems != null)
        foreach (var item in editOrderItems)
        {
          #region Edit OrderItemProcess                      
          EditOrderItemProcess(
                  id: item.Id,
                  rowVersion: item.RowVersion,
                  description: item.Description,
                  qty: item.Qty,
                  canceledQty: item.CanceledQty,
                  unitId: item.UnitId,
                  deliveryDate: item.DeliveryDate,
                  requestDate: item.RequestDate,
                  billOfMaterialVersion: item.BillOfMaterialVersion);
          #endregion
        }
      #endregion
      return order;
    }
    #endregion
    #region Add
    public Order AddOrder(
        int customerId,
        int orderTypeId,
        string orderer,
        string description,
        string documentNumber,
        OrderDocumentType documentType,
        double? totalAmount
        )
    {
      //var orderCount = GetOrders(x => x, documentNumber: documentNumber).Count();
      //if (orderCount != 0)
      //    throw new DocumentNumberExistException();
      var order = repository.Create<Order>();
      order.CustomerId = customerId;
      order.Description = description;
      order.Orderer = orderer;
      order.OrderTypeId = orderTypeId;
      order.DocumentNumber = documentNumber;
      order.DocumentType = documentType;
      order.TotalAmount = totalAmount;
      repository.Add(order);
      return order;
    }
    #endregion
    #region Edit
    public Order EditOrder(
        int id,
        byte[] rowVersion,
        TValue<int> customerId = null,
        TValue<int> orderTypeId = null,
        TValue<string> orderer = null,
        TValue<string> description = null,
        TValue<string> documentNumber = null,
        TValue<OrderDocumentType> documentType = null,
        TValue<bool> isDeleted = null)
    {
      if (documentNumber != null)
      {
        var orderCount = GetOrders(x => x, documentNumber: documentNumber)

              .Where(x => x.Id != id && x.DocumentNumber == documentNumber)
              .Count();
        if (orderCount != 0)
          throw new InternalException("شماره سند سفارش تکراری می باشد!");
      }
      var order = GetOrder(id: id);
      if (customerId != null)
        order.CustomerId = customerId;
      if (orderTypeId != null)
        order.OrderTypeId = orderTypeId;
      if (orderer != null)
        order.Orderer = orderer;
      if (description != null)
        order.Description = description;
      if (documentType != null)
        order.DocumentType = documentType;
      if (documentNumber != null)
        order.DocumentNumber = documentNumber;
      if (isDeleted != null)
        order.IsDelete = isDeleted;
      repository.Update(entity: order, rowVersion: rowVersion);
      return order;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetOrders<TResult>(
        Expression<Func<Order, TResult>> selector,
        TValue<int> id = null,
        TValue<int> customerId = null,
        TValue<string> orderer = null,
        TValue<string> description = null,
        TValue<int> orderTypeId = null,
        TValue<string> documentNumber = null,
        TValue<OrderDocumentType> documentType = null,
        TValue<bool> isDeleted = null
        )
    {
      var query = repository.GetQuery<Order>();
      if (id != null)
        query = query.Where(order => order.Id == id);
      if (customerId != null)
        query = query.Where(order => order.CustomerId == customerId);
      if (orderer != null)
        query = query.Where(order => order.Orderer == orderer);
      if (description != null)
        query = query.Where(order => order.Description == description);
      if (orderTypeId != null)
        query = query.Where(order => order.OrderTypeId == orderTypeId);
      if (documentNumber != null)
        query = query.Where(order => order.DocumentNumber == documentNumber);
      if (documentType != null)
        query = query.Where(order => order.DocumentType == documentType);
      if (isDeleted != null)
        query = query.Where(order => order.IsDelete == isDeleted);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public Order GetOrder(int id) => GetOrder(selector: e => e, id: id);
    public TResult GetOrder<TResult>(
        Expression<Func<Order, TResult>> selector,
        int id)
    {
      var order = GetOrders(
                    selector: selector,
                    id: id)

                .FirstOrDefault();
      if (order == null)
        throw new Core.Exceptions.RecordNotFoundException(id, typeof(Order));
      return order;
    }
    #endregion
    #region ToResult
    public Expression<Func<Order, OrderResult>> ToOrderResult =
        order => new OrderResult
        {
          Id = order.Id,
          CustomerId = order.CustomerId,
          Orderer = order.Orderer,
          Description = order.Description,
          OrderTypeId = order.OrderTypeId,
          DocumentNumber = order.DocumentNumber,
          DocumentType = order.DocumentType,
          IsDeleted = order.IsDelete,
          RowVersion = order.RowVersion
        };
    #endregion
    #region ToFullResult
    public Expression<Func<Order, FullOrderResult>> ToFullOrderResult =
        order => new FullOrderResult
        {
          Id = order.Id,
          CustomerId = order.CustomerId,
          Orderer = order.Orderer,
          Description = order.Description,
          OrderTypeId = order.OrderTypeId,
          DocumentNumber = order.DocumentNumber,
          DocumentType = order.DocumentType,
          OrderItems = order.OrderItems.AsQueryable().Where(i => i.IsDelete == false).Select(App.Internals.SaleManagement.ToFullOrderItemResult),
          PaymentDueDates = order.PaymentDueDates.AsQueryable().Where(i => i.IsDelete == false).Select(App.Internals.SaleManagement.ToPaymentDueDateResult),
          OrderDocuments = order.OrderDocuments.AsQueryable().Where(i => i.IsDelete == false).Select(App.Internals.SaleManagement.ToOrderDocumentResult),
          IsDeleted = order.IsDelete,
          RowVersion = order.RowVersion,
          TotalAmount = order.TotalAmount,
        };
    #endregion
    public void DeleteOrder(int id, byte[] rowVersion)
    {
      var order = GetOrder(id);
      var orderItems = order.OrderItems.Where(x => !x.IsDelete).ToList();
      foreach (var item in orderItems)
        DeleteOrderItem(item, item.RowVersion);
      EditOrder(id: id, rowVersion: rowVersion, isDeleted: true);
    }
  }
}