using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.SaleManagement.CheckOrderItem;
using lena.Models.SaleManagement.OrderItemSaleBlock;
using lena.Models.SaleManagement.ProductionRequest;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Gets
    public IQueryable<TResult> GetCheckOrderItems<TResult>(
        Expression<Func<CheckOrderItem, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> orderItemConfirmationId = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<CheckOrderItem>();
      if (orderItemConfirmationId != null)
        query = query.Where(i => i.OrderItemConfirmationId == orderItemConfirmationId);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public CheckOrderItem GetCheckOrderItem(int id) => GetCheckOrderItem(selector: e => e, id: id);
    public TResult GetCheckOrderItem<TResult>(
            Expression<Func<CheckOrderItem, TResult>> selector,
            int id)
    {

      var checkOrderItem = GetCheckOrderItems(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (checkOrderItem == null)
        throw new CheckOrderItemNotFoundException(id);
      return checkOrderItem;
    }
    #endregion
    #region Add
    public CheckOrderItem AddCheckOrderItem(
        CheckOrderItem checkOrderItem,
        string description,
        bool confirmed,
        TransactionBatch transactionBatch,
        int orderItemConfirmationId)
    {

      checkOrderItem = checkOrderItem ?? repository.Create<CheckOrderItem>();
      checkOrderItem.OrderItemConfirmationId = orderItemConfirmationId;
      checkOrderItem.Confirmed = confirmed;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: checkOrderItem,
                    transactionBatch: transactionBatch,
                    description: description);
      return checkOrderItem;
    }
    #endregion
    #region AddProcess
    public CheckOrderItem AddCheckOrderItemProcess(
        int orderItemConfirmationId,
        byte[] orderItemConfirmationRowVersion,
        string description,
        bool confirmed,
        double? suggestedQty,
        DateTime? suggestedDeliveryDate,
        AddProductionRequestInput[] productionRequests,
        AddOrderItemSaleBlockInput[] orderItemSaleBlocks)
    {

      var orderItemConfirmatin = GetOrderItemConfirmation(id: orderItemConfirmationId);
      return AddCheckOrderItemProcess(
                    orderItemConfirmation: orderItemConfirmatin,
                    orderItemConfirmationRowVersion: orderItemConfirmationRowVersion,
                    description: description,
                    confirmed: confirmed,
                    suggestedQty: suggestedQty,
                    suggestedDeliveryDate: suggestedDeliveryDate,
                    productionRequests: productionRequests,
                    orderItemSaleBlocks: orderItemSaleBlocks);
    }
    public CheckOrderItem AddCheckOrderItemProcess(
        OrderItemConfirmation orderItemConfirmation,
        byte[] orderItemConfirmationRowVersion,
        string description,
        bool confirmed,
        double? suggestedQty,
        DateTime? suggestedDeliveryDate,
        AddProductionRequestInput[] productionRequests,
        AddOrderItemSaleBlockInput[] orderItemSaleBlocks)
    {

      #region Add CheckOrderItem
      var checkOrderItem = AddCheckOrderItem(
              checkOrderItem: null,
              description: description,
              confirmed: confirmed,
              transactionBatch: null,
              orderItemConfirmationId: orderItemConfirmation.Id);
      #endregion
      #region GetOrderItem
      var orderItem = GetOrderItem(id: orderItemConfirmation.OrderItemId);
      #endregion
      #region Edit OrderItem 
      EditOrderItem(orderItem: orderItem,
              rowVersion: orderItem.RowVersion,
              checkOrderItemConfirmed: checkOrderItem.Confirmed,
              checkOrderItemDateTime: checkOrderItem.DateTime);
      #endregion
      if (confirmed)
      {
        #region Add ProductionRequests
        foreach (var productionRequest in productionRequests)
        {
          AddProductionRequestProcess(
                        transactionBatch: null,
                        checkOrderItem: checkOrderItem,
                        description: productionRequest.Description,
                        qty: productionRequest.Qty,
                        unitId: productionRequest.UnitId,
                        deadlineDate: productionRequest.DeadlineDate);
        }
        #endregion
        #region Add OrderItemSaleBlock
        foreach (var saleBlock in orderItemSaleBlocks)
        {
          AddOrderItemSaleBlockProcess(
                        description: saleBlock.Description,
                        warehouseId: saleBlock.WarehouseId,
                        billOfMaterialVersion: saleBlock.BillOfMaterialVersion,
                        qty: saleBlock.Qty,
                        unitId: saleBlock.UnitId,
                        checkOrderItem: checkOrderItem);
        }
        #endregion
      }
      #region GetOrderItemInfo
      var orderItemInfo = GetOrderItem(selector: e =>
                  new
                  {
                    OrderItemQty = e.Qty * e.Unit.ConversionRatio,
                    Code = e.Code,
                    StuffName = e.Stuff.Name
                  },
              id: orderItemConfirmation.OrderItemId);
      #endregion
      #region ResetOrderItemStatus
      ResetOrderItemStatus(orderItem: orderItem);
      #endregion
      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                  baseEntityId: orderItemConfirmation.Id,
                  scrumTaskType: ScrumTaskTypes.CheckOrderItem);
      #endregion
      var taskIsDone = confirmed && orderItem.OrderItemSummary.PlannedQty >= orderItem.Qty;
      if (!confirmed && (suggestedQty != null || suggestedDeliveryDate != null))
      {
        taskIsDone = true;
        suggestedQty = suggestedQty ?? orderItem.Qty;
        suggestedDeliveryDate = suggestedDeliveryDate ?? orderItem.DeliveryDate;
        #region Add OrderItemChangeRequestProcess

        var isActive = true;
        if (orderItem.Status.HasFlag(OrderItemStatus.Deactive))
          isActive = false;

        App.Internals.SaleManagement.AddOrderItemChangeRequestProcess(
                      orderItemId: orderItem.Id,
                      description: description,
                      qty: suggestedQty.Value,
                      unitId: orderItem.UnitId,
                      deliveryDate: suggestedDeliveryDate.Value,
                      requestDate: DateTime.Now.ToUniversalTime(),
                      isActive: isActive);
        #endregion
      }
      #region DoneTask

      if (taskIsDone)
      {
        if (projectWorkItem != null)
        {
          App.Internals.ScrumManagement.DoneScrumTask(
                        scrumTask: projectWorkItem,
                        rowVersion: projectWorkItem.RowVersion);
        }
      }
      #endregion
      return checkOrderItem;
    }
    #endregion
    #region ToFullResult
    public Expression<Func<CheckOrderItem, FullCheckOrderItemResult>> ToFullCheckOrderItemResult =
    checkOrderItem => new FullCheckOrderItemResult()
    {
      Id = checkOrderItem.Id,
      Description = checkOrderItem.Description,
      CustomerCode = checkOrderItem.OrderItemConfirmation.OrderItem.Order.Customer.Code,
      CustomerName = checkOrderItem.OrderItemConfirmation.OrderItem.Order.Customer.Name,
      OrderTypeName = checkOrderItem.OrderItemConfirmation.OrderItem.Order.OrderType.Name,
      StuffId = checkOrderItem.OrderItemConfirmation.OrderItem.StuffId,
      StuffCode = checkOrderItem.OrderItemConfirmation.OrderItem.Stuff.Code,
      StuffName = checkOrderItem.OrderItemConfirmation.OrderItem.Stuff.Name,
      BillOfMaterialVersion = checkOrderItem.OrderItemConfirmation.OrderItem.BillOfMaterialVersion,
      BillOfMaterialTitle = checkOrderItem.OrderItemConfirmation.OrderItem.BillOfMaterial.Title,
      Qty = checkOrderItem.OrderItemConfirmation.OrderItem.Qty,
      UnitName = checkOrderItem.OrderItemConfirmation.OrderItem.Unit.Name,
      UnitId = checkOrderItem.OrderItemConfirmation.OrderItem.UnitId,
      RequestDate = checkOrderItem.OrderItemConfirmation.OrderItem.RequestDate,
      DeliveryDate = checkOrderItem.OrderItemConfirmation.OrderItem.DeliveryDate,
      OrderItemSaleBlocks = checkOrderItem.OrderItemSaleBlocks.AsQueryable().Select(App.Internals.SaleManagement.ToFullOrderItemSaleBlockResult),
      ProductionRequests = checkOrderItem.ProductionRequests.AsQueryable().Select(App.Internals.SaleManagement.ToFullProductionRequestResult),
      RowVersion = checkOrderItem.RowVersion,
    };
    #endregion
  }
}
