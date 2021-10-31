using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Planning.ProductionLineProductionStep;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public IQueryable<ProductionLineProductionStep> GetProductionLineProductionSteps(
        TValue<int> productionStepId = null,
        TValue<int> productionLineId = null)
    {

      var query = repository.GetQuery<ProductionLineProductionStep>();
      if (productionStepId != null)
        query = query.Where(x => x.ProductionStepId == productionStepId.Value);
      if (productionLineId != null)
        query = query.Where(x => x.ProductionLineId == productionLineId.Value);
      return query;
    }
    public ProductionLineProductionStep GetProductionLineProductionStep(int productionLineId, int productionStepId)
    {

      var workStationProductionStep = GetProductionLineProductionSteps(
                productionStepId: productionStepId,
                productionLineId: productionLineId).SingleOrDefault();
      if (workStationProductionStep == null)
        throw new ProductionLineProductionStepNotFoundException(
                  productionLineId: productionLineId,
                  productionStepId: productionStepId);
      return workStationProductionStep;
    }
    public ProductionLineProductionStep AddProductionLineProductionStep(
        int productionStepId,
        int productionLineId)
    {

      var workStationProductionStep = repository.Create<ProductionLineProductionStep>();
      workStationProductionStep.ProductionStepId = productionStepId;
      workStationProductionStep.ProductionLineId = productionLineId;
      repository.Add(workStationProductionStep);
      return workStationProductionStep;
    }
    public void DeleteProductionLineProductionStep(int productionLineId, int productionStepId)
    {

      var workStationProductionStep = GetProductionLineProductionStep(
                    productionLineId: productionLineId,
                    productionStepId: productionStepId);
      if (workStationProductionStep == null)
        throw new ProductionLineProductionStepNotFoundException(
                  productionLineId: productionLineId,
                  productionStepId: productionStepId);
      repository.Delete(workStationProductionStep);
    }
    public IQueryable<ProductionLineProductionStepResult> ToProductionLineProductionStepResultQuery(IQueryable<ProductionLineProductionStep> query)
    {
      var resultQuery = from productionLineProductionStep in query
                        let department = productionLineProductionStep.ProductionLine.Department
                        select new ProductionLineProductionStepResult()
                        {
                          ProductionStepId = productionLineProductionStep.ProductionStepId,
                          ProductionLineId = productionLineProductionStep.ProductionLineId,
                          ProductionStepName = productionLineProductionStep.ProductionStep.Name,
                          ProductionLineName = productionLineProductionStep.ProductionLine.Name,
                          DepartmentName = department.Name,
                          DepartmentId = department.Id,
                          RowVersion = productionLineProductionStep.RowVersion,
                        };
      return resultQuery;
    }
    public ProductionLineProductionStepResult ToProductionLineProductionStepResult(ProductionLineProductionStep workStationProductionStep)
    {
      var department = workStationProductionStep.ProductionLine.Department;
      var result = new ProductionLineProductionStepResult()
      {
        ProductionStepId = workStationProductionStep.ProductionStepId,
        ProductionLineId = workStationProductionStep.ProductionLineId,
        ProductionStepName = workStationProductionStep.ProductionStep.Name,
        ProductionLineName = workStationProductionStep.ProductionLine.Name,
        RowVersion = workStationProductionStep.RowVersion,
        DepartmentName = department.Name,
        DepartmentId = department.Id
      };
      return result;
    }
    public IOrderedQueryable<ProductionLineProductionStepResult> SortProductionLineProductionStepResult(IQueryable<ProductionLineProductionStepResult> input, SortInput<ProductionLineProductionStepSortType> options)
    {
      switch (options.SortType)
      {
        case ProductionLineProductionStepSortType.ProductionLineName:
          return input.OrderBy(i => i.ProductionLineName, options.SortOrder);
        case ProductionLineProductionStepSortType.ProductionStepName:
          return input.OrderBy(i => i.ProductionStepName, options.SortOrder);
        case ProductionLineProductionStepSortType.DepartmentName:
          return input.OrderBy(i => i.DepartmentName, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IQueryable<ProductionLineProductionStepResult> SearchProductionLineProductionStep(
        IQueryable<ProductionLineProductionStepResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
        int? productionLineId = null,
        int? productionStepId = null
        )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.ProductionLineName.Contains(searchText) ||
                      item.ProductionStepName.Contains(searchText)
                select item;
      }

      if (productionLineId.HasValue)
      {
        query = query.Where(i => i.ProductionLineId == productionLineId);
      }

      if (productionStepId.HasValue)
      {
        query = query.Where(i => i.ProductionStepId == productionStepId);
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
  }
}
