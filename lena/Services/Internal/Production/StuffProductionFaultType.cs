
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Production.StuffProductionFaultType;
using System;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Get
    public StuffProductionFaultType GetStuffProductionFaultType(int productionFaultTypeId, int stuffId)
        => GetStuffProductionFaultType(selector: e => e, productionFaultTypeId: productionFaultTypeId, stuffId: stuffId);
    public TResult GetStuffProductionFaultType<TResult>(
        Expression<Func<StuffProductionFaultType, TResult>> selector,
        int productionFaultTypeId, int stuffId)
    {

      var stuffProductionFaultType = GetStuffProductionFaultTypes(
                   selector: selector,
                   productionFaultTypeId: productionFaultTypeId,
                   stuffId: stuffId)


               .FirstOrDefault();
      if (stuffProductionFaultType == null)
        throw new StuffProductionFaultTypeNotFoundException(productionFaultTypeId: productionFaultTypeId, stuffId: stuffId);
      return stuffProductionFaultType;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStuffProductionFaultTypes<TResult>(
            Expression<Func<StuffProductionFaultType, TResult>> selector,
            TValue<int> productionFaultTypeId = null,
            TValue<int> stuffId = null,
            TValue<int> operationId = null
            )
    {

      var query = repository.GetQuery<StuffProductionFaultType>();
      if (productionFaultTypeId != null) query = query.Where(x => x.ProductionFaultTypeId == productionFaultTypeId);
      if (stuffId != null) query = query.Where(x => x.StuffId == stuffId);
      if (operationId != null) query = query.Where(x => x.ProductionFaultType.OperationId == operationId);

      return query.Select(selector);
    }
    #endregion
    #region Add
    public StuffProductionFaultType AddStuffProductionFaultType(
                int productionFaultTypeId,
                int stuffId
                )
    {


      var stuffProductionFaultType = repository.Create<StuffProductionFaultType>();
      stuffProductionFaultType.StuffId = stuffId;
      stuffProductionFaultType.ProductionFaultTypeId = productionFaultTypeId;
      var getStuffProductionFaultType = GetStuffProductionFaultTypes(selector: e => e, stuffId: stuffProductionFaultType.StuffId, productionFaultTypeId: stuffProductionFaultType.ProductionFaultTypeId);
      if (getStuffProductionFaultType.Any(i => i.StuffId == stuffProductionFaultType.StuffId && i.ProductionFaultTypeId == stuffProductionFaultType.ProductionFaultTypeId))

      {
        throw new StuffProductionFaultTypeExistException(stuffProductionFaultType.StuffId, stuffProductionFaultType.ProductionFaultTypeId);
      }
      repository.Add(stuffProductionFaultType);
      return stuffProductionFaultType;
    }
    #endregion

    #region Delete
    public void DeleteStuffProductionFaultType(int productionFaultTypeId, int stuffId)
    {

      var stuffProductionFaultType = GetStuffProductionFaultType(productionFaultTypeId: productionFaultTypeId, stuffId: stuffId); ; DeleteStuffProductionFaultType(stuffProductionFaultType);

    }
    public void DeleteStuffProductionFaultType(StuffProductionFaultType stuffProductionFaultType)
    {

      repository.Delete(stuffProductionFaultType);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffProductionFaultTypeResult> SortStuffProductionFaultTypeResult(
        IQueryable<StuffProductionFaultTypeResult> query,
        SortInput<StuffProductionFaultTypeSortType> sort)
    {
      switch (sort.SortType)
      {
        case StuffProductionFaultTypeSortType.OperationCode: return query.OrderBy(a => a.OperationCode, sort.SortOrder);
        case StuffProductionFaultTypeSortType.OperationTitle: return query.OrderBy(a => a.OperationTitle, sort.SortOrder);
        case StuffProductionFaultTypeSortType.ProductionFaultTypeTitle: return query.OrderBy(a => a.ProductionFaultTypeTitle, sort.SortOrder);
        case StuffProductionFaultTypeSortType.ProductionFaultTypeId: return query.OrderBy(a => a.ProductionFaultTypeId, sort.SortOrder);
        case StuffProductionFaultTypeSortType.StuffCode: return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case StuffProductionFaultTypeSortType.StuffName: return query.OrderBy(a => a.StuffName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<StuffProductionFaultTypeResult> SearchStuffProductionFaultTypeResult(
        IQueryable<StuffProductionFaultTypeResult> query,
        string searchText, AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.StuffCode.Contains(searchText) ||
                    item.StuffName.Contains(searchText) ||
                    item.OperationCode.Contains(searchText) ||
                    item.OperationTitle.Contains(searchText) ||
                    item.StuffId.ToString().Contains(searchText) ||
                    item.OperationId.ToString().Contains(searchText) ||
                    item.ProductionFaultTypeId.ToString().Contains(searchText) ||
                    item.ProductionFaultTypeTitle.Contains(searchText)

                select item;
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<StuffProductionFaultType, StuffProductionFaultTypeResult>> ToStuffProductionFaultTypeResult =
                stuffProductionFaultType => new StuffProductionFaultTypeResult
                {
                  StuffId = stuffProductionFaultType.StuffId,
                  ProductionFaultTypeId = stuffProductionFaultType.ProductionFaultTypeId,
                  StuffCode = stuffProductionFaultType.Stuff.Code,
                  StuffName = stuffProductionFaultType.Stuff.Name,
                  ProductionFaultTypeTitle = stuffProductionFaultType.ProductionFaultType.Title,
                  OperationId = stuffProductionFaultType.ProductionFaultType.OperationId,
                  OperationCode = stuffProductionFaultType.ProductionFaultType.Operation.Code,
                  OperationTitle = stuffProductionFaultType.ProductionFaultType.Operation.Title,
                  RowVersion = stuffProductionFaultType.RowVersion
                };

    public Expression<Func<StuffProductionFaultType, StuffProductionFaultTypeComboResult>> ToStuffProductionFaultTypeComboResult =
              stuffProductionFaultType => new StuffProductionFaultTypeComboResult
              {
                StuffId = stuffProductionFaultType.StuffId,
                ProductionFaultTypeId = stuffProductionFaultType.ProductionFaultTypeId,
                FaultName = stuffProductionFaultType.ProductionFaultType.Title,
                StuffCode = stuffProductionFaultType.Stuff.Code,
                StuffName = stuffProductionFaultType.Stuff.Name,
                RowVersion = stuffProductionFaultType.RowVersion
              };
    #endregion

  }
}
