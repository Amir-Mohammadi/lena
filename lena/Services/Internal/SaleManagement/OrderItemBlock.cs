using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.StaticData;
using lena.Models;
using lena.Models.Common;
using lena.Models.SaleManagement.OrderItemBlock;
//using System.Data.Entity.SqlServer;
//using System.Data.Entity;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region GetOrderItemBlock
    public OrderItemBlock GetOrderItemBlock(int id) => GetOrderItemBlock(selector: e => e, id: id);
    public TResult GetOrderItemBlock<TResult>(
        Expression<Func<OrderItemBlock, TResult>> selector,
        int id)
    {

      var orderItemBlock = GetOrderItemBlocks(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (orderItemBlock == null)
        throw new OrderItemBlockNotFoundException(id);
      return orderItemBlock;
    }
    public IQueryable<TResult> GetOrderItemBlocks<TResult>(
        Expression<Func<OrderItemBlock, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> orderItemId = null,
        TValue<int> warehouseId = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<int> customerId = null,
        TValue<int[]> notHasCustomerIds = null,
        TValue<int> stuffId = null,
        TValue<OrderItemBlockType> orderItemBlockType = null,
        TValue<ExitReceiptRequestStatus> orderItemBlockStatus = null,
        TValue<ExitReceiptRequestStatus[]> orderItemBlockStatuses = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var orderItemBlocks = baseQuery.OfType<OrderItemBlock>();
      if (orderItemId != null)
        orderItemBlocks = orderItemBlocks.Where(i => i.OrderItemId == orderItemId);
      if (fromDate != null && toDate != null)
        orderItemBlocks = orderItemBlocks.Where(i => i.DateTime >= fromDate && i.DateTime <= toDate);
      if (customerId != null)
        orderItemBlocks = orderItemBlocks.Where(i => i.OrderItem.Order.CustomerId == customerId);
      if (stuffId != null)
        orderItemBlocks = orderItemBlocks.Where(i => i.OrderItem.StuffId == stuffId);
      //todo fix 
      //if (orderItemBlockStatus != null)
      //    orderItemBlocks = orderItemBlocks.Where(i => i.OrderItemBlockStatusType == orderItemBlockStatus);
      //if (orderItemBlockStatuses != null)
      //    orderItemBlocks = orderItemBlocks.Where(i =>
      //        orderItemBlockStatuses.Value.Contains(i.OrderItemBlockStatusType));
      if (orderItemBlockType != null)
        orderItemBlocks = orderItemBlocks.Where(i => i.OrderItemBlockType == orderItemBlockType);
      if (qty != null)
        orderItemBlocks = orderItemBlocks.Where(i => Math.Abs(i.Qty - qty) < 0.000001);
      if (unitId != null)
        orderItemBlocks = orderItemBlocks.Where(i => i.UnitId == unitId);
      if (warehouseId != null)
        orderItemBlocks = orderItemBlocks.Where(i => i.WarehouseId == warehouseId);
      if (notHasCustomerIds != null && notHasCustomerIds.Value.Length != 0)
        orderItemBlocks = orderItemBlocks.Where(i => !notHasCustomerIds.Value.Contains(i.OrderItem.Order.CustomerId));
      return orderItemBlocks.Select(selector);
    }
    public IQueryable<SendProductSummaryResult> GetSendProductSummary(
         int? stuffId,
         int? customerId,
         int? exitReceiptRequestTypeId,
         DateTime? fromDateOrder,
         DateTime? toDateOrder,
         DateTime? fromDateTransfer,
         DateTime? toDateTransfer,
       bool dividedByDate,
       bool dividedByCustomer,
       bool? ceofficientSetType)
    {

      var preparingSendingItems = App.Internals.WarehouseManagement.GetPreparingSendingItems(
                    selector: preparingSendingItem => new
                    {
                      Qty = preparingSendingItem.Qty,
                      UnitId = preparingSendingItem.UnitId,
                      UnitConversionRatio = preparingSendingItem.Unit.ConversionRatio,
                      DateTime = preparingSendingItem.PreparingSending.SendProduct.ExitReceipt.DateTime,
                      StuffId = preparingSendingItem.StuffId,
                      CooperatorId = preparingSendingItem.PreparingSending.SendPermission.ExitReceiptRequest
                            .CooperatorId,
                      DateTimeOrderItem =
                        (preparingSendingItem.PreparingSending.SendPermission.ExitReceiptRequest as OrderItemBlock)
                        .OrderItem.DateTime,
                      RowVersion = preparingSendingItem.RowVersion,
                      ExitReceiptRequestTypeId = preparingSendingItem.PreparingSending.SendPermission.ExitReceiptRequest.ExitReceiptRequestType.Id,

                    }
                    ,
                    fromDate: fromDateTransfer,
                    toDate: toDateTransfer,
                    cooperatorId: customerId,
                    stuffId: stuffId,
                    exitReceiptRequestTypeId: exitReceiptRequestTypeId,
                    isDelete: false);


      if (fromDateOrder != null)
        preparingSendingItems = preparingSendingItems.Where(x => x.DateTimeOrderItem >= fromDateOrder);
      if (toDateOrder != null)
        preparingSendingItems = preparingSendingItems.Where(x => x.DateTimeOrderItem <= toDateOrder);

      var preparingSendingItemsGroup = (from query in preparingSendingItems
                                        let date = dividedByDate ? query.DateTime.Date : (DateTime?)null
                                        let cooperatorId = dividedByCustomer ? query.CooperatorId : (int?)null
                                        group query by new
                                        {
                                          StuffId = query.StuffId,
                                          Date = date,
                                          CustomerId = cooperatorId,
                                          ExitReceiptRequestTypeId = query.ExitReceiptRequestTypeId
                                        }
                into groupedData
                                        select new
                                        {
                                          StuffId = groupedData.Key.StuffId,
                                          CustomerId = groupedData.Key.CustomerId,
                                          Date = groupedData.Key.Date,
                                          SentQty = groupedData.Sum(r => r.Qty * r.UnitConversionRatio),
                                          ExitReceiptRequestTypeId = groupedData.Key.ExitReceiptRequestTypeId
                                        });

      var customers = GetCooperators(
                selector: e => e);
      var stuffs = GetStuffs(selector: e => e);
      var units = App.Internals.ApplicationBase.GetUnits(selector: e => e, isMainUnit: true);
      var exitReceiptRequestTypes = App.Internals.WarehouseManagement.GetExitReceiptRequestTypes(selector: e => e);




      var result = (from oib in preparingSendingItemsGroup
                    join exitReceiptRequestType in exitReceiptRequestTypes on oib.ExitReceiptRequestTypeId equals exitReceiptRequestType.Id
                    join stuff in stuffs on oib.StuffId equals stuff.Id
                    join unit in units on stuff.UnitTypeId equals unit.UnitTypeId
                    join customer in customers on oib.CustomerId equals customer.Id into allCustomer
                    from customerLeftJoin in allCustomer.DefaultIfEmpty()
                    where (stuff.CeofficientSet > 0 || ceofficientSetType == false)
                    select new SendProductSummaryResult()
                    {
                      StuffId = stuff.Id,
                      StuffCode = stuff.Code,
                      StuffName = stuff.Name,
                      UnitId = unit.Id,
                      UnitName = unit.Name,
                      CustomerId = customerLeftJoin.Id,
                      CustomerCode = customerLeftJoin.Code,
                      CustomerName = customerLeftJoin.Name,
                      SentQty = oib.SentQty,
                      SentDate = oib.Date,
                      ExitReceiptRequestTypeTitle = exitReceiptRequestType.Title,
                      CeofficientSet = stuff.CeofficientSet,
                      StuffCategoryId = stuff.StuffCategoryId,
                      StuffCategoryName = stuff.StuffCategory.Name,
                    });
      var q = result.ToList();
      return result;
    }
    #endregion
    #region AddOrderItemBlockProcess
    public OrderItemBlock AddOrderItemBlockProcess(
        OrderItemBlock orderItemBlock,
        TransactionBatch transactionBatch,
        string description,
        int orderItemId,
        short warehouseId,
        double qty,
        byte unitId,
        string address,
        int? customerId)
    {

      var orderItem = App.Internals.SaleManagement.GetOrderItem(id: orderItemId);
      orderItemBlock = orderItemBlock ?? repository.Create<OrderItemBlock>();
      orderItemBlock.OrderItemId = orderItemId;
      App.Internals.SaleManagement.AddExitReceiptRequestProcess(
                exitReceiptRequest: orderItemBlock,
                description: description,
                warehouseId: warehouseId,
                stuffId: orderItem.StuffId,
                qty: qty,
                unitId: unitId,
                exitReceiptRequestTypeId: StaticExitReceiptRequestTypes.SaleExitReceiptRequest.Id,
                address: address,
                cooperatorId: customerId ?? orderItem.Order.CustomerId,
                serials: null);
      return orderItemBlock;
    }
    #endregion
    #region EditOrderItemBlock
    public OrderItemBlock EditOrderItemBlock(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<short> warehouseId = null,
        TValue<double> qty = null,
        TValue<ExitReceiptRequestStatus> status = null,
        TValue<byte> unitId = null)
    {

      OrderItemBlock orderItemBlock = GetOrderItemBlock(id: id);
      return EditOrderItemBlock(
                    orderItemBlock: orderItemBlock,
                    rowVersion: rowVersion,
                    description: description,
                    qty: qty,
                    unitId: unitId,
                    warehouseId: warehouseId,
                    status: status);
    }
    public OrderItemBlock EditOrderItemBlock(
        OrderItemBlock orderItemBlock,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<short> warehouseId = null,
        TValue<double> qty = null,
        TValue<ExitReceiptRequestStatus> status = null,
        TValue<byte> unitId = null,
        TValue<int> orderItemId = null)
    {


      var retValue = EditExitReceiptRequest(
                    exitReceiptRequest: orderItemBlock,
                    rowVersion: rowVersion,
                    description: description,
                    warehouseId: warehouseId,
                    qty: qty,
                    status: status,
                    unitId: unitId);
      return retValue as OrderItemBlock;
    }
    #endregion
    #region ToOrderItemBlockResult
    public Expression<Func<OrderItemBlock, OrderItemBlockResult>> ToOrderItemBlockResult =
        orderItemBlock => new OrderItemBlockResult
        {
          Id = orderItemBlock.Id,
          Code = orderItemBlock.Code,
          DateTime = orderItemBlock.DateTime,
          Description = orderItemBlock.Description,
          OrderItemId = orderItemBlock.OrderItem.Id,
          OrderItemCode = orderItemBlock.OrderItem.Code,
          OrderItemQty = orderItemBlock.OrderItem.Qty,
          OrderItemUnitId = orderItemBlock.OrderItem.UnitId,
          OrderItemUnitName = orderItemBlock.OrderItem.Unit.Name,
          CustomerCode = orderItemBlock.OrderItem.Order.Customer.Code,
          CustomerName = orderItemBlock.OrderItem.Order.Customer.Name,
          OrderTypeName = orderItemBlock.OrderItem.Order.OrderType.Name,
          StuffId = orderItemBlock.OrderItem.StuffId,
          StuffCode = orderItemBlock.OrderItem.Stuff.Code,
          StuffName = orderItemBlock.OrderItem.Stuff.Name,
          StuffNoun = orderItemBlock.OrderItem.Stuff.Noun,
          BillOfMaterialVersion = orderItemBlock.OrderItem.BillOfMaterialVersion,
          BillOfMaterialTitle = orderItemBlock.OrderItem.BillOfMaterial.Title,
          RequestDate = orderItemBlock.OrderItem.RequestDate,
          DeliveryDate = orderItemBlock.OrderItem.DeliveryDate,
          Qty = orderItemBlock.Qty,
          UnitId = orderItemBlock.UnitId,
          UnitName = orderItemBlock.Unit.Name,
          WarehouseId = orderItemBlock.WarehouseId,
          WarehouseName = orderItemBlock.Warehouse.Name,
          OrderItemBlockType = orderItemBlock.OrderItemBlockType,
          //todo fix 
          //OrderItemBlockStatusType = orderItemBlock.OrderItemBlockStatusType,
          PermissionQty = orderItemBlock.ExitReceiptRequestSummary.PermissionQty,
          PreparingSendingQty = orderItemBlock.ExitReceiptRequestSummary.PreparingSendingQty,
          SendedQty = orderItemBlock.ExitReceiptRequestSummary.SendedQty,
          RowVersion = orderItemBlock.RowVersion,
          DocumentNumber = orderItemBlock.OrderItem.Order.DocumentNumber,
          DocumentType = orderItemBlock.OrderItem.Order.DocumentType,
        };
    #endregion
    #region ToOrderItemBlockComboResult
    public Expression<Func<OrderItemBlock, OrderItemBlockComboResult>> ToOrderItemBlockComboResult =
        orderItemBlock => new OrderItemBlockComboResult()
        {
          Id = orderItemBlock.Id,
          Code = orderItemBlock.Code,
          DateTime = orderItemBlock.DateTime,
          Qty = orderItemBlock.Qty,
          UnitId = orderItemBlock.UnitId,
          UnitName = orderItemBlock.Unit.Name,
          WarehouseId = orderItemBlock.WarehouseId,
          WarehouseName = orderItemBlock.Warehouse.Name,
          RowVersion = orderItemBlock.RowVersion,
        };
    #endregion
    #region SearchOrderItemBlock
    public IQueryable<OrderItemBlockResult> SearchOrderItemBlockResult(IQueryable<OrderItemBlockResult> query, string search)
    {
      if (string.IsNullOrEmpty(search)) return query;
      query = query.Where(x =>
          x.StuffCode.Contains(search) ||
          x.StuffName.Contains(search) ||
          x.CustomerCode.Contains(search) ||
          x.CustomerName.Contains(search) ||
          x.OrderItemCode.Contains(search) ||
          x.Description.Contains(search));
      return query;
    }
    public IQueryable<SendProductSummaryResult> SearchSendProductSummaryResult(
        IQueryable<SendProductSummaryResult> query,
        string search,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(search))
      {
        query = query.Where(x =>
            x.StuffCode.Contains(search) ||
            x.StuffName.Contains(search) ||
            x.CustomerCode.Contains(search) ||
            x.ExitReceiptRequestTypeTitle.Contains(search) ||
            x.CustomerName.Contains(search));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<OrderItemBlockResult> SortOrderItemBlockResult(IQueryable<OrderItemBlockResult> query, SortInput<OrderItemBlockSortType> sort)
    {
      switch (sort.SortType)
      {
        case OrderItemBlockSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case OrderItemBlockSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case OrderItemBlockSortType.OrderItemBlockStatusType:
          return query.OrderBy(a => a.OrderItemBlockStatusType, sort.SortOrder);
        case OrderItemBlockSortType.OrderItemBlockType:
          return query.OrderBy(a => a.OrderItemBlockType, sort.SortOrder);
        case OrderItemBlockSortType.OrderedQty:
          return query.OrderBy(a => a.OrderItemQty, sort.SortOrder);
        case OrderItemBlockSortType.CustomerCode:
          return query.OrderBy(a => a.CustomerCode, sort.SortOrder);
        case OrderItemBlockSortType.CustomerName:
          return query.OrderBy(a => a.CustomerName, sort.SortOrder);
        case OrderItemBlockSortType.OrderItemCode:
          return query.OrderBy(a => a.OrderItemCode, sort.SortOrder);
        case OrderItemBlockSortType.PermissionQty:
          return query.OrderBy(a => a.PermissionQty, sort.SortOrder);
        case OrderItemBlockSortType.SendedQty:
          return query.OrderBy(a => a.SendedQty, sort.SortOrder);
        case OrderItemBlockSortType.PreparingSendingQty:
          return query.OrderBy(a => a.PreparingSendingQty, sort.SortOrder);
        case OrderItemBlockSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case OrderItemBlockSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case OrderItemBlockSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case OrderItemBlockSortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<SendProductSummaryResult> SortSendProductSummaryResult(IQueryable<SendProductSummaryResult> query, SortInput<SendProductSummarySortType> sort)
    {
      switch (sort.SortType)
      {
        case SendProductSummarySortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case SendProductSummarySortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case SendProductSummarySortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case SendProductSummarySortType.CustomerCode:
          return query.OrderBy(a => a.CustomerCode, sort.SortOrder);
        case SendProductSummarySortType.CustomerName:
          return query.OrderBy(a => a.CustomerName, sort.SortOrder);
        case SendProductSummarySortType.SentQty:
          return query.OrderBy(a => a.SentQty, sort.SortOrder);
        case SendProductSummarySortType.SentDate:
          return query.OrderBy(a => a.SentDate, sort.SortOrder);
        case SendProductSummarySortType.ExitReceiptRequestTypeTitle:
          return query.OrderBy(a => a.ExitReceiptRequestTypeTitle, sort.SortOrder);
        case SendProductSummarySortType.CeofficientSet:
          return query.OrderBy(a => a.CeofficientSet, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
