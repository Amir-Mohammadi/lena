using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Services.Internals.Production.Exception;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {

    #region Get
    public ProductionOrderSummary GetProductionOrderSummaryByProductionOrderId(int productionOrderId) =>
    GetProductionOrderSummaryByProductionOrderId(selector: e => e, productionOrderId: productionOrderId);
    public TResult GetProductionOrderSummaryByProductionOrderId<TResult>(
        Expression<Func<ProductionOrderSummary, TResult>> selector,
        int productionOrderId)
    {

      var productionOrderSummary = GetProductionOrderSummarys(
                    selector: selector,
                    productionOrderId: productionOrderId)


                .FirstOrDefault();
      if (productionOrderSummary == null)
        throw new ProductionOrderSummaryForProductionOrderNotFoundException(productionOrderId: productionOrderId);
      return productionOrderSummary;
    }
    #endregion
    #region Get
    public ProductionOrderSummary GetProductionOrderSummary(int id) => GetProductionOrderSummary(selector: e => e, id: id);
    public TResult GetProductionOrderSummary<TResult>(
        Expression<Func<ProductionOrderSummary, TResult>> selector,
        int id)
    {

      var productionOrderSummary = GetProductionOrderSummarys(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (productionOrderSummary == null)
        throw new ProductionOrderSummaryNotFoundException(id: id);
      return productionOrderSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionOrderSummarys<TResult>(
            Expression<Func<ProductionOrderSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> producedQty = null,
            TValue<int> productionOrderId = null)
    {

      var query = repository.GetQuery<ProductionOrderSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (producedQty != null)
        query = query.Where(x => x.ProducedQty == producedQty);
      if (productionOrderId != null)
        query = query.Where(x => x.ProductionOrder.Id == productionOrderId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public ProductionOrderSummary AddProductionOrderSummary(
        double producedQty,
        double inProductionQty,
        int productionOrderId)
    {

      var productionOrderSummary = repository.Create<ProductionOrderSummary>();
      productionOrderSummary.ProducedQty = producedQty;
      productionOrderSummary.InProductionQty = inProductionQty;
      productionOrderSummary.ProductionOrder = GetProductionOrder(id: productionOrderId);
      repository.Add(productionOrderSummary);
      return productionOrderSummary;
    }
    #endregion
    #region Edit
    public ProductionOrderSummary EditProductionOrderSummary(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<double> producedQty = null)
    {

      var productionOrderSummary = GetProductionOrderSummary(id: id);
      return EditProductionOrderSummary(
                    productionOrderSummary: productionOrderSummary,
                    rowVersion: rowVersion,
                    description: description,
                    producedQty: producedQty);
    }

    public ProductionOrderSummary EditProductionOrderSummary(
                ProductionOrderSummary productionOrderSummary,
                byte[] rowVersion,
                TValue<string> description = null,
                TValue<double> producedQty = null,
                TValue<double> inProductionQty = null)
    {

      if (producedQty != null)
        productionOrderSummary.ProducedQty = producedQty;
      if (inProductionQty != null)
        productionOrderSummary.InProductionQty = inProductionQty;
      if (description != null)
        productionOrderSummary.Description = description;
      repository.Update(rowVersion: rowVersion, entity: productionOrderSummary);
      return productionOrderSummary;
    }

    #endregion
    #region Delete
    public void DeleteProductionOrderSummary(int id)
    {

      var productionOrderSummary = GetProductionOrderSummary(id: id);
      repository.Delete(productionOrderSummary);
    }
    #endregion
    #region Reset
    public ProductionOrderSummary ResetProductionOrderSummaryByProductionOrderId(int productionOrderId)
    {

      var productionOrderSummary = GetProductionOrderSummaryByProductionOrderId(productionOrderId: productionOrderId); ; return ResetProductionOrderSummary(productionOrderSummary: productionOrderSummary);

    }
    public ProductionOrderSummary ResetProductionOrderSummary(int id)
    {

      var productionOrderSummary = GetProductionOrderSummary(id: id); ; return ResetProductionOrderSummary(productionOrderSummary: productionOrderSummary);

    }

    public ProductionOrderSummary ResetProductionOrderSummary(ProductionOrderSummary productionOrderSummary)
    {

      #region Get Productions
      var productionQtys = GetProductions(
          selector: e => new
          {
            Qty = (e.StuffSerial.InitQty - e.StuffSerial.PartitionedQty) * e.StuffSerial.InitUnit.ConversionRatio / e.ProductionOrder.Unit.ConversionRatio,
            Status = e.Status,
            e.ProductionOrderId,
            StuffId = e.StuffSerial.StuffId,
            StuffSerialCode = e.StuffSerial.Code,
            ProductionType = e.Type,
            ConversionRatio = e.ProductionOrder.Unit.ConversionRatio,

          },
          isDelete: false,
          statuses: new[] { ProductionStatus.Produced, ProductionStatus.InProduction },
          productionOrderId: productionOrderSummary.ProductionOrder.Id);

      var summary = from item in productionQtys
                    group item by new { item.ProductionOrderId, item.Status, item.ProductionType } into gItems
                    select new
                    {
                      Status = gItems.Key.Status,
                      ProductionType = gItems.Key.ProductionType,
                      ProductionOrderId = gItems.Key.ProductionOrderId,
                      Sum = (double?)gItems.Sum(i => i.Qty)
                    };

      var producedQty = summary.FirstOrDefault(i => i.Status == ProductionStatus.Produced)?.Sum ?? 0;

      double inProductionQty = 0;

      var inProductions = productionQtys.Where(m => m.ProductionType == ProductionType.Partial && m.Status == ProductionStatus.InProduction).ToList();

      if (inProductions.Any())
      {
        foreach (var item in inProductions)
        {
          double qty = 0;
          var serialBuffer = App.Internals.WarehouseManagement.GetSerialBuffers(
                           selector: e => new
                           {
                             RemainingQty = e.RemainingAmount,
                             ConversionRatio = e.BaseTransaction.Unit.ConversionRatio
                           },
                           stuffId: item.StuffId,
                           stuffSerialCode: item.StuffSerialCode)

                        .FirstOrDefault();
          if (serialBuffer != null)
          {
            var remainingQty = serialBuffer.RemainingQty == 0 ? item.Qty : serialBuffer.RemainingQty;
            qty = (remainingQty * serialBuffer.ConversionRatio) / item.ConversionRatio;
          }
          else
          {
            qty = item.Qty;
          }
          inProductionQty += qty;
        }
      }
      else
      {
        inProductionQty = summary.FirstOrDefault(i => i.Status == ProductionStatus.InProduction && i.ProductionType == ProductionType.Complete)?.Sum ?? 0;
      }

      #endregion

      #region EditProductionOrderSummary
      EditProductionOrderSummary(
          productionOrderSummary: productionOrderSummary,
          rowVersion: productionOrderSummary.RowVersion,
          producedQty: producedQty,
          inProductionQty: inProductionQty);
      #endregion

      #region ResetProductionScheduleStatus
      var productionScheduleId = productionOrderSummary.ProductionOrder.ProductionScheduleId;
      if (productionScheduleId != null)
      {
        App.Internals.Planning.ResetProductionScheduleStatus(id: productionScheduleId.Value);
      }
      #endregion
      return productionOrderSummary;
    }

    #endregion


  }
}
