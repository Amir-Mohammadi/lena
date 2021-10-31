using System;
using System.Linq;
using System.Linq.Expressions;
// using LinqKit;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.WarehouseManagement.UncommitedTransaction;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    public const double Threshold = -0.00000001;
    #region Add
    internal TransactionBatch AddTransactionBatch()
    {

      var transactionBatch = repository.Create<TransactionBatch>();
      transactionBatch.UserId = App.Providers.Security.CurrentLoginData.UserId;
      transactionBatch.DateTime = DateTime.Now.ToUniversalTime();
      repository.Add(transactionBatch);
      return transactionBatch;
    }
    #endregion
    #region Gets
    internal IQueryable<TransactionBatch> GetTransactionBatchs(
        TValue<int> id = null,
        TValue<DateTime> dateTime = null,
        TValue<int> userId = null,
        TValue<int> baseEntityId = null)
    {

      var query = repository.GetQuery<TransactionBatch>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (dateTime != null)
        query = query.Where(i => i.DateTime == dateTime);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (baseEntityId != null)
        query = query.Where(i => i.BaseEntity.Id == baseEntityId);
      return query;
    }
    #endregion
    #region Get
    internal TransactionBatch GetTransactionBatch(int id)
    {

      var transactionBatch = GetTransactionBatchs(id: id).FirstOrDefault();
      if (transactionBatch == null)
        throw new TransactionBatchNotFoundException(id);
      return transactionBatch;
    }
    #endregion
    #region RemoveTransactionBatch
    public void RemoveTransactionBatchProcess(int oldTransactionBathId, int? newTransactionBatchId)
    {

      var baseTransactions = GetBaseTransactions(selector: e => e,
                    transactionBatchId: oldTransactionBathId)


                .OrderByDescending(i => i.Id);
      foreach (var baseTransaction in baseTransactions)
      {
        RemoveBaseTransaction(
                      baseTransaction: baseTransaction,
                      rowVersion: baseTransaction.RowVersion,
                      transactionBatchId: newTransactionBatchId);
      }
    }
    #endregion
    #region CheckTransactionBatch
    internal void CheckTransactionBatch()
    {

      var transactions = App.Providers.UncommitedTransactionAgent.GetReport();
      transactions = transactions.Where(i => i.TransactionLevel != TransactionLevel.Plan).ToList();
      if (!transactions.Any()) return;
      #region Check warehouse limitations
      foreach (var item in transactions.Where(i => i.Factor > 0 && i.WarehouseId != null))
      {
        #region CheckWarehouseTransactionLevel
        CheckWarehouseTransactionLevel(
            warehouseId: item.WarehouseId,
            transactionLevel: item.TransactionLevel);
        #endregion
        #region CheckWarehouseStuffType
        var stuff = App.Internals.SaleManagement.GetStuff(
            id: item.StuffId);
        CheckWarehouseStuffType(
                  warehouseId: item.WarehouseId,
                  stuffType: stuff.StuffType);
        #endregion
      }
      #endregion
      #region GetQuery
      var warehouseTransactions = GetWarehouseTransactions(
              selector: e => e);
      var predicate = PredicateBuilder.New<BaseTransaction>(false);
      var distinctTransactions = transactions.Select(i => new { i.StuffId, i.WarehouseId, i.StuffSerialCode }).Distinct();
      foreach (var transaction in distinctTransactions)
      {
        var innerPredicate = PredicateBuilder.New<BaseTransaction>(true);
        innerPredicate = innerPredicate.And(i => i.StuffId == transaction.StuffId);
        if (transaction.WarehouseId != null)
          innerPredicate = innerPredicate.And(i => i.WarehouseId == transaction.WarehouseId);
        if (transaction.StuffSerialCode != null)
          innerPredicate = innerPredicate.And(i => i.StuffSerialCode == transaction.StuffSerialCode);
        predicate = predicate.Or(innerPredicate);
      }
      warehouseTransactions = warehouseTransactions.Where(predicate);
      var query = from transaction in warehouseTransactions
                  select new
                  {
                    StuffId = transaction.StuffId,
                    StuffSerialCode = transaction.StuffSerialCode,
                    WarehouseId = transaction.WarehouseId,
                    TransactionLevel = transaction.TransactionType.TransactionLevel,
                    Factor = transaction.TransactionType.Factor,
                    Amount = transaction.Amount,
                    UnitId = transaction.UnitId,
                    UnitConversionRatio = transaction.Unit.ConversionRatio,
                    DecimalDigitCount = transaction.Unit.DecimalDigitCount
                  };
      #endregion
      if (App.Providers.Storage.CheckTransactionBatchNegativeStuffValues)
      {
        #region Group With TransactionLevel and Warehouse
        var totalEqualZero = (from item in query
                              group item by new
                              {
                                item.StuffId,
                                item.WarehouseId,
                                item.TransactionLevel
                              }
            into itemGroups
                              select new CheckTransactionBatchInfo
                              {
                                StuffId = itemGroups.Key.StuffId,
                                WarehouseId = itemGroups.Key.WarehouseId,
                                TransactionLevel = itemGroups.Key.TransactionLevel,
                                Total = itemGroups.Sum(i => Math.Round(i.Amount * (int)i.Factor * i.UnitConversionRatio, i.DecimalDigitCount + 1))
                              });
        if (totalEqualZero.Any(i => i.Total < Threshold))
        {
          throw new CheckTransactionBatchException(totalEqualZero.Where(i => i.Total < Threshold).ToList());
        }
        #endregion
      }
      if (App.Providers.Storage.CheckTransactionBatchNegativeStuffSerialValues)
      {
        #region Group With StuffSerialCode and TransactionLevel and Warehouse
        var groupQuery = from item in query
                         group item by new
                         {
                           item.StuffId,
                           item.StuffSerialCode,
                           item.WarehouseId,
                           item.TransactionLevel,
                         }
            into itemGroups
                         select new CheckTransactionBatchInfo
                         {
                           StuffId = itemGroups.Key.StuffId,
                           StuffSerialCode = itemGroups.Key.StuffSerialCode,
                           WarehouseId = itemGroups.Key.WarehouseId,
                           TransactionLevel = itemGroups.Key.TransactionLevel,
                           Total = itemGroups.Sum(i => Math.Round(i.Amount * (int)i.Factor * i.UnitConversionRatio, i.DecimalDigitCount + 1))
                         };
        groupQuery = groupQuery.Where(i => i.StuffSerialCode != null);
        if (groupQuery.Any(i => i.Total < Threshold))
        {
          throw new CheckTransactionBatchSerialLevelException(groupQuery.Where(i => i.Total < Threshold).ToList());
        }
        #endregion
      }
      if (App.Providers.Storage.CheckTransactionBatchFragmentedSerials)
      {
        #region Group With TransactionLevel and Warehouse and serial for check SerialFragmented
        var serialGroupedQuery = from item in query
                                 where item.StuffSerialCode != null
                                 group item by new
                                 {
                                   item.StuffId,
                                   item.WarehouseId,
                                   item.TransactionLevel,
                                   item.StuffSerialCode,
                                 }
            into itemGroups
                                 where itemGroups.Sum(u => Math.Round(u.Amount * (int)u.Factor * u.UnitConversionRatio, u.DecimalDigitCount + 1)) > Math.Abs(Threshold)
                                 select new
                                 {
                                   StuffId = itemGroups.Key.StuffId,
                                   WarehouseId = itemGroups.Key.WarehouseId,
                                   TransactionLevel = itemGroups.Key.TransactionLevel,
                                   StuffSerialCode = itemGroups.Key.StuffSerialCode,
                                   items = itemGroups.ToList().Sum(u => Math.Round(u.Amount * (int)u.Factor * u.UnitConversionRatio, u.DecimalDigitCount + 1)),
                                   its = itemGroups.ToList()
                                 };
        var notPartitionedSerial = from item in serialGroupedQuery.ToList()
                                   group item by new
                                   {
                                     StuffId = item.StuffId,
                                     StuffSerialCode = item.StuffSerialCode
                                   }
                  into itemGroups
                                   select new
                                   {
                                     StuffId = itemGroups.Key.StuffId,
                                     StuffSerialCode = itemGroups.Key.StuffSerialCode,
                                     Count = itemGroups.Count(),
                                     items = itemGroups.ToList()
                                   };
        var l = notPartitionedSerial.ToList();
        if (notPartitionedSerial.Any(i => i.Count > 1))
        {
          var infos = notPartitionedSerial
                .Where(i => i.Count > 1)
                .Select(i => new CheckTransactionBatchInfo
                {
                  StuffId = i.StuffId,
                  StuffSerialCode = i.StuffSerialCode
                });
          throw new CheckTransactionBatchSerialFragmentedException(infos.ToList());
        }
        #endregion
      }
    }
    #endregion
  }
}