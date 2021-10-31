//using Parlar.DAL.UnitOfWorks;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.WarehouseManagement.UncommitedTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace lena.Services.Core.Provider.Transaction
{
  public class UncommitedTransactionAgent : Provider
  {
    public List<UncommitedTransaction> UncommitedTransactions { get; }
    public UncommitedTransactionAgent()
    {
      UncommitedTransactions = new List<UncommitedTransaction>();
    }
    public bool Any => UncommitedTransactions.Any();
    public void Add(BaseTransaction transaction)
    {
      if (!UncommitedTransactions.Any(item => item.Id == transaction.Id))
      {
        var tr = new UncommitedTransaction()
        {
          Id = transaction.Id,
          TransactionBatchId = transaction.TransactionBatchId,
          Amount = transaction.Amount,
          Description = transaction.TransactionType.Name,
          StuffId = transaction.StuffId,
          StuffSerialCode = transaction.StuffSerialCode,
          TransactionTypeId = transaction.TransactionTypeId,
          UnitId = transaction.UnitId
        };
        tr.WarehouseId = transaction.WarehouseId ?? null;
        UncommitedTransactions.Add(tr);
      }
    }
    public List<UncommitedTransaction> GetReport()
    {
      foreach (var item in UncommitedTransactions)
      {
        try
        {
          if (item.WarehouseId.HasValue)
          {
            var warehouse = App.Internals.WarehouseManagement.GetWarehouse(item.WarehouseId.Value);
            item.WarehouseName = warehouse.Name;
          }
          if (item.StuffSerialCode.HasValue)
          {
            var serial = App.Internals.WarehouseManagement.GetStuffSerial(stuffId: item.StuffId, code: item.StuffSerialCode.Value);
            item.Serial = serial.Serial;
          }
          var transactionType = App.Internals.WarehouseManagement.GetTransactionType(item.TransactionTypeId);
          item.Description = transactionType.Name;
          item.TransactionLevel = transactionType.TransactionLevel;
          item.Factor = (int)transactionType.Factor;
          item.Amount = item.Amount * (int)transactionType.Factor;
        }
        catch (Exception)
        {
          item.Description = "Could not retrive transaction data.";
        }
      }
      return UncommitedTransactions.ToList();
    }
    public void CheckTransactionBatch()
    {
      if (Any)
      {
        if (UncommitedTransactions.Any(i => i.TransactionLevel != TransactionLevel.Plan))
          App.Internals.WarehouseManagement.CheckTransactionBatch();
      }
    }
  }
}