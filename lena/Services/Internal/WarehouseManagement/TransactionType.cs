using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.WarehouseManagement.TransactionType;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add Process
    internal TransactionType AddTransactionTypeProcess(
        string name,
        TransactionTypeFactor factor,
        TransactionLevel transactionLevel,
        short? rollbackTransactionTypeId)
    {
      var transactionTypes = GetTransactionTypes(selector: i => i.Id);
      short newId = (short)(transactionTypes.Any() ? transactionTypes.Max() : 0);
      newId++;
      return AddTransactionType(
              id: newId,
              name: name,
              factor: factor,
              rollbackTransactionTypeId: rollbackTransactionTypeId,
              transactionLevel: transactionLevel);
    }
    #endregion
    #region Add
    internal TransactionType AddTransactionType(
        short id,
        string name,
        TransactionTypeFactor factor,
        TransactionLevel transactionLevel,
        short? rollbackTransactionTypeId)
    {
      var transactionType = repository.Create<TransactionType>();
      transactionType.Id = id;
      transactionType.Name = name;
      transactionType.Factor = factor;
      transactionType.TransactionLevel = transactionLevel;
      if (rollbackTransactionTypeId != null)
        transactionType.RollbackTransactionType = GetTransactionType(id: rollbackTransactionTypeId.Value);
      repository.Add(transactionType);
      return transactionType;
    }
    #endregion
    #region Edit
    internal TransactionType EditTransactionType(
        byte[] rowVersion,
        short id,
        TValue<string> name = null,
        TValue<TransactionTypeFactor> factor = null,
        TValue<TransactionLevel> transactionLevel = null,
        TValue<short?> rollbackTransactionTypeId = null,
        TransactionType transactionTypeEntity = null)
    {
      var transactionType = transactionTypeEntity ?? GetTransactionType(id: id);
      if (name != null)
        transactionType.Name = name;
      if (factor != null)
        transactionType.Factor = factor;
      if (transactionLevel != null)
        transactionType.TransactionLevel = transactionLevel;
      if (rollbackTransactionTypeId != null)
        transactionType.RollbackTransactionTypeId = rollbackTransactionTypeId;
      repository.Update(transactionType, rowVersion);
      return transactionType;
    }
    #endregion
    #region Delete
    internal void DeleteTransactionType(short id)
    {
      var transactionType = GetTransactionType(id: id);
      if (transactionType.BaseTransactions.Any())
      {
        throw new TransactionTypeIsUsedBySomeTransactionsException();
      }
      repository.Delete(transactionType);
    }
    #endregion
    #region Get
    internal TransactionType GetTransactionType(short id) => GetTransactionType(selector: e => e, id: id);
    public TResult GetTransactionType<TResult>(
        Expression<Func<TransactionType, TResult>> selector,
        short id)
    {
      var transactionType = GetTransactionTypes(
                selector: selector,
                id: id)
            .FirstOrDefault();
      if (transactionType == null)
        throw new TransactionTypeNotFoundException(id);
      return transactionType;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetTransactionTypes<TResult>(
        Expression<Func<TransactionType, TResult>> selector,
        TValue<int> id = null, TValue<string> name = null,
        TValue<TransactionTypeFactor> factor = null, TValue<TransactionLevel> transactionLevel = null, TValue<int?> rollbackTransactionTypeId = null
        )
    {
      var query = repository.GetQuery<TransactionType>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (name != null)
        query = query.Where(i => i.Name == name);
      if (factor != null)
        query = query.Where(i => i.Factor == factor);
      if (transactionLevel != null)
        query = query.Where(i => i.TransactionLevel == transactionLevel);
      if (rollbackTransactionTypeId != null)
        query = query.Where(i => i.RollbackTransactionTypeId == rollbackTransactionTypeId);
      return query.Select(selector);
    }
    #endregion
    #region Search
    public IQueryable<TransactionTypeResult> SearchTransactionTypeResult(
         IQueryable<TransactionTypeResult> query,
         string searchText,
         AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (!string.IsNullOrEmpty(searchText))
      {
        query = query.Where(i =>
            i.Name.Contains(searchText) ||
            i.RollbackTransactionTypeName.Contains(searchText) ||
            i.TransactionType.ToString().Contains(searchText) ||
            i.Factor.ToString().Contains(searchText));
      }
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Sort
    internal IOrderedQueryable<TransactionTypeResult> SortTransacrtionTypeResult(
        IQueryable<TransactionTypeResult> input, SortInput<TransactionTypeSortType> options)
    {
      switch (options.SortType)
      {
        case TransactionTypeSortType.Name:
          return input.OrderBy(a => a.Name, options.SortOrder);
        case TransactionTypeSortType.Factor:
          return input.OrderBy(a => a.Factor, options.SortOrder);
        case TransactionTypeSortType.TransactionType:
          return input.OrderBy(a => a.TransactionType, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region To Combo Result
    public Expression<Func<TransactionType, TransactionTypeComboResult>> ToTransactionTypeComboResult =>
    item => new TransactionTypeComboResult
    {
      Id = item.Id,
      Name = item.Name
    };
    #endregion
    #region To Result Query
    public Expression<Func<TransactionType, TransactionTypeResult>> ToTransactionTypeResult =>
    item => new TransactionTypeResult
    {
      Id = item.Id,
      Name = item.Name,
      Factor = item.Factor,
      TransactionType = item.TransactionLevel,
      RollbackTransactionTypeId = item.RollbackTransactionTypeId,
      RollbackTransactionTypeName = item.RollbackTransactionType.Name,
      RowVersion = item.RowVersion,
    };
    #endregion
  }
}