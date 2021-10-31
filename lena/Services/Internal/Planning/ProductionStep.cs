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
using lena.Models.Planning.ProductionStep;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {

    public ProductionStep AddProductionStepProcess(string name, string description, int productivityImpactFactor, int[] workStations)
    {

      var productionStep =
                AddProductionStep(name: name, description: description, productivityImpactFactor: productivityImpactFactor);
      foreach (var workStation in workStations)
      {
        AddProductionLineProductionStep(productionLineId: workStation, productionStepId: productionStep.Id);
      }
      return productionStep;
    }

    public ProductionStep EditProductionStepProcess(byte[] rowVersion, int id, TValue<string> name = null, TValue<int> productivityImpactFactor = null,
        TValue<string> description = null, TValue<int[]> addedWorkStations = null, TValue<int[]> deletedWorkStations = null)
    {

      var productionStep = EditProductionStep(rowVersion: rowVersion, id: id, name: name, description: description, productivityImpactFactor: productivityImpactFactor);
      if (deletedWorkStations != null)
      {
        foreach (var deleted in deletedWorkStations.Value)
        {
          DeleteProductionLineProductionStep(productionLineId: deleted, productionStepId: productionStep.Id);
        }
      }

      if (addedWorkStations != null)
      {
        foreach (var added in addedWorkStations.Value)
        {
          AddProductionLineProductionStep(productionLineId: added, productionStepId: productionStep.Id);
        }
      }

      return productionStep;
    }

    public void DeleteProductionStep(int id)
    {

      var productionStep = GetProductionStep(id: id);
      if (productionStep.ProductionLineProductionSteps.Any())
        throw new ProductionStepHasProductionLineProductionStepsExeption();
      repository.Delete(productionStep);
    }
    public ProductionStep AddProductionStep(string name, string description, int productivityImpactFactor)
    {

      var productionStep = repository.Create<ProductionStep>();
      productionStep.Name = name;
      productionStep.Description = description;
      productionStep.ProductivityImpactFactor = productivityImpactFactor;
      repository.Add(productionStep);
      return productionStep;
    }

    public ProductionStep EditProductionStep(byte[] rowVersion, int id, TValue<string> name = null, TValue<int> productivityImpactFactor = null,
        TValue<string> description = null)
    {

      var productionStep = GetProductionStep(id: id);
      if (name != null)
        productionStep.Name = name;
      if (description != null)
        productionStep.Description = description;
      if (productivityImpactFactor != null)
        productionStep.ProductivityImpactFactor = productivityImpactFactor;
      repository.Update(productionStep, rowVersion);
      return productionStep;
    }

    public IQueryable<ProductionStep> GetProductionSteps(TValue<int> id = null, TValue<string> name = null,
        TValue<string> description = null, TValue<int> productivityImpactFactor = null)
    {

      var isIdNull = id == null;
      var isNameNull = name == null;
      var isDescriptionNull = description == null;
      var isProductivityImpactFactor = productivityImpactFactor == null;

      var productionSteps = from item in repository.GetQuery<ProductionStep>()
                            where (isIdNull || item.Id == id) &&
                                        (isNameNull || item.Name == name) &&
                                        (isDescriptionNull || item.Description == description) &&
                                        (isProductivityImpactFactor || item.ProductivityImpactFactor == productivityImpactFactor)
                            select item;
      return productionSteps;
    }

    public ProductionStep GetProductionStep(int id)
    {

      var productionStep = GetProductionSteps(id: id).FirstOrDefault();
      if (productionStep == null)
        throw new ProductionStepNotFoundException(id);
      return productionStep;
    }

    public ProductionStepResult ToProductionStepResult(ProductionStep productionStep)
    {
      return new ProductionStepResult
      {
        Id = productionStep.Id,
        Name = productionStep.Name,
        ProductivityImpactFactor = productionStep.ProductivityImpactFactor,
        Description = productionStep.Description,
        RowVersion = productionStep.RowVersion,
        ProductionLines = productionStep.ProductionLineProductionSteps.Select(a => a.ProductionLineId).ToArray()
      };
    }

    public IQueryable<ProductionStepResult> ToProductionStepResultQuery(IQueryable<ProductionStep> query)
    {
      return from item in query
             select new ProductionStepResult
             {
               Id = item.Id,
               Name = item.Name,
               Description = item.Description,
               ProductivityImpactFactor = item.ProductivityImpactFactor,
               RowVersion = item.RowVersion
             };
    }

    public IQueryable<ProductionStepComboResult> ToProductionStepComboResult(IQueryable<ProductionStep> query)
    {
      return from item in query
             select new ProductionStepComboResult
             {
               Name = item.Name,
               Id = item.Id
             };
    }

    public IQueryable<ProductionStepResult> SearchProductionStepResultQuery(
       IQueryable<ProductionStepResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from productionStep in query
                where productionStep.Id.ToString().Contains(searchText) ||
                      productionStep.Name.Contains(searchText) ||
                      productionStep.Description.Contains(searchText)
                select productionStep;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }


    public IOrderedQueryable<ProductionStepResult> SortProductionStepResult(
        IQueryable<ProductionStepResult> query, SortInput<ProductionStepSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProductionStepSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ProductionStepSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case ProductionStepSortType.ProductivityImpactFactor:
          return query.OrderBy(a => a.ProductivityImpactFactor, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
