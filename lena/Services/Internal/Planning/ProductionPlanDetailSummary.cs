using System;
using System.Linq;
using System.Linq.Expressions;
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
    public ProductionPlanDetailSummary GetProductionPlanDetailSummaryByProductionPlanDetailId(int productionPlanDetailId)
              => GetProductionPlanDetailSummaryByProductionPlanDetailId(selector: e => e, productionPlanDetailId: productionPlanDetailId);
    public TResult GetProductionPlanDetailSummaryByProductionPlanDetailId<TResult>(
        Expression<Func<ProductionPlanDetailSummary, TResult>> selector,
        int productionPlanDetailId)
    {
      var productionPlanDetailSummary = GetProductionPlanDetailSummarys(
                    selector: selector,
                    productionPlanDetailId: productionPlanDetailId)
                .FirstOrDefault();
      if (productionPlanDetailSummary == null)
        throw new ProductionPlanDetailSummaryForProductionPlanDetailNotFoundException(productionPlanDetailId: productionPlanDetailId);
      return productionPlanDetailSummary;
    }
    #endregion
    #region Get
    public ProductionPlanDetailSummary GetProductionPlanDetailSummary(int id) => GetProductionPlanDetailSummary(selector: e => e, id: id);
    public TResult GetProductionPlanDetailSummary<TResult>(
        Expression<Func<ProductionPlanDetailSummary, TResult>> selector,
        int id)
    {
      var productionPlanDetailSummary = GetProductionPlanDetailSummarys(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (productionPlanDetailSummary == null)
        throw new ProductionPlanDetailSummaryNotFoundException(id: id);
      return productionPlanDetailSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionPlanDetailSummarys<TResult>(
            Expression<Func<ProductionPlanDetailSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> producedQty = null,
            TValue<double> scheduledQty = null,
            TValue<int> productionPlanDetailId = null)
    {
      var query = repository.GetQuery<ProductionPlanDetailSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (producedQty != null)
        query = query.Where(x => x.ProducedQty == producedQty);
      if (scheduledQty != null)
        query = query.Where(x => x.ScheduledQty == scheduledQty);
      if (productionPlanDetailId != null)
        query = query.Where(x => x.ProductionPlanDetail.Id == productionPlanDetailId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public ProductionPlanDetailSummary AddProductionPlanDetailSummary(
        double producedQty,
        double scheduledQty,
        int productionPlanDetailId)
    {
      var productionPlanDetailSummary = repository.Create<ProductionPlanDetailSummary>();
      productionPlanDetailSummary.ProducedQty = producedQty;
      productionPlanDetailSummary.ScheduledQty = scheduledQty;
      productionPlanDetailSummary.ProductionPlanDetail = GetProductionPlanDetail(id: productionPlanDetailId);
      repository.Add(productionPlanDetailSummary);
      return productionPlanDetailSummary;
    }
    #endregion
    #region Edit
    public ProductionPlanDetailSummary EditProductionPlanDetailSummary(
        int id,
        byte[] rowVersion,
        TValue<double> producedQty = null,
        TValue<double> scheduledQty = null)
    {
      var productionPlanDetailSummary = GetProductionPlanDetailSummary(id: id);
      return EditProductionPlanDetailSummary(
                    productionPlanDetailSummary: productionPlanDetailSummary,
                    rowVersion: rowVersion,
                    producedQty: producedQty,
                    scheduledQty: scheduledQty);
    }
    public ProductionPlanDetailSummary EditProductionPlanDetailSummary(
                ProductionPlanDetailSummary productionPlanDetailSummary,
                byte[] rowVersion,
                TValue<double> producedQty = null,
                TValue<double> scheduledQty = null)
    {
      if (producedQty != null)
        productionPlanDetailSummary.ProducedQty = producedQty;
      if (scheduledQty != null)
        productionPlanDetailSummary.ScheduledQty = scheduledQty;
      repository.Update(rowVersion: rowVersion, entity: productionPlanDetailSummary);
      return productionPlanDetailSummary;
    }
    #endregion
    #region Delete
    public void DeleteProductionPlanDetailSummary(int id)
    {
      var productionPlanDetailSummary = GetProductionPlanDetailSummary(id: id);
      repository.Delete(productionPlanDetailSummary);
    }
    #endregion
    #region Reset
    public ProductionPlanDetailSummary ResetProductionPlanDetailSummaryByProductionPlanDetailId(int productionPlanDetailId)
    {
      var productionPlanDetailSummary = GetProductionPlanDetailSummaryByProductionPlanDetailId(productionPlanDetailId: productionPlanDetailId); ; return ResetProductionPlanDetailSummary(productionPlanDetailSummary: productionPlanDetailSummary);
    }
    public ProductionPlanDetailSummary ResetProductionPlanDetailSummary(int id)
    {
      var productionPlanDetailSummary = GetProductionPlanDetailSummary(id: id); ; return ResetProductionPlanDetailSummary(productionPlanDetailSummary: productionPlanDetailSummary);
    }
    public ProductionPlanDetailSummary ResetProductionPlanDetailSummary(ProductionPlanDetailSummary productionPlanDetailSummary)
    {
      #region GetProductionSchedules
      var productionScheduleQtys = GetProductionSchedules(
              selector: e =>
                  new
                  {
                    producedQty = e.ProductionScheduleSummary.ProducedQty,
                    scheduledQty = e.Qty
                  },
              isDelete: false,
              productionPlanDetailId: productionPlanDetailSummary.ProductionPlanDetail.Id);
      var producedQty = 0d;
      var scheduledQty = 0d;
      if (productionScheduleQtys.Any())
      {
        producedQty = productionScheduleQtys.Sum(i => i.producedQty);
        scheduledQty = productionScheduleQtys.Sum(i => i.scheduledQty);
      }
      #endregion
      #region EditProductionPlanDetailSummary
      EditProductionPlanDetailSummary(
              productionPlanDetailSummary: productionPlanDetailSummary,
              rowVersion: productionPlanDetailSummary.RowVersion,
              producedQty: producedQty,
              scheduledQty: scheduledQty);
      #endregion
      #region ResetProductionPlanStatus
      ResetProductionPlanStatus(id: productionPlanDetailSummary.ProductionPlanDetail.ProductionPlanId);
      #endregion
      return productionPlanDetailSummary;
    }
    #endregion
  }
}