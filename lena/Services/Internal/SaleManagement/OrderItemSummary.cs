using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Common;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Get
    public OrderItemSummary GetOrderItemSummaryByOrderItemId(int orderItemId) => GetOrderItemSummaryByOrderItemId(selector: e => e, orderItemId: orderItemId);
    public TResult GetOrderItemSummaryByOrderItemId<TResult>(
        Expression<Func<OrderItemSummary, TResult>> selector,
        int orderItemId)
    {

      var orderItemSummary = GetOrderItemSummarys(
                    selector: selector,
                    orderItemId: orderItemId)


                .FirstOrDefault();
      if (orderItemSummary == null)
        throw new OrderItemSummaryForOrderItemNotFoundException(orderItemId: orderItemId);
      return orderItemSummary;
    }
    #endregion
    #region Get
    public OrderItemSummary GetOrderItemSummary(int id) => GetOrderItemSummary(selector: e => e, id: id);
    public TResult GetOrderItemSummary<TResult>(
        Expression<Func<OrderItemSummary, TResult>> selector,
        int id)
    {

      var orderItemSummary = GetOrderItemSummarys(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (orderItemSummary == null)
        throw new OrderItemSummaryNotFoundException(id: id);
      return orderItemSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetOrderItemSummarys<TResult>(
            Expression<Func<OrderItemSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> plannedQty = null,
            TValue<double> producedQty = null,
            TValue<double> blockedQty = null,
            TValue<double> permissionQty = null,
            TValue<double> preparingSendingQty = null,
            TValue<double> sendedQty = null,
            TValue<int> orderItemId = null)
    {

      var query = repository.GetQuery<OrderItemSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (plannedQty != null)
        query = query.Where(x => x.PlannedQty == plannedQty);
      if (producedQty != null)
        query = query.Where(x => x.ProducedQty == producedQty);
      if (blockedQty != null)
        query = query.Where(x => x.BlockedQty == blockedQty);
      if (permissionQty != null)
        query = query.Where(x => x.PermissionQty == permissionQty);
      if (preparingSendingQty != null)
        query = query.Where(x => x.PreparingSendingQty == preparingSendingQty);
      if (sendedQty != null)
        query = query.Where(x => x.SendedQty == sendedQty);
      if (orderItemId != null)
        query = query.Where(x => x.OrderItem.Id == orderItemId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public OrderItemSummary AddOrderItemSummary(
        double plannedQty,
        double producedQty,
        double blockedQty,
        double permissionQty,
        double preparingSendingQty,
        double sendedQty,
        int orderItemId)
    {

      var orderItemSummary = repository.Create<OrderItemSummary>();
      orderItemSummary.PlannedQty = plannedQty;
      orderItemSummary.ProducedQty = producedQty;
      orderItemSummary.BlockedQty = blockedQty;
      orderItemSummary.PermissionQty = permissionQty;
      orderItemSummary.PreparingSendingQty = preparingSendingQty;
      orderItemSummary.SendedQty = sendedQty;
      orderItemSummary.OrderItem = GetOrderItem(id: orderItemId);
      repository.Add(orderItemSummary);
      return orderItemSummary;
    }
    #endregion
    #region Edit
    public OrderItemSummary EditOrderItemSummary(
        int id,
        byte[] rowVersion,
        TValue<double> plannedQty = null,
        TValue<double> producedQty = null,
        TValue<double> blockedQty = null,
        TValue<double> permissionQty = null,
        TValue<double> preparingSendingQty = null,
        TValue<double> sendedQty = null)
    {

      var orderItemSummary = GetOrderItemSummary(id: id);
      return EditOrderItemSummary(
                    orderItemSummary: orderItemSummary,
                    rowVersion: rowVersion,
                    plannedQty: plannedQty,
                    producedQty: producedQty,
                    blockedQty: blockedQty,
                    permissionQty: permissionQty,
                    preparingSendingQty: preparingSendingQty,
                    sendedQty: sendedQty);
    }
    public OrderItemSummary EditOrderItemSummary(
                OrderItemSummary orderItemSummary,
                byte[] rowVersion,
                TValue<double> plannedQty = null,
                TValue<double> producedQty = null,
                TValue<double> blockedQty = null,
                TValue<double> permissionQty = null,
                TValue<double> preparingSendingQty = null,
                TValue<double> sendedQty = null,
                TValue<double> sentToOtherCustomersQty = null,
                TValue<double> blockedQtyOtherCustomers = null)
    {

      if (plannedQty != null)
        orderItemSummary.PlannedQty = plannedQty;
      if (producedQty != null)
        orderItemSummary.ProducedQty = producedQty;
      if (blockedQty != null)
        orderItemSummary.BlockedQty = blockedQty;
      if (permissionQty != null)
        orderItemSummary.PermissionQty = permissionQty;
      if (preparingSendingQty != null)
        orderItemSummary.PreparingSendingQty = preparingSendingQty;
      if (sendedQty != null)
        orderItemSummary.SendedQty = sendedQty;
      if (sentToOtherCustomersQty != null)
        orderItemSummary.SentToOtherCustomersQty = sentToOtherCustomersQty;
      if (blockedQtyOtherCustomers != null)
        orderItemSummary.BlockedQtyOtherCustomers = blockedQtyOtherCustomers;

      repository.Update(rowVersion: rowVersion, entity: orderItemSummary);
      return orderItemSummary;
    }
    #endregion
    #region Delete
    public void DeleteOrderItemSummary(int id)
    {

      var orderItemSummary = GetOrderItemSummary(id: id);
      repository.Delete(orderItemSummary);
    }
    #endregion
    #region Reset
    public OrderItemSummary ResetOrderItemSummaryByOrderItemId(int orderItemId)
    {

      var orderItemSummary = GetOrderItemSummaryByOrderItemId(orderItemId: orderItemId); ; return ResetOrderItemSummary(orderItemSummary: orderItemSummary);
    }
    public OrderItemSummary ResetOrderItemSummary(int id)
    {

      var orderItemSummary = GetOrderItemSummary(id: id); ; return ResetOrderItemSummary(orderItemSummary: orderItemSummary);
    }
    public OrderItemSummary ResetOrderItemSummary(OrderItemSummary orderItemSummary)
    {


      #region GetOrderItemBlocks
      var orderItemBlockQtys = GetOrderItemBlocks(
              selector: e => new
              {
                BlockedQty = e.Qty * e.Unit.ConversionRatio / e.OrderItem.Unit.ConversionRatio,
                //todo fix 
                PermissionQty = e.ExitReceiptRequestSummary.PermissionQty * e.Unit.ConversionRatio / e.OrderItem.Unit.ConversionRatio,
                PreparingSendingQty = e.ExitReceiptRequestSummary.PreparingSendingQty * e.Unit.ConversionRatio / e.OrderItem.Unit.ConversionRatio,
                SendedQty = e.ExitReceiptRequestSummary.SendedQty * e.Unit.ConversionRatio / e.OrderItem.Unit.ConversionRatio
              },
              isDelete: false,
              orderItemId: orderItemSummary.OrderItem.Id)

          .ToList();
      var blockedQty = 0d;
      var permissionQty = 0d;
      var preparingSendingQty = 0d;
      var sendedQty = 0d;

      if (orderItemBlockQtys.Any())
      {
        blockedQty = orderItemBlockQtys.Sum(i => i.BlockedQty);
        if (blockedQty == 0)
          blockedQty = orderItemSummary.BlockedQty;
        //else
        //    blockedQty += orderItemSummary.BlockedQty;
        permissionQty = orderItemBlockQtys.Sum(i => i.PermissionQty);
        if (permissionQty == 0)
          permissionQty = orderItemSummary.PermissionQty;
        //else
        //    permissionQty += orderItemSummary.PermissionQty;
        preparingSendingQty = orderItemBlockQtys.Sum(i => i.PreparingSendingQty);
        if (preparingSendingQty == 0)
          preparingSendingQty = orderItemSummary.PreparingSendingQty;
        //else
        //    preparingSendingQty += orderItemSummary.PreparingSendingQty;
        sendedQty = orderItemBlockQtys.Sum(i => i.SendedQty);
        //if (sendedQty == 0)
        //    sendedQty = orderItemSummary.SendedQty;
        //else
        //    sendedQty += orderItemSummary.SendedQty;
      }
      #endregion

      var otherCustomersOrderItemBlocksQty = 0d;
      var sentQtyOtherCustomers = 0d;

      var otherCustomersOrderItemBlocks = GetOrderItemBlocks(
                   selector: e => new
                   {
                     BlockedQty = e.Qty * e.Unit.ConversionRatio / e.OrderItem.Unit.ConversionRatio,
                     //todo fix 
                     PermissionQty = e.ExitReceiptRequestSummary.PermissionQty * e.Unit.ConversionRatio / e.OrderItem.Unit.ConversionRatio,
                     PreparingSendingQty = e.ExitReceiptRequestSummary.PreparingSendingQty * e.Unit.ConversionRatio / e.OrderItem.Unit.ConversionRatio,
                     SendedQty = e.ExitReceiptRequestSummary.SendedQty * e.Unit.ConversionRatio / e.OrderItem.Unit.ConversionRatio,

                   },
                   isDelete: false,
                   orderItemId: orderItemSummary.OrderItem.Id,
                notHasCustomerIds: new[] { orderItemSummary.OrderItem.Order.CustomerId })

               .ToList();

      if (otherCustomersOrderItemBlocks.Any())
      {
        otherCustomersOrderItemBlocksQty = otherCustomersOrderItemBlocks.Sum(i => i.BlockedQty);
        sentQtyOtherCustomers = otherCustomersOrderItemBlocks.Sum(i => i.SendedQty);
      }

      #region GetProductionRequests
      var productionRequestQtys = App.Internals.SaleManagement.GetProductionRequests(
              selector: e =>
                  new
                  {
                    producedQty = e.ProductionRequestSummary.ProducedQty * e.Unit.ConversionRatio /
                                    e.CheckOrderItem.OrderItemConfirmation.OrderItem.Unit.ConversionRatio,
                    plannedQty = e.Qty * e.Unit.ConversionRatio /
                                   e.CheckOrderItem.OrderItemConfirmation.OrderItem.Unit.ConversionRatio,
                  },
              isDelete: false,
              orderItemId: orderItemSummary.OrderItem.Id);

      var plannedQty = orderItemSummary.PlannedQty;
      var producedQty = orderItemSummary.ProducedQty;

      if (productionRequestQtys.Any())
      {
        producedQty = productionRequestQtys.Sum(i => i.producedQty);
        plannedQty = productionRequestQtys.Sum(i => i.plannedQty);
      }
      #endregion
      #region EditOrderItemSummary
      EditOrderItemSummary(
              orderItemSummary: orderItemSummary,
              rowVersion: orderItemSummary.RowVersion,
              plannedQty: plannedQty,
              producedQty: producedQty,
              blockedQty: blockedQty,
              permissionQty: permissionQty,
              preparingSendingQty: preparingSendingQty,
              sendedQty: sendedQty,
              blockedQtyOtherCustomers: otherCustomersOrderItemBlocksQty,
              sentToOtherCustomersQty: sentQtyOtherCustomers);
      #endregion
      return orderItemSummary;
    }
    #endregion
  }
}
