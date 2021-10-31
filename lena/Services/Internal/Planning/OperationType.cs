using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Planning.OperationType;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Get
    public OperationType GetOperationType(int id) => GetOperationType(selector: e => e, id: id);
    public TResult GetOperationType<TResult>(
        Expression<Func<OperationType, TResult>> selector,
        int id)
    {

      var operationType = GetOperationTypes(selector: selector, id: id)


                .FirstOrDefault();
      if (operationType == null)
        throw new OperationTypeNotFoundException(id);
      return operationType;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetOperationTypes<TResult>(
        Expression<Func<OperationType, TResult>> selector,
        TValue<int> id = null,
        TValue<string> title = null)
    {

      var query = repository.GetQuery<OperationType>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (title != null)
        query = query.Where(x => x.Title == title);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public OperationType AddOperationType(
        string title,
        byte[] symbol)
    {

      var entity = repository.Create<OperationType>();
      entity.Title = title;
      entity.Symbol = symbol;
      repository.Add(entity);
      return entity;
    }
    #endregion
    #region Edit
    public OperationType EditOperationType(
        int id,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<byte[]> symbol = null)
    {

      var operationType = GetOperationType(id: id);
      if (title != null)
        operationType.Title = title;
      if (symbol != null)
        operationType.Symbol = symbol;
      repository.Update(rowVersion: rowVersion, entity: operationType);
      return operationType;
    }
    #endregion
    #region Delete
    public void DeleteOperationType(int id)
    {

      var operationType = GetOperationType(id: id);
      repository.Delete(operationType);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<OperationTypeResult> SortOperationTypeResult(
        IQueryable<OperationTypeResult> query,
        SortInput<OperationTypeSortType> sort)
    {
      switch (sort.SortType)
      {
        case OperationTypeSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case OperationTypeSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<OperationTypeResult> SearchOperationTypeResult(
        IQueryable<OperationTypeResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.Title.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<OperationType, OperationTypeResult>> ToOperationTypeResult =
        operationType => new OperationTypeResult
        {
          Id = operationType.Id,
          Title = operationType.Title,
          Symbol = operationType.Symbol,
          RowVersion = operationType.RowVersion
        };
    #endregion
  }
}
