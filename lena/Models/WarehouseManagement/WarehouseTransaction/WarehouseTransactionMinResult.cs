using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseTransaction
{
  public class WarehouseTransactionMinResult
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public short? BillOfMaterialVersion { get; set; }
    public short? WarehouseId { get; set; }
    public double Amount { get; set; }
    public long? StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public DateTime EffectDateTime { get; set; }
    public TransactionLevel TransactionLevel { get; set; }
    public int TransactionTypeId { get; set; }
    public TransactionTypeFactor TransactionTypeFactor { get; set; }
    public double Value { get; set; }
    public int TransactionBatchId { get; set; }
  }
}



