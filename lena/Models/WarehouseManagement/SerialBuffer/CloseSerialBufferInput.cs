using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SerialBuffer
{
  public class CloseSerialBufferInput
  {
    public string Serial { get; set; }
    public short WarehouseId { get; set; }
  }
}
