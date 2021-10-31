using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.BaseTransaction
{
  public class BaseTransactionMinResult
  {
    public int Id { get; set; }
    public int TransactionBatchId { get; set; }
    public int? BaseEntityId { get; set; }
    public int StuffId { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public DateTime EffectDateTime { get; set; }
    public TransactionLevel TransactionLevel { get; set; }
    public int TransactionTypeId { get; set; }
    public double Value { get; set; }
    public double StuffFaultyPercentage { get; set; }
    public int? ReferenceTransactionId { get; set; }
    public int? ReferenceTransactionTransactionBatchId { get; set; }
    public TransactionTypeFactor TransactionTypeFactor { get; set; }
  }
}
