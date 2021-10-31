using System;
using System.Linq;
using lena.Services.Core;
using lena.Models.Common;
using lena.Domains;
using core.Models;
namespace lena.Services.Common.Utilities
{
  public static class Extentions
  {
    public static IQueryable<T> FilterSaveLog<T>(this IQueryable<T> query, TValue<int> userId = null,
        TValue<DateTime> fromDateTime = null, TValue<DateTime> toDateTime = null) where T : IHasSaveLog
    {
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);
      return query;
    }
    public static void AddSaveLog(this IHasSaveLog hasSaveLog)
    {
      hasSaveLog.UserId = App.Providers.Security.CurrentLoginData.UserId;
      hasSaveLog.DateTime = DateTime.Now.ToUniversalTime();
    }
    public static void AddDescription(this IHasDescription hasDescription, string description)
    {
      hasDescription.Description = description;
    }
    public static IQueryable<T> FilterDescription<T>(this IQueryable<T> query, TValue<string> description = null) where T : IHasDescription
    {
      if (description != null)
        query = query.Where(i => i.Description == description);
      return query;
    }
    public static void EditDescription(this IHasDescription hasDescription, TValue<string> description = null)
    {
      if (description != null)
        hasDescription.Description = description;
    }
    public static void AddCode(this IHasCode hasCode, string code)
    {
      hasCode.Code = code;
    }
    public static IQueryable<T> FilterCode<T>(this IQueryable<T> query, TValue<string> code = null) where T : IHasCode
    {
      if (code != null)
        query = query.Where(i => i.Code == code);
      return query;
    }
    public static void EditCode(this IHasCode hasCode, TValue<string> code = null)
    {
      if (code != null)
        hasCode.Code = code;
    }
    public static void AddRemovable(this IRemovable removable)
    {
      removable.IsDelete = false;
    }
    public static IQueryable<T> FilterIsDelete<T>(this IQueryable<T> query, TValue<bool> isDelete = null) where T : IRemovable
    {
      if (isDelete != null)
        query = query.Where(i => i.IsDelete == isDelete);
      return query;
    }
    public static void EditRemovable(this IRemovable removable, TValue<bool> isDelete = null)
    {
      if (removable != null)
        removable.IsDelete = isDelete;
    }
    public static void AddTransactionBatch(this IHasTransaction hasTransaction, int transactionBatchId)
    {
      hasTransaction.TransactionBatchId = transactionBatchId;
    }
    public static IQueryable<T> FilterTransactionBatch<T>(this IQueryable<T> query, TValue<int> transactionBatchId = null) where T : IHasTransaction
    {
      if (transactionBatchId != null)
        query = query.Where(i => i.TransactionBatchId == transactionBatchId);
      return query;
    }
    public static void EditTransactionBatch(this IHasTransaction hasTransaction, TValue<int> transactionBatchId = null)
    {
      if (transactionBatchId != null)
        hasTransaction.TransactionBatchId = transactionBatchId;
    }
    public static void AddFinancialTransactionBatch(this IHasFinancialTransaction hasFinancialTransaction, int financialTransactionBatchId)
    {
      hasFinancialTransaction.FinancialTransactionBatchId = financialTransactionBatchId;
    }
    public static IQueryable<T> FilterFinancialTransactionBatch<T>(this IQueryable<T> query, TValue<int> financialTransactionBatchId = null) where T : IHasFinancialTransaction
    {
      if (financialTransactionBatchId != null)
        query = query.Where(i => i.FinancialTransactionBatchId == financialTransactionBatchId);
      return query;
    }
    public static void EditFinancialTransactionBatch(this IHasFinancialTransaction hasFinancialTransaction, TValue<int> financialTransactionBatchId = null)
    {
      if (financialTransactionBatchId != null)
        hasFinancialTransaction.FinancialTransactionBatchId = financialTransactionBatchId;
    }
  }
  public static class QueryableExtentions
  {
    //public static IQueryable<T> Where<T>(this IQueryable<T> source, AdvanceSearchItem[] advanceSearchItems)
    //{
    //    if (advanceSearchItems != null)
    //    {
    //        //var stringExpression = string.Join(" && ", advanceSearchItems.Select(i => i.ToString()));
    //        //source = source.Where(stringExpression);
    //        foreach (var item in advanceSearchItems)
    //        {
    //            var exp = item.ToString();
    //            if (exp.Contains("@"))
    //            {
    //                if (item.Value != null)
    //                    source = source.Where(exp, item.Value);
    //            }
    //            else
    //                source = source.Where(exp);
    //        }
    //    }
    //    return source;
    //}
  }
}