using System;
using System.Linq;
using lena.Services.Core;
using System.Linq.Expressions;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Common;
using lena.Services.Common;
using lena.Domains;
using lena.Models.Accounting.FinancialTransactionBatch;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Add
    internal FinancialTransactionBatch AddFinancialTransactionBatch()
    {

      var financialTransactionBatch = repository.Create<FinancialTransactionBatch>();
      financialTransactionBatch.UserId = App.Providers.Security.CurrentLoginData.UserId;
      financialTransactionBatch.DateTime = DateTime.Now.ToUniversalTime();
      repository.Add(financialTransactionBatch);
      return financialTransactionBatch;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetFinancialTransactionBatches<TResult>(
        Expression<Func<FinancialTransactionBatch, TResult>> selector,
        TValue<int> id = null,
        TValue<DateTime> dateTime = null,
        TValue<int> userId = null,
        TValue<int> baseEntityId = null)
    {

      var query = repository.GetQuery<FinancialTransactionBatch>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (dateTime != null)
        query = query.Where(i => i.DateTime == dateTime);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (baseEntityId != null)
        query = query.Where(i => i.BaseEntity.Id == baseEntityId);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public FinancialTransactionBatch GetFinancialTransactionBatch(int id) => GetFinancialTransactionBatch(selector: e => e, id: id);
    internal TResult GetFinancialTransactionBatch<TResult>(Expression<Func<FinancialTransactionBatch, TResult>> selector, int id)
    {

      var financialTransactionBatch = GetFinancialTransactionBatches(selector: selector, id: id).FirstOrDefault();
      if (financialTransactionBatch == null)
        throw new FinancialTransactionBatchNotFoundException(id);
      return financialTransactionBatch;
    }
    #endregion
    #region ToResult
    internal Expression<Func<FinancialTransactionBatch, FinancialTransactionBatchResult>> ToFinancialTransactionBatchResult =
       financialTransactionBatch => new FinancialTransactionBatchResult
       {
         Id = financialTransactionBatch.Id,
         BaseEntityId = financialTransactionBatch.BaseEntity.Id,
         DateTime = financialTransactionBatch.DateTime,
         UserId = financialTransactionBatch.UserId,
         Description = financialTransactionBatch.Description,
         RowVersion = financialTransactionBatch.RowVersion
       };
    #endregion
    //#region RemoveFinancialTransactionBatch
    //public void RemoveFinancialTransactionBatchProcess(int oldFinancialTransactionBathId, int? newFinancialTransactionBatchId)
    //{
    //    
    //        var FinancialTransactions = GetFinancialTransactions(selector: e => e,
    //                financialTransactionBatchId: oldFinancialTransactionBathId)
    //            
    //            .OrderByDescending(i => i.Id);
    //        foreach (var financialTransaction in FinancialTransactions)
    //        {
    //            RemoveFinancialTransaction(
    //                    FinancialTransaction: financialTransaction,
    //                    rowVersion: financialTransaction.RowVersion,
    //                    financialTransactionBatchId: newFinancialTransactionBatchId)
    //                
    //;
    //        }
    //    });
    //}
    //#endregion
  }
}
