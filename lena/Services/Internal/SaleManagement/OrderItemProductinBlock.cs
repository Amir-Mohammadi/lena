using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.ProductionManagement.OrderItemProductionBlock;
using lena.Models.SaleManagement.OrderItemProductionBlock;
using lena.Models.SaleManagement.OrderItemSaleBlock;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Add
    public OrderItemProductionBlock AddOrderItemProductionBlock(
       OrderItemProductionBlock orderItemProductionBlock,
       string description,
       short warehouseId,
       int? billOfMaterialVersion,
       double qty,
       byte unitId,
       int orderItemId,
       string address,
       int? customerId)
    {

      orderItemProductionBlock = orderItemProductionBlock ?? repository.Create<OrderItemProductionBlock>();
      AddOrderItemBlockProcess(
                orderItemBlock: orderItemProductionBlock,
                transactionBatch: null,
                description: description,
                orderItemId: orderItemId,
                warehouseId: warehouseId,
                qty: qty,
                unitId: unitId,
                address: address,
                customerId: customerId
                );
      return orderItemProductionBlock;
    }
    #endregion
    #region Edit
    public OrderItemProductionBlock EditOrderItemProductionBlock(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<int> warehouseId = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<OrderItemBlock> orderItemBlock = null,
        TValue<int> checkOrderItemId = null)
    {

      var orderItemProductionBlock = GetOrderItemProductionBlock(id: id);
      var retValue = EditOrderItemProductionBlock(
                orderItemProductionBlock: orderItemProductionBlock,
                rowVersion: rowVersion,
                warehouseId: orderItemProductionBlock.WarehouseId,
                qty: orderItemProductionBlock.Qty,
                description: orderItemProductionBlock.Description,
                unitId: orderItemProductionBlock.UnitId);
      return retValue;
    }
    public OrderItemProductionBlock EditOrderItemProductionBlock(
       byte[] rowVersion,
       OrderItemProductionBlock orderItemProductionBlock,
       TValue<string> description = null,
       TValue<int> warehouseId = null,
       TValue<int?> billOfMaterialVersion = null,
       TValue<double> qty = null,
       TValue<byte> unitId = null,
       TValue<OrderItemBlock> orderItemBlock = null)
    {

      var retValue = EditOrderItemBlock(
                orderItemBlock: orderItemProductionBlock,
                rowVersion: rowVersion,
                qty: qty,
                unitId: unitId);
      return retValue as OrderItemProductionBlock;
    }
    #endregion
    #region Get
    public OrderItemProductionBlock GetOrderItemProductionBlock(int id) => GetOrderItemProductionBlock(selector: e => e, id: id);
    public TResult GetOrderItemProductionBlock<TResult>(
        Expression<Func<OrderItemProductionBlock, TResult>> selector,
        int id)
    {

      var orderItemProductionBlock = GetOrderItemProductionBlocks(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (orderItemProductionBlock == null)
        throw new OrderItemProductionBlockNotFoundException(id);
      return orderItemProductionBlock;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetOrderItemProductionBlocks<TResult>(
        Expression<Func<OrderItemProductionBlock, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<string> description = null,
        TValue<int> transactionBatchId = null,
        TValue<int> orderItemId = null,
        TValue<int> warehouseId = null,
        TValue<double> qty = null,
        TValue<int> unitId = null)
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
      var query = baseQuery.OfType<OrderItemProductionBlock>();
      return query.Select(selector);
    }
    #endregion
    #region AddProcess
    public OrderItemProductionBlock AddOrderItemProductionBlockProcess(
        string description,
        short warehouseId,
        int? billOfMaterialVersion,
        double qty,
        byte unitId,
        int orderItemId,
        string address,
        int? customerId)
    {

      var orderItemProductionBlock = AddOrderItemProductionBlock(
                    orderItemProductionBlock: null,
                    description: description,
                    warehouseId: warehouseId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    qty: qty,
                    unitId: unitId,
                    orderItemId: orderItemId,
                    address: address,
                    customerId: customerId
                    );
      return orderItemProductionBlock;
    }

    public void AddOrderItemsProductionBlockProcess(AddOrderItemProductionBlockInputs input)
    {

      foreach (var item in input.AddOrderItemProductionBlockInputArray)
      {
        var orderItemProductionBlock = AddOrderItemProductionBlock(
                      orderItemProductionBlock: null,
                      description: item.Description,
                      warehouseId: item.WarehouseId,
                      billOfMaterialVersion: item.BillOfMaterialVersion,
                      qty: item.Qty,
                      unitId: item.UnitId,
                      orderItemId: item.OrderItemId,
                      address: item.ContactInfo,
                      customerId: item.CustomerId
                      );
      }
    }
    #endregion
    #region ToFullResult
    public Expression<Func<OrderItemProductionBlock, FullOrderItemProductionBlockResult>> ToFullOrderItemProductionBlockResult =
    orderItemProductionBlock => new FullOrderItemProductionBlockResult()
    {
      Id = orderItemProductionBlock.Id,
      Description = orderItemProductionBlock.Description,
      OrderItemCode = orderItemProductionBlock.OrderItem.Code,
      CustomerCode = orderItemProductionBlock.OrderItem.Order.Customer.Code,
      CustomerName = orderItemProductionBlock.OrderItem.Order.Customer.Name,
      OrderTypeName = orderItemProductionBlock.OrderItem.Order.OrderType.Name,
      StuffId = orderItemProductionBlock.OrderItem.StuffId,
      StuffCode = orderItemProductionBlock.OrderItem.Stuff.Code,
      StuffName = orderItemProductionBlock.OrderItem.Stuff.Name,
      StuffNoun = orderItemProductionBlock.OrderItem.Stuff.Noun,
      BillOfMaterialVersion = orderItemProductionBlock.OrderItem.BillOfMaterialVersion,
      RequestDate = orderItemProductionBlock.OrderItem.RequestDate,
      DeliveryDate = orderItemProductionBlock.OrderItem.DeliveryDate,
      Qty = orderItemProductionBlock.Qty,
      UnitId = orderItemProductionBlock.UnitId,
      UnitName = orderItemProductionBlock.Unit.Name,
      UnitConversionRatio = orderItemProductionBlock.Unit.ConversionRatio,
      WarehouseId = orderItemProductionBlock.WarehouseId,
      WarehouseName = orderItemProductionBlock.Warehouse.Name,
      RowVersion = orderItemProductionBlock.RowVersion,
    };
    #endregion
  }
}
