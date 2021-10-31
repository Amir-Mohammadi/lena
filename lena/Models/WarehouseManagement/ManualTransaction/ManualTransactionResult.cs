using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ManualTransaction
{
  public class ManualTransactionResult
  {
    public int Id { get; set; }
    public int? WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int TransnsactionBatchId { get; set; }
    public double Amount { get; set; }
    public DateTime? DateTime { get; set; }
    public DateTime? EffectDateTime { get; set; }
    public string StuffCode { get; set; }
    public int? Qty { get; set; }
    public string StuffName { get; set; }
    public int? StuffId { get; set; }
    public long? StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public string UnitName { get; set; }
    public string UserName { get; set; }
    public string EmployeeFullName { get; set; }
    public int TransactionTypeId { get; set; }
    public string TransactionTypeName { get; set; }
    public TransactionLevel TransactionLevel { get; set; }
    public TransactionTypeFactor TransactionTypeFactor { get; set; }
    public int? ReferenceTransactionId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public int? BillOfMaterialVersion { get; set; }
  }
}
