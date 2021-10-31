using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.QtyCorrectionRequest
{
  public class QtyCorrectionRequestInventoryResult
  {
    public int Id { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public long? SerialCode { get; set; }
    public string Serial { get; set; }
    public string Description { get; set; }
    public QtyCorrectionRequestType Type { get; set; }
    public QtyCorrectionRequestStatus Status { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double? TotalAmount { get; set; }
    public double? AvailableAmount { get; set; }
    public double? BlockedAmount { get; set; }
    public double? QualityControlAmount { get; set; }
    public double? WasteAmount { get; set; }
    public int? WarehouseInventoryUnitId { get; set; }
    public string WarehouseInventoryUnitName { get; set; }
    public string ConfirmationEmployeeName { get; set; }
    public DateTime? ConfirmationDateTime { get; set; }
    public DateTime DateTime { get; set; }
    public string RegistrarFullName { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
