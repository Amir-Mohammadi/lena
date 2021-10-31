using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SerialBuffer
{
  public class SerialBufferMinResult
  {
    public int Id { get; set; }
    public double RemainingAmount { get; set; }
    public SerialBufferType SerialBufferType { get; set; }
    public short? WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public string Serial { get; set; }
    public byte[] RowVersion { get; set; }
    public int StuffId { get; set; }
    public long? StuffSerialCode { get; set; }
    public short? BillOfMaterialVersion { get; set; }
    public int? ProductionTerminalId { get; set; }
    public double ShortageAmount { get; set; }
    public double DamagedAmount { get; set; }
    public double AvailableAmount { get; set; }
    public BillOfMaterialVersionType? BillOfMaterialVersionType { get; set; }
  }
}
