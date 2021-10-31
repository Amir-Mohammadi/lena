using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Planning.Operation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Get
    public Operation GetOperation(string code) => GetOperation(selector: e => e, code: code);
    public TResult GetOperation<TResult>(
        Expression<Func<Operation, TResult>> selector,
        string code)
    {

      var entity = GetOperations(
                selector: selector,
                code: code)


                .FirstOrDefault();
      if (entity == null)
        throw new OperationNotFoundException(code);
      return entity;
    }

    public Operation GetOperation(int id) => GetOperation(selector: e => e, id: id);
    public TResult GetOperation<TResult>(
        Expression<Func<Operation, TResult>> selector,
        int id)
    {

      var entity = GetOperations(
                selector: selector,
                id: id)


                .FirstOrDefault();
      if (entity == null)
        throw new OperationNotFoundException(id);
      return entity;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetOperations<TResult>(
        Expression<Func<Operation, TResult>> selector,
        TValue<int> id = null,
        TValue<string> title = null,
        TValue<string> code = null,
        TValue<string[]> codes = null,
        TValue<bool> isQualityControl = null,
        TValue<bool> isCorrective = null,
        TValue<int> operationTypeId = null,
        TValue<string> description = null)
    {

      var query = repository.GetQuery<Operation>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (title != null)
        query = query.Where(x => x.Title == title);
      if (isQualityControl != null)
        query = query.Where(x => x.IsQualityControl == isQualityControl);
      if (isCorrective != null)
        query = query.Where(x => x.IsCorrective == isCorrective);
      if (description != null)
        query = query.Where(x => x.Description == description);
      if (code != null)
        query = query.Where(x => x.Code == code);
      if (codes != null && codes.Value.Length != 0)
        query = query.Where(x => codes.Value.Contains(x.Code));
      if (operationTypeId != null)
        query = query.Where(x => x.OperationTypeId == operationTypeId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public Operation AddOperation(
        string title,
        string code,
        bool isQualityControl,
        bool isCorrective,
        byte operationTypeId,
        string description)
    {

      var entity = repository.Create<Operation>();
      entity.Title = title;
      entity.Code = code;
      entity.OperationTypeId = operationTypeId;
      entity.Description = description;
      entity.IsQualityControl = isQualityControl;
      entity.IsCorrective = isCorrective;
      repository.Add(entity);
      return entity;
    }
    #endregion
    #region Edit
    public Operation EditOperation(
        int id,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<string> code = null,
        TValue<bool> isCorrective = null,
        TValue<bool> isQualityControl = null,
        TValue<byte> operationTypeId = null,
        TValue<string> description = null)
    {

      var entity = GetOperation(id: id);
      if (title != null)
        entity.Title = title;
      if (code != null)
        entity.Code = code;
      if (isCorrective != null)
        entity.IsCorrective = isCorrective;
      if (isQualityControl != null)
        entity.IsQualityControl = isQualityControl;
      if (operationTypeId != null)
        entity.OperationTypeId = operationTypeId;
      if (description != null)
        entity.Description = description;
      repository.Update(rowVersion: rowVersion, entity: entity);
      return entity;
    }
    #endregion
    #region Delete
    public void DeleteOperation(int id)
    {

      var operation = GetOperation(id: id);
      repository.Delete(operation);
    }
    #endregion
    #region ToComboResult
    public Expression<Func<Operation, OperationComboResult>> ToOperationComboResult =
        entity => new OperationComboResult()
        {
          Id = entity.Id,
          Code = entity.Code,
          Title = entity.Title,
          IsQualityControl = entity.IsQualityControl,
          OperationTypeSymbol = entity.OperationType.Symbol,

        };
    #endregion
    #region ToResult
    public Expression<Func<Operation, OperationResult>> ToOperationResult =
        entity => new OperationResult
        {
          Id = entity.Id,
          Code = entity.Code,
          Title = entity.Title,
          IsQualityControl = entity.IsQualityControl,
          OperationTypeId = entity.OperationTypeId,
          OperationTypeTitle = entity.OperationType.Title,
          OperationTypeSymbol = entity.OperationType.Symbol,
          Description = entity.Description,
          IsCorrective = entity.IsCorrective,
          Barcode = "OP" + entity.Code,
          RowVersion = entity.RowVersion
        };
    #endregion
    #region Sort
    public IOrderedQueryable<OperationResult> SortOperationResult(IQueryable<OperationResult> query, SortInput<OperationSortType> sort)
    {
      switch (sort.SortType)
      {
        case OperationSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case OperationSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case OperationSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case OperationSortType.IsQualityControl:
          return query.OrderBy(a => a.IsQualityControl, sort.SortOrder);
        case OperationSortType.IsCorrective:
          return query.OrderBy(a => a.IsCorrective, sort.SortOrder);
        case OperationSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<OperationResult> SearchOperationResult(
        IQueryable<OperationResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
        bool? isQualityControl = null,
        bool? isCorrective = null
        )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(x => x.Code.Contains(searchText) ||
                                 x.Title.Contains(searchText) ||
                                 x.OperationTypeTitle.Contains(searchText) ||
                                 x.Description.Contains(searchText));
      }

      if (isQualityControl.HasValue)
      {
        query = query.Where(i => i.IsQualityControl == isQualityControl);
      }

      if (isCorrective.HasValue)
      {
        query = query.Where(i => i.IsCorrective == isCorrective);
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
  }
}
