using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SerialBuffer
{
  public class AddSerialBufferInput
  {
    public string Serial { get; set; }

    public SerialBufferType SerialBufferType { get; set; }
    public int ProductionTerminalId { get; set; }
    public int? ProductionOrderId { get; set; }
    public short WarehouseId { get; set; }
  }
}
