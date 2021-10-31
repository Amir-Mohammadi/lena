using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Common;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Get
    public ProductionPlanSummary GetProductionPlanSummaryByProductionPlanId(int productionPlanId) =>
            GetProductionPlanSummaryByProductionPlanId(selector: e => e, productionPlanId: productionPlanId);
    public TResult GetProductionPlanSummaryByProductionPlanId<TResult>(
        Expression<Func<ProductionPlanSummary, TResult>> selector,
        int productionPlanId)
    {
      var productionPlanSummary = GetProductionPlanSummarys(
                    selector: selector,
                    productionPlanId: productionPlanId)
                .FirstOrDefault();
      if (productionPlanSummary == null)
        throw new ProductionPlanSummaryForProductionPlanNotFoundException(productionPlanId: productionPlanId);
      return productionPlanSummary;
    }
    #endregion
    #region Get
    public ProductionPlanSummary GetProductionPlanSummary(int id) => GetProductionPlanSummary(selector: e => e, id: id);
    public TResult GetProductionPlanSummary<TResult>(
        Expression<Func<ProductionPlanSummary, TResult>> selector,
        int id)
    {
      var productionPlanSummary = GetProductionPlanSummarys(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (productionPlanSummary == null)
        throw new ProductionPlanSummaryNotFoundException(id: id);
      return productionPlanSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionPlanSummarys<TResult>(
            Expression<Func<ProductionPlanSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> producedQty = null,
            TValue<double> scheduledQty = null,
            TValue<int> productionPlanId = null)
    {
      var query = repository.GetQuery<ProductionPlanSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (producedQty != null)
        query = query.Where(x => x.ProducedQty == producedQty);
      if (scheduledQty != null)
        query = query.Where(x => x.ScheduledQty == scheduledQty);
      if (productionPlanId != null)
        query = query.Where(x => x.ProductionPlan.Id == productionPlanId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public ProductionPlanSummary AddProductionPlanSummary(
        double producedQty,
        double scheduledQty,
        int productionPlanId)
    {
      var productionPlanSummary = repository.Create<ProductionPlanSummary>();
      productionPlanSummary.ProducedQty = producedQty;
      productionPlanSummary.ScheduledQty = scheduledQty;
      productionPlanSummary.ProductionPlan = GetProductionPlan(id: productionPlanId);
      repository.Add(productionPlanSummary);
      return productionPlanSummary;
    }
    #endregion
    #region Edit
    public ProductionPlanSummary EditProductionPlanSummary(
        int id,
        byte[] rowVersion,
        TValue<double> producedQty = null,
        TValue<double> scheduledQty = null)
    {
      var productionPlanSummary = GetProductionPlanSummary(id: id);
      return EditProductionPlanSummary(
                    productionPlanSummary: productionPlanSummary,
                    rowVersion: rowVersion,
                    producedQty: producedQty,
                    scheduledQty: scheduledQty);
    }
    public ProductionPlanSummary EditProductionPlanSummary(
                ProductionPlanSummary productionPlanSummary,
                byte[] rowVersion,
                TValue<double> producedQty = null,
                TValue<double> scheduledQty = null)
    {
      if (producedQty != null)
        productionPlanSummary.ProducedQty = producedQty;
      if (scheduledQty != null)
        productionPlanSummary.ScheduledQty = scheduledQty;
      repository.Update(rowVersion: rowVersion, entity: productionPlanSummary);
      return productionPlanSummary;
    }
    #endregion
    #region Delete
    public void DeleteProductionPlanSummary(int id)
    {
      var productionPlanSummary = GetProductionPlanSummary(id: id);
      repository.Delete(productionPlanSummary);
    }
    #endregion
    #region Reset
    public ProductionPlanSummary ResetProductionPlanSummaryByProductionPlanId(int productionPlanId)
    {
      var productionPlanSummary = GetProductionPlanSummaryByProductionPlanId(productionPlanId: productionPlanId); ; return ResetProductionPlanSummary(productionPlanSummary: productionPlanSummary);
    }
    public ProductionPlanSummary ResetProductionPlanSummary(int id)
    {
      var productionPlanSummary = GetProductionPlanSummary(id: id); ; return ResetProductionPlanSummary(productionPlanSummary: productionPlanSummary);
    }
    public ProductionPlanSummary ResetProductionPlanSummary(ProductionPlanSummary productionPlanSummary)
    {
      #region GetProductionPlanDetails
      var productionPlanDetailQtys = GetProductionPlanDetails(
              selector: e =>
                  new
                  {
                    producedQty = e.ProductionPlanDetailSummary.ProducedQty * e.Unit.ConversionRatio / e.ProductionPlan.Unit.ConversionRatio,
                    scheduledQty = e.Qty * e.Unit.ConversionRatio / e.ProductionPlan.Unit.ConversionRatio
                  },
              isDelete: false,
              productionPlanId: productionPlanSummary.ProductionPlan.Id,
              billOfMaterialStuffId: productionPlanSummary.ProductionPlan.BillOfMaterialStuffId,
              billOfMaterialVersion: productionPlanSummary.ProductionPlan.BillOfMaterialVersion
              );
      var producedQty = 0d;
      var scheduledQty = 0d;
      if (productionPlanDetailQtys.Any())
      {
        producedQty = productionPlanDetailQtys.Sum(i => i.producedQty);
        scheduledQty = productionPlanDetailQtys.Sum(i => i.scheduledQty);
      }
      #endregion
      #region EditProductionPlanSummary
      EditProductionPlanSummary(
              productionPlanSummary: productionPlanSummary,
              rowVersion: productionPlanSummary.RowVersion,
              producedQty: producedQty,
              scheduledQty: scheduledQty);
      #endregion
      #region ResetProductionRequestStatus
      if (productionPlanSummary.ProductionPlan.ProductionRequestId == null)
        throw new ProductionRequestIdIsNullException();
      App.Internals.SaleManagement.ResetProductionRequestStatus(id: productionPlanSummary.ProductionPlan.ProductionRequestId.Value);
      #endregion
      return productionPlanSummary;
    }
    #endregion
  }
}