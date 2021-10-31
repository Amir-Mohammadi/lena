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
using lena.Models.Accounting.FinancialTransactionType;
using lena.Services.Internals.Accounting.Exception;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Add Process
    internal FinancialTransactionType AddFinancialTransactionTypeProcess(
        string title,
        TransactionTypeFactor factor,
        FinancialTransactionLevel transactionLevel,
        int? rollbackFinancialTransactionTypeId)
    {
      var transactionTypes = GetFinancialTransactionTypes(selector: i => i.Id)
               ;
      var newId = transactionTypes.Any() ? transactionTypes.Max() : 0;
      newId++;
      var financialTransactionType = AddFinancialTransactionType(id: newId,
                    title: title,
                    factor: factor,
                    rollbackFinancialTransactionTypeId: rollbackFinancialTransactionTypeId,
                    transactionLevel: transactionLevel);
      return financialTransactionType;
    }
    #endregion
    #region Add
    internal FinancialTransactionType AddFinancialTransactionType(
        int id,
        string title,
        TransactionTypeFactor factor,
        FinancialTransactionLevel transactionLevel,
        int? rollbackFinancialTransactionTypeId)
    {
      var transactionType = repository.Create<FinancialTransactionType>();
      transactionType.Id = id;
      transactionType.Title = title;
      transactionType.Factor = factor;
      transactionType.FinancialTransactionLevel = transactionLevel;
      if (rollbackFinancialTransactionTypeId != null)
        transactionType.RollbackFinancialTransactionType = GetFinancialTransactionType(id: rollbackFinancialTransactionTypeId.Value);
      repository.Add(transactionType);
      return transactionType;
    }
    #endregion
    #region Edit
    internal FinancialTransactionType EditFinancialTransactionType(byte[] rowVersion, int id, TValue<string> title = null,
        TValue<TransactionTypeFactor> factor = null, TValue<FinancialTransactionLevel> financialTransactionLevel = null,
        TValue<int?> rollbackFinancialTransactionTypeId = null, FinancialTransactionType financialTransactionType = null)
    {
      var transactionType = financialTransactionType ?? GetFinancialTransactionType(id: id);
      if (title != null)
        transactionType.Title = title;
      if (factor != null)
        transactionType.Factor = factor;
      if (financialTransactionLevel != null)
        transactionType.FinancialTransactionLevel = financialTransactionLevel;
      if (rollbackFinancialTransactionTypeId != null)
        transactionType.RollbackFinancialTransactionTypeId = rollbackFinancialTransactionTypeId;
      repository.Update(transactionType, rowVersion);
      return transactionType;
    }
    #endregion
    #region Delete
    internal void DeleteFinancialTransactionType(int id)
    {
      var financialTransactionType = GetFinancialTransactionType(id: id);
      if (financialTransactionType.FinancialTransactions.Any())
      {
        throw new FinancialTransactionTypeIsUsedBySomeTransactionsException();
      }
      repository.Delete(financialTransactionType);
    }
    #endregion
    #region Get
    internal FinancialTransactionType GetFinancialTransactionType(int id) => GetFinancialTransactionType(selector: e => e, id: id);
    public TResult GetFinancialTransactionType<TResult>(
        Expression<Func<FinancialTransactionType, TResult>> selector,
        int id)
    {
      var transactionType = GetFinancialTransactionTypes(selector: selector,
               id: id).FirstOrDefault();
      if (transactionType == null)
        throw new FinancialTransactionTypeNotFoundException(id);
      return transactionType;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetFinancialTransactionTypes<TResult>(
        Expression<Func<FinancialTransactionType, TResult>> selector,
        TValue<int> id = null, TValue<string> title = null,
        TValue<TransactionTypeFactor> factor = null, TValue<FinancialTransactionLevel> financialTransactionLevel = null, TValue<int?> rollbackFinancialTransactionTypeId = null
        )
    {
      var query = repository.GetQuery<FinancialTransactionType>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (title != null)
        query = query.Where(i => i.Title == title);
      if (factor != null)
        query = query.Where(i => i.Factor == factor);
      if (financialTransactionLevel != null)
        query = query.Where(i => i.FinancialTransactionLevel == financialTransactionLevel);
      if (rollbackFinancialTransactionTypeId != null)
        query = query.Where(i => i.RollbackFinancialTransactionTypeId == rollbackFinancialTransactionTypeId);
      return query.Select(selector);
    }
    #endregion
    #region Search
    public IQueryable<FinancialTransactionTypeResult> SearchFinancialTransactionTypeResult(
         IQueryable<FinancialTransactionTypeResult> query,
         string searchText
        )
    {
      if (!string.IsNullOrEmpty(searchText))
      {
        query = query.Where(i =>
            i.Title.Contains(searchText) ||
            i.RollbackFinancialTransactionTypeTitle.Contains(searchText) ||
            i.FinancialTransactionType.ToString().Contains(searchText) ||
            i.Factor.ToString().Contains(searchText));
      }
      return query;
    }
    #endregion
    #region Sort
    internal IOrderedQueryable<FinancialTransactionTypeResult> SortTransacrtionTypeResult(
        IQueryable<FinancialTransactionTypeResult> input, SortInput<FinancialTransactionTypeSortType> options)
    {
      switch (options.SortType)
      {
        case FinancialTransactionTypeSortType.Title:
          return input.OrderBy(a => a.Title, options.SortOrder);
        case FinancialTransactionTypeSortType.Factor:
          return input.OrderBy(a => a.Factor, options.SortOrder);
        case FinancialTransactionTypeSortType.TransactionType:
          return input.OrderBy(a => a.FinancialTransactionType, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region To Combo Result
    public Expression<Func<FinancialTransactionType, FinancialTransactionTypeComboResult>> ToFinancialTransactionTypeComboResult =>
    item => new FinancialTransactionTypeComboResult
    {
      Id = item.Id,
      Title = item.Title
    };
    #endregion
    #region To Result Query
    public Expression<Func<FinancialTransactionType, FinancialTransactionTypeResult>> ToFinancialTransactionTypeResult =>
    item => new FinancialTransactionTypeResult
    {
      Id = item.Id,
      Title = item.Title,
      Factor = item.Factor,
      FinancialTransactionType = item.FinancialTransactionLevel,
      RollbackFinancialTransactionTypeId = item.RollbackFinancialTransactionTypeId,
      RollbackFinancialTransactionTypeTitle = item.RollbackFinancialTransactionType.Title,
      RowVersion = item.RowVersion,
    };
    #endregion
  }
}