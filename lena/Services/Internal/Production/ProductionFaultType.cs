using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Production.ProductionFaultType;
using lena.Models.Production.StuffProductionFaultType;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {

    #region Get
    public ProductionFaultType GetProductionFaultType(int id) => GetProductionFaultType(selector: e => e, id: id);
    public TResult GetProductionFaultType<TResult>(
        Expression<Func<ProductionFaultType, TResult>> selector,
        int id)
    {

      var productionFaultType = GetProductionFaultTypes(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (productionFaultType == null)
        throw new ProductionFaultTypeNotFoundException(id);
      return productionFaultType;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionFaultTypes<TResult>(
        Expression<Func<ProductionFaultType, TResult>> selector,
        TValue<int> id = null,
        TValue<string> title = null,
        TValue<int?> operationId = null)
    {

      var query = repository.GetQuery<ProductionFaultType>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (title != null)
        query = query.Where(x => x.Title == title);
      if (operationId != null)
        query = query.Where(x => x.OperationId == operationId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public ProductionFaultType AddProductionFaultType(
        string title,
        short? operationId)
    {

      var productionFaultType = repository.Create<ProductionFaultType>();
      productionFaultType.Title = title;
      productionFaultType.OperationId = operationId;
      repository.Add(productionFaultType);
      return productionFaultType;
    }
    #endregion

    #region Add Process
    public ProductionFaultType AddProductionFaultTypeProcess(
        string title,
        short? operationId,
        int[] stuffIds)
    {

      var productionFaultType = AddProductionFaultType(
                    title: title,
                    operationId: operationId);
      foreach (var stuffId in stuffIds)
      {
        AddStuffProductionFaultType(
                      productionFaultTypeId: productionFaultType.Id,
                      stuffId: stuffId);
      }
      return productionFaultType;
    }
    #endregion
    #region Edit
    public ProductionFaultType EditProductionFaultType(
        int id,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<short?> operationId = null)
    {

      var productionFaultType = GetProductionFaultType(id: id);
      return EditProductionFaultType(
                productionFaultType: productionFaultType,
                rowVersion: rowVersion,
                 title: title,
                 operationId: operationId);

    }




    public ProductionFaultType EditProductionFaultType(
                ProductionFaultType productionFaultType,
                byte[] rowVersion,
            TValue<string> title = null,
            TValue<short?> operationId = null)
    {

      if (title != null)
        productionFaultType.Title = title;
      if (operationId != null)
        productionFaultType.OperationId = operationId;
      repository.Update(rowVersion: rowVersion, entity: productionFaultType);
      return productionFaultType;
    }

    #endregion

    #region Edit Process
    public ProductionFaultType EditProductionFaultTypeProcess(
        int id,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<short?> operationId = null,
        TValue<int[]> addStuffIds = null,
        TValue<int[]> deleteStuffIds = null)
    {

      var productionFaultType = EditProductionFaultType(
                    id: id,
                    rowVersion: rowVersion,
                    title: title,
                    operationId: operationId);


      if (addStuffIds != null)
        foreach (var stuffId in addStuffIds.Value)
        {
          AddStuffProductionFaultType(
                        productionFaultTypeId: productionFaultType.Id,
                        stuffId: stuffId);
        }
      if (deleteStuffIds != null)
        foreach (var stuffId in deleteStuffIds.Value)
        {
          DeleteStuffProductionFaultType(
                        productionFaultTypeId: productionFaultType.Id,
                        stuffId: stuffId);
        }




      return productionFaultType;

    }
    #endregion





    #region DeleteProcess
    public void DeleteProductionFaultTypeProcess(int id)
    {

      var productionFaultType = GetProductionFaultType(id: id);
      #region StuffProductionFaultType
      var stuffProductionFaultTypes = GetStuffProductionFaultTypes(
              selector: e => e,
              productionFaultTypeId: id);
      foreach (var stuffProductionFaultType in stuffProductionFaultTypes)
      {
        DeleteStuffProductionFaultType(stuffProductionFaultType: stuffProductionFaultType);
      }
      #endregion

      repository.Delete(productionFaultType);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ProductionFaultTypeResult> SortProductionFaultTypeResult(
        IQueryable<ProductionFaultTypeResult> query,
        SortInput<ProductionFaultTypeSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProductionFaultTypeSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ProductionFaultTypeSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case ProductionFaultTypeSortType.OperationTitle:
          return query.OrderBy(a => a.OperationTitle, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<ProductionFaultTypeResult> SearchProductionFaultTypeResult(
        IQueryable<ProductionFaultTypeResult> query,
        string searchText, AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.Title.Contains(searchText) ||
                item.OperationTitle.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<ProductionFaultType, ProductionFaultTypeResult>> ToProductionFaultTypeResult =
                productionFaultType => new ProductionFaultTypeResult
                {
                  Id = productionFaultType.Id,
                  Title = productionFaultType.Title,
                  OperationId = productionFaultType.OperationId,
                  OperationTitle = productionFaultType.Operation.Title,
                  RowVersion = productionFaultType.RowVersion
                };
    #endregion
    #region ToFullResult
    public Expression<Func<ProductionFaultType, ProductionFaultTypeFullResult>> ToFullProductionFaultTypeResult =
        productionFaultType => new ProductionFaultTypeFullResult
        {
          Id = productionFaultType.Id,
          Title = productionFaultType.Title,
          OperationId = productionFaultType.OperationId,
          OperationTitle = productionFaultType.Operation.Title,
          StuffProductionFaultTypes = productionFaultType.StuffProductionFaultTypes.AsQueryable().Select(App.Internals.Production.ToStuffProductionFaultTypeResult),
          RowVersion = productionFaultType.RowVersion
        };
    #endregion

  }
}


