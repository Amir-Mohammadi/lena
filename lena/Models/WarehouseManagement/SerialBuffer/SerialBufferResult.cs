using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class SerialBufferResult
  {
    public int Id { get; set; }
    public string Serial { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public short? WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public int? ProductionTerminalId { get; set; }
    public string ProductionTerminalName { get; set; }
    public double InitialQty { get; set; }
    public double RemainingAmount { get; set; }
    public double DamagedAmount { get; set; }
    public double ShortageAmount { get; set; }
    public SerialBufferType SerialBufferType { get; set; }
    public byte[] RowVersion { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public long? StuffSerialCode { get; set; }
    public int UserId { get; set; }
    public string EmployeeName { get; set; }
    public DateTime CreationTime { get; set; }
    public double AvailableAmount { get; set; }
  }
}
