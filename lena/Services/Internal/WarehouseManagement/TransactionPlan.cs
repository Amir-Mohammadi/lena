using System;
using System.Linq;
using System.Linq.Expressions;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Services.Core;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    internal BaseTransaction AddTransactionPlanProcess(
        int transactionBatchId,
        DateTime effectDateTime,
        int stuffId,
        short? billOfMaterialVersion,
        long? stuffSerialCode,
        short transactionTypeId,
        double amount,
        byte unitId,
        string description,
        bool isEstimated,
        BaseTransaction referenceTransaction,
        short? warehouseFiscalPeriodId = null)
    {


      #region GetCurrentWarehouseFiscalPeriod
      if (warehouseFiscalPeriodId == null)
      {
        var currentWarehouseFiscalPeriodIds = GetWarehouseFiscalPeriods(
                  selector: e => e.Id,
                  isCurrent: true);

        if (!currentWarehouseFiscalPeriodIds.Any())
          throw new CurrentWarehouseFiscalPeriodIsNotDefinedException();

        if (currentWarehouseFiscalPeriodIds.Count() > 1)
          throw new CurrentWarehouseFiscalPeriodCanNotBeMoreThanOneException();

        warehouseFiscalPeriodId = currentWarehouseFiscalPeriodIds.FirstOrDefault();
      }
      #endregion

      #region WarehouseFiscalPeriod can not be closed
      var warehouseFiscalPeriod = GetWarehouseFiscalPeriod(id: warehouseFiscalPeriodId.Value);
      if (warehouseFiscalPeriod.IsClosed)
        throw new WarehouseFiscalPeriodIsClosedException(id: warehouseFiscalPeriodId.Value);
      #endregion

      var transactionPlan = repository.Create<BaseTransaction>();
      transactionPlan.IsDelete = false;
      transactionPlan.IsEstimated = isEstimated;
      transactionPlan.WarehouseFiscalPeriodId = (short)warehouseFiscalPeriodId;

      AddBaseTransaction(
                    baseTransaction: transactionPlan,
                    amount: amount,
                    effectDateTime: effectDateTime,
                    description: description,
                    stuffId: stuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    stuffSerialCode: stuffSerialCode,
                    unitId: unitId,
                    transactionTypeId: transactionTypeId,
                    transactionBatchId: transactionBatchId,
                    referenceTransaction: referenceTransaction);

      App.Providers.UncommitedTransactionAgent.Add(transactionPlan);

      return transactionPlan;
    }
    #endregion


    #region Edit
    internal BaseTransaction EditBaseTransaction(
        int id,
        byte[] rowVersion,
        TValue<double> amount = null,
        TValue<DateTime> effectDateTime = null,
        TValue<string> description = null,
        TValue<int> stuffId = null,
        TValue<short?> billOfMaterialVersion = null,
        TValue<long?> stuffSerialCode = null,
        TValue<byte> unitId = null,
        TValue<short> transactionTypeId = null,
        TValue<int> transactionBatchId = null,
        TValue<BaseTransaction> referenceTransaction = null,
        TValue<bool> isDelete = null)
    {

      var transactionPlan = GetBaseTransaction(id: id);
      return EditBaseTransaction(
                    transactionPlan: transactionPlan,
                    rowVersion: rowVersion,
                    amount: amount,
                    effectDateTime: effectDateTime,
                    description: description,
                    stuffId: stuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    stuffSerialCode: stuffSerialCode,
                    unitId: unitId,
                    transactionTypeId: transactionTypeId,
                    transactionBatchId: transactionBatchId,
                    referenceTransaction: referenceTransaction,
                    isDelete: isDelete);
    }
    internal BaseTransaction EditBaseTransaction(
        BaseTransaction transactionPlan,
        byte[] rowVersion,
        TValue<double> amount = null,
        TValue<DateTime> effectDateTime = null,
        TValue<string> description = null,
        TValue<int> stuffId = null,
        TValue<short?> billOfMaterialVersion = null,
        TValue<long?> stuffSerialCode = null,
        TValue<byte> unitId = null,
        TValue<short> transactionTypeId = null,
        TValue<int> transactionBatchId = null,
        TValue<BaseTransaction> referenceTransaction = null,
        TValue<bool> isDelete = null)
    {

      if (isDelete != null)
        transactionPlan.IsDelete = isDelete;
      EditBaseTransaction(
                baseTransaction: transactionPlan,
                rowVersion: rowVersion,
                amount: amount,
                effectDateTime: effectDateTime,
                description: description,
                stuffId: stuffId,
                billOfMaterialVersion: billOfMaterialVersion,
                stuffSerialCode: stuffSerialCode,
                unitId: unitId,
                transactionTypeId: transactionTypeId,
                transactionBatchId: transactionBatchId,
                referenceTransaction: referenceTransaction);
      return transactionPlan;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetTransactionPlans<TResult>(
        Expression<Func<BaseTransaction, TResult>> selector,
        TValue<int> id = null,
        TValue<double> amount = null,
        TValue<DateTime> dateTime = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<DateTime> effectDateTime = null,
        TValue<DateTime> fromEffectDateTime = null,
        TValue<DateTime> toEffectDateTime = null,
        TValue<string> description = null,
        TValue<long?> stuffSerialCode = null,
        TValue<int> stuffId = null,
        TValue<int?> billOfMaterialVersion = null,
        TValue<int> unitId = null,
        TValue<int> transactionBatchId = null,
        TValue<int> transactionTypeId = null,
        TValue<TransactionLevel> transactionLevel = null,
        TValue<int> referenceTransactionId = null,
        TValue<int> warehouseId = null,
        TValue<bool> isDelete = null,
        TValue<bool> isEstimated = null,
        TValue<int> baseEntityId = null)
    {

      var baseTransactionQuery = GetBaseTransactions(
                    selector: e => e,
                    id: id,
                    amount: amount,
                    dateTime: dateTime,
                    fromDateTime: fromDateTime,
                    toDateTime: toDateTime,
                    effectDateTime: effectDateTime,
                    fromEffectDateTime: fromEffectDateTime,
                    toEffectDateTime: toEffectDateTime,
                    description: description,
                    stuffSerialCodes: new long?[] { stuffSerialCode },
                    stuffId: stuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    unitId: unitId,
                    transactionBatchId: transactionBatchId,
                    transactionTypeId: transactionTypeId,
                    transactionLevel: transactionLevel,
                    referenceTransactionId: referenceTransactionId,
                    baseEntityId: baseEntityId);
      var transactionPlans = baseTransactionQuery.OfType<BaseTransaction>();
      if (isDelete != null)
        transactionPlans = transactionPlans.Where(i => i.IsDelete == isDelete);
      if (isEstimated != null)
        transactionPlans = transactionPlans.Where(i => i.IsEstimated == isEstimated);
      return transactionPlans.Select(selector);
    }
    #endregion

    #region RemoveTransactionPlan
    internal void RemoveBaseTransactionProcess(int id, byte[] rowVersion)
    {

      EditBaseTransaction(
                    id: id,
                    rowVersion: rowVersion,
                    isDelete: true);
    }
    internal void RemoveBaseTransactionProcess(BaseTransaction baseTransaction, byte[] rowVersion)
    {

      EditBaseTransaction(
                    transactionPlan: baseTransaction,
                    rowVersion: rowVersion,
                    isDelete: true);
    }
    #endregion
  }
}
