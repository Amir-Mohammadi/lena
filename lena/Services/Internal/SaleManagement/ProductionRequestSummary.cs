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
    public ProductionRequestSummary GetProductionRequestSummaryByProductionRequestId(int productionRequestId) =>
    GetProductionRequestSummaryByProductionRequestId(selector: e => e, productionRequestId: productionRequestId);
    public TResult GetProductionRequestSummaryByProductionRequestId<TResult>(
        Expression<Func<ProductionRequestSummary, TResult>> selector,
        int productionRequestId)
    {

      var productionRequestSummary = GetProductionRequestSummarys(
                    selector: selector,
                    productionRequestId: productionRequestId)


                .FirstOrDefault();
      if (productionRequestSummary == null)
        throw new ProductionRequestSummaryForProductionRequestNotFoundException(productionRequestId: productionRequestId);
      return productionRequestSummary;
    }
    #endregion
    #region Get
    public ProductionRequestSummary GetProductionRequestSummary(int id) => GetProductionRequestSummary(selector: e => e, id: id);
    public TResult GetProductionRequestSummary<TResult>(
        Expression<Func<ProductionRequestSummary, TResult>> selector,
        int id)
    {

      var productionRequestSummary = GetProductionRequestSummarys(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (productionRequestSummary == null)
        throw new ProductionRequestSummaryNotFoundException(id: id);
      return productionRequestSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionRequestSummarys<TResult>(
            Expression<Func<ProductionRequestSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> plannedQty = null,
            TValue<double> producedQty = null,
            TValue<double> scheduledQty = null,
            TValue<double> permissionQty = null,
            TValue<double> preparingSendingQty = null,
            TValue<double> sendedQty = null,
            TValue<int> productionRequestId = null)
    {



      var query = repository.GetQuery<ProductionRequestSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (plannedQty != null)
        query = query.Where(x => x.PlannedQty == plannedQty);
      if (producedQty != null)
        query = query.Where(x => x.ProducedQty == producedQty);
      if (scheduledQty != null)
        query = query.Where(x => x.ScheduledQty == scheduledQty);
      if (productionRequestId != null)
        query = query.Where(x => x.ProductionRequest.Id == productionRequestId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public ProductionRequestSummary AddProductionRequestSummary(
        double plannedQty,
        double producedQty,
        double scheduledQty,
        int productionRequestId)
    {

      var productionRequestSummary = repository.Create<ProductionRequestSummary>();
      productionRequestSummary.PlannedQty = plannedQty;
      productionRequestSummary.ProducedQty = producedQty;
      productionRequestSummary.ScheduledQty = scheduledQty;
      productionRequestSummary.ProductionRequest = GetProductionRequest(id: productionRequestId);
      repository.Add(productionRequestSummary);
      return productionRequestSummary;
    }
    #endregion
    #region Edit
    public ProductionRequestSummary EditProductionRequestSummary(
        int id,
        byte[] rowVersion,
        TValue<double> plannedQty = null,
        TValue<double> producedQty = null,
        TValue<double> scheduledQty = null)
    {

      var productionRequestSummary = GetProductionRequestSummary(id: id);
      return EditProductionRequestSummary(
                    productionRequestSummary: productionRequestSummary,
                    rowVersion: rowVersion,
                    plannedQty: plannedQty,
                    producedQty: producedQty,
                    scheduledQty: scheduledQty);

    }

    public ProductionRequestSummary EditProductionRequestSummary(
        ProductionRequestSummary productionRequestSummary,
        byte[] rowVersion,
        TValue<double> plannedQty = null,
        TValue<double> producedQty = null,
        TValue<double> scheduledQty = null)
    {

      if (plannedQty != null)
        productionRequestSummary.PlannedQty = plannedQty;
      if (producedQty != null)
        productionRequestSummary.ProducedQty = producedQty;
      if (scheduledQty != null)
        productionRequestSummary.ScheduledQty = scheduledQty;
      repository.Update(rowVersion: rowVersion, entity: productionRequestSummary);
      return productionRequestSummary;
    }

    #endregion
    #region Delete
    public void DeleteProductionRequestSummary(int id)
    {

      var productionRequestSummary = GetProductionRequestSummary(id: id);
      repository.Delete(productionRequestSummary);
    }
    #endregion
    #region Reset
    public ProductionRequestSummary ResetProductionRequestSummaryByProductionRequestId(int productionRequestId)
    {

      var productionRequestSummary = GetProductionRequestSummaryByProductionRequestId(productionRequestId: productionRequestId); ; return ResetProductionRequestSummary(productionRequestSummary: productionRequestSummary);

    }
    public ProductionRequestSummary ResetProductionRequestSummary(int id)
    {

      var productionRequestSummary = GetProductionRequestSummary(id: id); ; return ResetProductionRequestSummary(productionRequestSummary: productionRequestSummary);

    }

    public ProductionRequestSummary ResetProductionRequestSummary(ProductionRequestSummary productionRequestSummary)
    {


      #region GetProductionPlans
      var productionPlanQtys = App.Internals.Planning.GetProductionPlans(
              selector: e =>
                  new
                  {
                    producedQty = e.ProductionPlanSummary.ProducedQty * e.Unit.ConversionRatio / e.ProductionRequest.Unit.ConversionRatio,
                    scheduledQty = e.ProductionPlanSummary.ScheduledQty * e.Unit.ConversionRatio / e.ProductionRequest.Unit.ConversionRatio,
                    plannedQty = e.Qty * e.Unit.ConversionRatio / e.ProductionRequest.Unit.ConversionRatio
                  },
              isDelete: false,
              productionRequestId: productionRequestSummary.ProductionRequest.Id);

      var plannedQty = 0d;
      var producedQty = 0d;
      var scheduledQty = 0d;
      if (productionPlanQtys.Any())
      {
        producedQty = productionPlanQtys.Sum(i => i.producedQty);
        scheduledQty = productionPlanQtys.Sum(i => i.scheduledQty);
        plannedQty = productionPlanQtys.Sum(i => i.plannedQty);
      }
      #endregion
      #region EditProductionRequestSummary
      EditProductionRequestSummary(
              productionRequestSummary: productionRequestSummary,
              rowVersion: productionRequestSummary.RowVersion,
              plannedQty: plannedQty,
              producedQty: producedQty,
              scheduledQty: scheduledQty);
      #endregion
      #region GetOrderItemId
      var orderItemId = GetProductionRequestSummary(
              selector: e => e.ProductionRequest.CheckOrderItem.OrderItemConfirmation.OrderItemId,
              id: productionRequestSummary.Id);
      #endregion
      #region ResetProductionRequestStatus
      App.Internals.SaleManagement.ResetOrderItemStatus(id: orderItemId);
      #endregion
      return productionRequestSummary;
    }

    #endregion


  }
}
