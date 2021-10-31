using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.PartitionStuffSerial
{
  public class PartitionStuffSerialResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTime DateTime { get; set; }
    public bool IsDelete { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string EmployeeFullName { get; set; }
    public long MainStuffSerialCode { get; set; }
    public int MainStuffSerialStuffId { get; set; }
    public string MainSerial { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int BoxCount { get; set; }
    public double QtyPerBox { get; set; }
    public byte[] RowVersion { get; set; }
    public string Serial { get; set; }
  }
}
