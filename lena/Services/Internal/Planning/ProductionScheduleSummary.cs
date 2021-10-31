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
    public ProductionScheduleSummary GetProductionScheduleSummaryByProductionScheduleId(int productionScheduleId) =>
    GetProductionScheduleSummaryByProductionScheduleId(selector: e => e, productionScheduleId: productionScheduleId);
    public TResult GetProductionScheduleSummaryByProductionScheduleId<TResult>(
        Expression<Func<ProductionScheduleSummary, TResult>> selector,
        int productionScheduleId)
    {
      var productionScheduleSummary = GetProductionScheduleSummarys(
                    selector: selector,
                    productionScheduleId: productionScheduleId)
                .FirstOrDefault();
      if (productionScheduleSummary == null)
        throw new ProductionScheduleSummaryForProductionScheduleNotFoundException(productionScheduleId: productionScheduleId);
      return productionScheduleSummary;
    }
    #endregion
    #region Get
    public ProductionScheduleSummary GetProductionScheduleSummary(int id) => GetProductionScheduleSummary(selector: e => e, id: id);
    public TResult GetProductionScheduleSummary<TResult>(
        Expression<Func<ProductionScheduleSummary, TResult>> selector,
        int id)
    {
      var productionScheduleSummary = GetProductionScheduleSummarys(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (productionScheduleSummary == null)
        throw new ProductionScheduleSummaryNotFoundException(id: id);
      return productionScheduleSummary;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionScheduleSummarys<TResult>(
            Expression<Func<ProductionScheduleSummary, TResult>> selector,
            TValue<int> id = null,
            TValue<double> producedQty = null,
            TValue<int> productionScheduleId = null)
    {
      var query = repository.GetQuery<ProductionScheduleSummary>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (producedQty != null)
        query = query.Where(x => x.ProducedQty == producedQty);
      if (productionScheduleId != null)
        query = query.Where(x => x.ProductionSchedule.Id == productionScheduleId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public ProductionScheduleSummary AddProductionScheduleSummary(
        double producedQty,
        int productionScheduleId)
    {
      var productionScheduleSummary = repository.Create<ProductionScheduleSummary>();
      productionScheduleSummary.ProducedQty = producedQty;
      productionScheduleSummary.ProductionSchedule = GetProductionSchedule(id: productionScheduleId);
      repository.Add(productionScheduleSummary);
      return productionScheduleSummary;
    }
    #endregion
    #region Edit
    public ProductionScheduleSummary EditProductionScheduleSummary(
        int id,
        byte[] rowVersion,
        TValue<double> producedQty = null)
    {
      var productionScheduleSummary = GetProductionScheduleSummary(id: id);
      return EditProductionScheduleSummary(
                    productionScheduleSummary: productionScheduleSummary,
                    rowVersion: rowVersion,
                    producedQty: producedQty);
    }
    public ProductionScheduleSummary EditProductionScheduleSummary(
                ProductionScheduleSummary productionScheduleSummary,
                byte[] rowVersion,
                TValue<double> producedQty = null)
    {
      if (producedQty != null)
        productionScheduleSummary.ProducedQty = producedQty;
      repository.Update(rowVersion: rowVersion, entity: productionScheduleSummary);
      return productionScheduleSummary;
    }
    #endregion
    #region Delete
    public void DeleteProductionScheduleSummary(int id)
    {
      var productionScheduleSummary = GetProductionScheduleSummary(id: id);
      repository.Delete(productionScheduleSummary);
    }
    #endregion
    #region Reset
    public ProductionScheduleSummary ResetProductionScheduleSummaryByProductionScheduleId(int productionScheduleId)
    {
      var productionScheduleSummary = GetProductionScheduleSummaryByProductionScheduleId(productionScheduleId: productionScheduleId); ; return ResetProductionScheduleSummary(productionScheduleSummary: productionScheduleSummary);
    }
    public ProductionScheduleSummary ResetProductionScheduleSummary(int id)
    {
      var productionScheduleSummary = GetProductionScheduleSummary(id: id); ; return ResetProductionScheduleSummary(productionScheduleSummary: productionScheduleSummary);
    }
    public ProductionScheduleSummary ResetProductionScheduleSummary(ProductionScheduleSummary productionScheduleSummary)
    {
      #region GetProductionOrders
      var productionOrderQtys = App.Internals.Production.GetProductionOrders(
              selector: e => e.ProductionOrderSummary.ProducedQty * e.Unit.ConversionRatio /
                             e.ProductionSchedule.ProductionPlanDetail.Unit.ConversionRatio,
              isDelete: false,
              productionScheduleId: productionScheduleSummary.ProductionSchedule.Id);
      var producedQty = productionOrderQtys.Any() ? productionOrderQtys.Sum() : 0;
      #endregion
      #region EditProductionScheduleSummary
      EditProductionScheduleSummary(
              productionScheduleSummary: productionScheduleSummary,
              rowVersion: productionScheduleSummary.RowVersion,
              producedQty: producedQty);
      #endregion
      #region ResetProductionPlanDetailStatus
      ResetProductionPlanDetailStatus(id: productionScheduleSummary.ProductionSchedule.ProductionPlanDetailId);
      #endregion
      return productionScheduleSummary;
    }
    #endregion
  }
}