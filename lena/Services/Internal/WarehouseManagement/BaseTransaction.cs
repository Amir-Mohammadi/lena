using System;
using System.CodeDom;
using System.Linq;
using System.Linq.Expressions;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.WarehouseManagement.BaseTransaction;
using lena.Services.Core;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    internal BaseTransaction AddBaseTransaction(
            BaseTransaction baseTransaction,
            double amount,
            DateTime effectDateTime,
            string description,
            int stuffId,
            short? billOfMaterialVersion,
            long? stuffSerialCode,
            byte unitId,
            short transactionTypeId,
            int transactionBatchId,
            BaseTransaction referenceTransaction)
    {
      baseTransaction.TransactionTypeId = transactionTypeId;
      baseTransaction.StuffId = stuffId;
      baseTransaction.BillOfMaterialVersion = billOfMaterialVersion;
      baseTransaction.StuffSerialCode = stuffSerialCode;
      baseTransaction.UnitId = unitId;
      baseTransaction.Amount = amount;
      baseTransaction.EffectDateTime = effectDateTime;
      baseTransaction.Description = description;
      baseTransaction.TransactionBatchId = transactionBatchId;
      baseTransaction.ReferenceTransaction = referenceTransaction;
      repository.Add(baseTransaction);
      return baseTransaction;
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
        TValue<BaseTransaction> referenceTransaction = null)
    {
      var baseTransaction = GetBaseTransaction(id: id);
      return EditBaseTransaction(
                    baseTransaction: baseTransaction,
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
    }
    internal BaseTransaction EditBaseTransaction(
        BaseTransaction baseTransaction,
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
        TValue<BaseTransaction> referenceTransaction = null)
    {
      if (transactionTypeId != null)
        baseTransaction.TransactionTypeId = transactionTypeId;
      if (stuffId != null)
        baseTransaction.StuffId = stuffId;
      if (billOfMaterialVersion != null)
        baseTransaction.BillOfMaterialVersion = billOfMaterialVersion;
      if (stuffSerialCode != null)
        baseTransaction.StuffSerialCode = stuffSerialCode;
      if (unitId != null)
        baseTransaction.UnitId = unitId;
      if (amount != null)
        baseTransaction.Amount = amount;
      if (effectDateTime != null)
        baseTransaction.EffectDateTime = effectDateTime;
      if (description != null)
        baseTransaction.Description = description;
      if (transactionBatchId != null)
        baseTransaction.TransactionBatchId = transactionBatchId;
      if (referenceTransaction != null)
        baseTransaction.ReferenceTransaction = referenceTransaction;
      repository.Update(baseTransaction, rowVersion);
      return baseTransaction;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetBaseTransactions<TResult>(
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
        TValue<long?[]> stuffSerialCodes = null,
        TValue<int> stuffId = null,
        TValue<int?> billOfMaterialVersion = null,
        TValue<string> serial = null,
        TValue<int> unitId = null,
        TValue<int> transactionBatchId = null,
        TValue<int> transactionTypeId = null,
        TValue<TransactionLevel> transactionLevel = null,
        TValue<TransactionLevel[]> transactionLevels = null,
        TValue<int[]> transactionTypeIds = null,
        TValue<int[]> excludeTransactionTypeIds = null,
        TValue<int> stuffCategoryId = null,
        TValue<int> referenceTransactionId = null,
        TValue<int> baseEntityId = null,
        TValue<int[]> stuffIds = null)
    {
      var baseTransactions = repository.GetQuery<BaseTransaction>();
      if (id != null)
        baseTransactions = baseTransactions.Where(i => i.Id == id);
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial); ; var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial: serial);
        if (stuffId == null)
          stuffId = stuffSerial.StuffId;
        else if (stuffId != stuffSerial.StuffId)
        {
          var stuff = App.Internals.SaleManagement.GetStuff(id: stuffId);
          throw new SerialStuffNotMatchException(serial: serial,
                          serialStuffCode: stuffSerial.Stuff.Code,
                          stuffCode: stuff.Code);
        }
        baseTransactions = baseTransactions.Where(i => i.StuffSerialCode == stuffSerial.Code && i.StuffId == stuffSerial.StuffId);
      }
      if (amount != null)
        baseTransactions = baseTransactions.Where(i => i.Amount == amount);
      if (dateTime != null)
        baseTransactions = baseTransactions.Where(i => i.TransactionBatch.DateTime == dateTime);
      if (fromDateTime != null)
        baseTransactions = baseTransactions.Where(i => i.TransactionBatch.DateTime > fromDateTime);
      if (toDateTime != null)
        baseTransactions = baseTransactions.Where(i => i.TransactionBatch.DateTime < toDateTime);
      if (effectDateTime != null)
        baseTransactions = baseTransactions.Where(i => i.EffectDateTime == effectDateTime);
      if (fromEffectDateTime != null)
        baseTransactions = baseTransactions.Where(i => i.EffectDateTime >= fromEffectDateTime);
      if (toEffectDateTime != null)
        baseTransactions = baseTransactions.Where(i => i.EffectDateTime <= toEffectDateTime);
      if (description != null)
        baseTransactions = baseTransactions.Where(i => i.Description == description);
      if (stuffSerialCodes != null)
        baseTransactions = baseTransactions.Where(i => stuffSerialCodes.Value.Contains(i.StuffSerialCode));
      if (stuffId != null)
        baseTransactions = baseTransactions.Where(i => i.StuffId == stuffId);
      if (billOfMaterialVersion != null)
        baseTransactions = baseTransactions.Where(i => i.BillOfMaterialVersion == billOfMaterialVersion);
      if (unitId != null)
        baseTransactions = baseTransactions.Where(i => i.UnitId == unitId);
      if (transactionBatchId != null)
        baseTransactions = baseTransactions.Where(i => i.TransactionBatchId == transactionBatchId);
      if (transactionTypeId != null)
        baseTransactions = baseTransactions.Where(i => i.TransactionTypeId == transactionTypeId);
      if (transactionLevel != null)
        baseTransactions = baseTransactions.Where(i => i.TransactionType.TransactionLevel == transactionLevel);
      if (referenceTransactionId != null)
        baseTransactions = baseTransactions.Where(i => i.ReferenceTransactionId == referenceTransactionId);
      if (transactionTypeIds != null)
        baseTransactions = baseTransactions.Where(i => transactionTypeIds.Value.Contains(i.TransactionTypeId));
      //var bt1 = baseTransactions.ToList();
      if (excludeTransactionTypeIds != null)
        baseTransactions = baseTransactions.Where(i => !excludeTransactionTypeIds.Value.Contains(i.TransactionTypeId));
      //var bt2 = baseTransactions.ToList();
      if (stuffCategoryId != null)
        baseTransactions = baseTransactions.Where(i => i.Stuff.StuffCategoryId == stuffCategoryId);
      if (transactionLevels != null)
        baseTransactions = baseTransactions.Where(i => transactionLevels.Value.Contains(i.TransactionType.TransactionLevel));
      if (baseEntityId != null)
        baseTransactions = baseTransactions.Where(i => i.TransactionBatch.BaseEntity.Id == baseEntityId);
      if (stuffIds != null)
        baseTransactions = baseTransactions.Where(i => stuffIds.Value.Contains(i.StuffId));
      return baseTransactions.Select(selector);
    }
    #endregion
    #region Get
    internal BaseTransaction GetBaseTransaction(int id) => GetBaseTransaction(selector: e => e, id: id);
    internal TResult GetBaseTransaction<TResult>(
            Expression<Func<BaseTransaction, TResult>> selector,
            int id)
    {
      var baseTransaction = GetBaseTransactions(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (baseTransaction == null)
        throw new BaseTransactionNotFoundException(id);
      return baseTransaction;
    }
    #endregion
    #region ToResult
    internal Expression<Func<BaseTransaction, BaseTransactionResult>> ToBaseTransactionResult =
        baseTransaction => new BaseTransactionResult
        {
          Id = baseTransaction.Id,
          Amount = baseTransaction.Amount,
          StuffId = baseTransaction.StuffId,
          StuffName = baseTransaction.Stuff.Name,
          StuffCode = baseTransaction.Stuff.Code,
          DateTime = baseTransaction.TransactionBatch.DateTime,
          EffectDateTime = baseTransaction.EffectDateTime,
          StuffSerialCode = baseTransaction.StuffSerialCode,
          Serial = baseTransaction.StuffSerial.Serial,
          UnitId = baseTransaction.UnitId,
          UnitName = baseTransaction.Unit.Name,
          UserId = baseTransaction.TransactionBatch.UserId,
          UserName = baseTransaction.TransactionBatch.User.UserName,
          EmployeeFullName = baseTransaction.TransactionBatch.User.Employee.FirstName + " " +
                baseTransaction.TransactionBatch.User.Employee.LastName,
          TransactionTypeId = baseTransaction.TransactionTypeId,
          TransactionTypeName = baseTransaction.TransactionType.Name,
          Description = baseTransaction.Description,
          RowVersion = baseTransaction.RowVersion
        };
    #endregion
    #region ToMinResult
    public Expression<Func<BaseTransaction, BaseTransactionMinResult>> ToBaseTransactionMinResult =
        baseTransaction => new BaseTransactionMinResult
        {
          Id = baseTransaction.Id,
          TransactionBatchId = baseTransaction.TransactionBatchId,
          BaseEntityId = baseTransaction.TransactionBatch.BaseEntity.Id,
          ReferenceTransactionId = baseTransaction.ReferenceTransactionId,
          ReferenceTransactionTransactionBatchId = baseTransaction.ReferenceTransaction.TransactionBatchId,
          StuffId = baseTransaction.StuffId,
          BillOfMaterialVersion = baseTransaction.BillOfMaterialVersion,
          EffectDateTime = baseTransaction.EffectDateTime,
          TransactionLevel = baseTransaction.TransactionType.TransactionLevel,
          TransactionTypeId = baseTransaction.TransactionTypeId,
          TransactionTypeFactor = baseTransaction.TransactionType.Factor,
          Value = baseTransaction.Amount * baseTransaction.Unit.ConversionRatio * (int)baseTransaction.TransactionType.Factor,
          StuffFaultyPercentage = baseTransaction.Stuff.FaultyPercentage
        };
    #endregion
    #region Rmove
    internal BaseTransaction RemoveBaseTransaction(
        int id,
        byte[] rowVersion,
        int? transactionBatchId)
    {
      var baseTransaction = GetBaseTransaction(id: id);
      return RemoveBaseTransaction(
                    baseTransaction: baseTransaction,
                    rowVersion: rowVersion,
                    transactionBatchId: transactionBatchId);
    }
    internal BaseTransaction RemoveBaseTransaction(
        BaseTransaction baseTransaction,
        byte[] rowVersion,
        int? transactionBatchId)
    {
      if (baseTransaction.TransactionType.TransactionLevel == TransactionLevel.Plan)
      {
        // remove plan transaction
        RemoveBaseTransactionProcess(
                baseTransaction: baseTransaction,
                rowVersion: rowVersion);
      }
      else if (transactionBatchId != null)
      {
        //remove warehouse transaction
        RollbackWarehouseTransaction(
                warehouseTransaction: baseTransaction,
                rowVersion: rowVersion,
                transactionBatchId: transactionBatchId.Value);
      }
      return baseTransaction;
    }
    #endregion
  }
}