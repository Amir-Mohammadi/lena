using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SerialBuffer
{
  public class DecreaseSerialBufferInput
  {
    public string Serial { get; set; }
    public short WarehouseId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
  }
}
