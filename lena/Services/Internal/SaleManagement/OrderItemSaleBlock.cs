using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.SaleManagement.OrderItemSaleBlock;


using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Add
    public OrderItemSaleBlock AddOrderItemSaleBlock(
       OrderItemSaleBlock orderItemSaleBlock,
       string description,
       short warehouseId,
       int? billOfMaterialVersion,
       double qty,
       byte unitId,
       CheckOrderItem checkOrderItem)
    {

      var orderItem = checkOrderItem.OrderItemConfirmation.OrderItem;
      orderItemSaleBlock = orderItemSaleBlock ?? repository.Create<OrderItemSaleBlock>();
      orderItemSaleBlock.CheckOrderItemId = checkOrderItem.Id;
      AddOrderItemBlockProcess(
                    orderItemBlock: orderItemSaleBlock,
                    transactionBatch: null,
                    description: description,
                    orderItemId: orderItem.Id,
                    warehouseId: warehouseId,
                    qty: qty,
                    unitId: unitId,
                    address: null,
                    customerId: null);
      return orderItemSaleBlock;
    }
    #endregion
    #region Edit
    public OrderItemSaleBlock EditOrderItemSaleBlock(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<int> warehouseId = null,
        TValue<int?> billOfMaterialVersion = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<OrderItemBlock> orderItemBlock = null,
        TValue<int> checkOrderItemId = null)
    {

      OrderItemSaleBlock orderItemSaleBlock = GetOrderItemSaleBlock(id: id);
      var retValue = EditOrderItemSaleBlock(
                orderItemSaleBlock: orderItemSaleBlock,
                rowVersion: rowVersion,
                warehouseId: orderItemSaleBlock.WarehouseId,
                qty: orderItemSaleBlock.Qty,
                description: orderItemSaleBlock.Description,
                unitId: orderItemSaleBlock.UnitId);
      return retValue as OrderItemSaleBlock;
    }
    public OrderItemSaleBlock EditOrderItemSaleBlock(
       byte[] rowVersion,
       OrderItemSaleBlock orderItemSaleBlock,
       TValue<string> description = null,
       TValue<short> warehouseId = null,
       TValue<int?> billOfMaterialVersion = null,
       TValue<double> qty = null,
       TValue<byte> unitId = null,
       TValue<OrderItemBlock> orderItemBlock = null,
       TValue<int> checkOrderItemId = null)
    {

      if (checkOrderItemId != null)
        orderItemSaleBlock.CheckOrderItemId = checkOrderItemId;
      if (warehouseId != null)
        orderItemSaleBlock.WarehouseId = warehouseId;
      if (billOfMaterialVersion != null)
        if (unitId != null)
          orderItemSaleBlock.UnitId = unitId;
      if (qty != null)
      {
        if (qty < 0)
          throw new QtyInvalidException(qty);
        else
          orderItemSaleBlock.Qty = qty;
      }
      if (description != null)
        orderItemSaleBlock.Description = description;
      var retValue = EditOrderItemBlock(
                orderItemBlock: orderItemSaleBlock,
                rowVersion: rowVersion);
      return retValue as OrderItemSaleBlock;
    }
    #endregion
    #region Get
    public OrderItemSaleBlock GetOrderItemSaleBlock(int id) => GetOrderItemSaleBlock(selector: e => e, id: id);
    public TResult GetOrderItemSaleBlock<TResult>(
        Expression<Func<OrderItemSaleBlock, TResult>> selector,
        int id)
    {

      var orderItemSaleBlock = GetOrderItemSaleBlocks(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (orderItemSaleBlock == null)
        throw new OrderItemSaleBlockNotFoundException(id);
      return orderItemSaleBlock;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetOrderItemSaleBlocks<TResult>(
        Expression<Func<OrderItemSaleBlock, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<string> description = null,
        TValue<int> transactionBatchId = null,
        TValue<int> orderItemId = null,
        TValue<int> warehouseId = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<int> checkOrderItemId = null)
    {

      var baseQuery = GetOrderItemBlocks(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description,
                    orderItemId: orderItemId,
                    warehouseId: warehouseId,
                    qty: qty,
                    unitId: unitId);
      var query = baseQuery.OfType<OrderItemSaleBlock>();
      if (checkOrderItemId != null)
        query = query.Where(i => i.CheckOrderItemId == checkOrderItemId);
      return query.Select(selector);
    }
    #endregion
    #region AddProcess
    public OrderItemSaleBlock AddOrderItemSaleBlockProcess(
        string description,
        short warehouseId,
        int? billOfMaterialVersion,
        double qty,
        byte unitId,
        CheckOrderItem checkOrderItem)
    {

      var orderItemSaleBlock = AddOrderItemSaleBlock(
                    orderItemSaleBlock: null,
                    description: description,
                    warehouseId: warehouseId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    qty: qty,
                    unitId: unitId,
                    checkOrderItem: checkOrderItem);
      return orderItemSaleBlock;
    }
    #endregion
    #region ToFullResult
    public Expression<Func<OrderItemSaleBlock, OrderItemSaleBlockResult>> ToOrderItemSaleBlockResult =
    orderItemSaleBlock => new OrderItemSaleBlockResult()
    {
      Id = orderItemSaleBlock.Id,
      Description = orderItemSaleBlock.Description,
      OrderItemCode = orderItemSaleBlock.OrderItem.Code,
      CustomerCode = orderItemSaleBlock.OrderItem.Order.Customer.Code,
      CustomerName = orderItemSaleBlock.OrderItem.Order.Customer.Name,
      OrderTypeName = orderItemSaleBlock.OrderItem.Order.OrderType.Name,
      StuffId = orderItemSaleBlock.OrderItem.StuffId,
      StuffCode = orderItemSaleBlock.OrderItem.Stuff.Code,
      StuffName = orderItemSaleBlock.OrderItem.Stuff.Name,
      BillOfMaterialVersion = orderItemSaleBlock.OrderItem.BillOfMaterialVersion,
      RequestDate = orderItemSaleBlock.OrderItem.RequestDate,
      DeliveryDate = orderItemSaleBlock.OrderItem.DeliveryDate,
      Qty = orderItemSaleBlock.Qty,
      UnitId = orderItemSaleBlock.UnitId,
      UnitName = orderItemSaleBlock.Unit.Name,
      CheckOrderItemId = orderItemSaleBlock.CheckOrderItemId,
      WarehouseId = orderItemSaleBlock.WarehouseId,
      WarehouseName = orderItemSaleBlock.Warehouse.Name,
      OrderId = orderItemSaleBlock.OrderItem.OrderId,
      OrderItemId = orderItemSaleBlock.OrderItemId,
      RowVersion = orderItemSaleBlock.RowVersion,
    };

    public Expression<Func<OrderItemSaleBlock, FullOrderItemSaleBlockResult>> ToFullOrderItemSaleBlockResult =
    orderItemSaleBlock => new FullOrderItemSaleBlockResult()
    {
      Id = orderItemSaleBlock.Id,
      Description = orderItemSaleBlock.Description,
      OrderItemCode = orderItemSaleBlock.OrderItem.Code,
      CustomerCode = orderItemSaleBlock.OrderItem.Order.Customer.Code,
      CustomerName = orderItemSaleBlock.OrderItem.Order.Customer.Name,
      OrderTypeName = orderItemSaleBlock.OrderItem.Order.OrderType.Name,
      StuffId = orderItemSaleBlock.OrderItem.StuffId,
      StuffCode = orderItemSaleBlock.OrderItem.Stuff.Code,
      StuffName = orderItemSaleBlock.OrderItem.Stuff.Name,
      StuffNoun = orderItemSaleBlock.OrderItem.Stuff.Noun,
      BillOfMaterialVersion = orderItemSaleBlock.OrderItem.BillOfMaterialVersion,
      RequestDate = orderItemSaleBlock.OrderItem.RequestDate,
      DeliveryDate = orderItemSaleBlock.OrderItem.DeliveryDate,
      Qty = orderItemSaleBlock.Qty,
      UnitId = orderItemSaleBlock.UnitId,
      UnitName = orderItemSaleBlock.Unit.Name,
      UnitConversionRatio = orderItemSaleBlock.Unit.ConversionRatio,
      CheckOrderItemId = orderItemSaleBlock.CheckOrderItemId,
      WarehouseId = orderItemSaleBlock.WarehouseId,
      WarehouseName = orderItemSaleBlock.Warehouse.Name,
      OrderId = orderItemSaleBlock.OrderItem.OrderId,
      OrderItemId = orderItemSaleBlock.OrderItemId,
      RowVersion = orderItemSaleBlock.RowVersion,
    };
    #endregion
    public IQueryable<OrderItemSaleBlockResult> SearchOrderItemSaleBlock(IQueryable<OrderItemSaleBlockResult> query,
      string searchText,
      AdvanceSearchItem[] advanceSearchItems,
      int? orderId = null,
      int? orderItemId = null
      )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where
                item.OrderItemCode.Contains(searchText) ||
                item.WarehouseName.Contains(searchText) ||
                item.StuffName.Contains(searchText)
                select item;


      if (orderId != null)
        query = query.Where(i => i.OrderId == orderId);
      if (orderItemId == null)
        query = query.Where(i => i.OrderItemId == orderItemId);

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);

      return query;
    }
    public IQueryable<FullOrderItemSaleBlockResult> SearchFullOrderItemSaleBlock(IQueryable<FullOrderItemSaleBlockResult> query,
      string searchText,
      AdvanceSearchItem[] advanceSearchItems
      )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where
                item.OrderItemCode.Contains(searchText) ||
                item.WarehouseName.Contains(searchText) ||
                item.StuffName.Contains(searchText)
                select item;

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);

      return query;
    }

    public IOrderedQueryable<FullOrderItemSaleBlockResult> SortFullOrderItemSaleBlockResult(IQueryable<FullOrderItemSaleBlockResult> query, SortInput<FullOrderItemSaleBlockSortType> sort)
    {

      switch (sort.SortType)
      {
        case FullOrderItemSaleBlockSortType.Id:
          return query.OrderBy(i => i.Id, sort.SortOrder);
        case FullOrderItemSaleBlockSortType.CustomerCode:
          return query.OrderBy(i => i.CustomerCode, sort.SortOrder);
        case FullOrderItemSaleBlockSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sort.SortOrder);
        case FullOrderItemSaleBlockSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sort.SortOrder);
        case FullOrderItemSaleBlockSortType.BillOfMaterialVersion:
          return query.OrderBy(i => i.BillOfMaterialVersion, sort.SortOrder);
        case FullOrderItemSaleBlockSortType.RequestDate:
          return query.OrderBy(i => i.RequestDate, sort.SortOrder);
        case FullOrderItemSaleBlockSortType.DeliveryDate:
          return query.OrderBy(i => i.DeliveryDate, sort.SortOrder);
        case FullOrderItemSaleBlockSortType.Qty:
          return query.OrderBy(i => i.Qty, sort.SortOrder);
        case FullOrderItemSaleBlockSortType.UnitName:
          return query.OrderBy(i => i.UnitName, sort.SortOrder);
        case FullOrderItemSaleBlockSortType.WarehouseName:
          return query.OrderBy(i => i.WarehouseName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException("Sort method not implemented for this sort type of FullOrderItemSaleBlockSortType");
      }
    }
  }

}
