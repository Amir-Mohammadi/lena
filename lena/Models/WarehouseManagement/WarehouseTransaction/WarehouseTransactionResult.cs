using System;
using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseTransaction
{
  public class WarehouseTransactionResult
  {
    public int Id { get; set; }
    public int TransnsactionBatchId { get; set; }
    public int BaseEntityId { get; set; }
    public short? WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public double Amount { get; set; }
    public int StuffId { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime EffectDateTime { get; set; }
    public long? StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string EmployeeFullName { get; set; }
    public int TransactionTypeId { get; set; }
    public string TransactionTypeName { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public int? BillOfMaterialVersion { get; set; }
  }
}
