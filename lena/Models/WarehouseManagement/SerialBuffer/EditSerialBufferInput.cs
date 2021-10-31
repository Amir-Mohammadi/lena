using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SerialBuffer
{
  public class EditSerialBufferInput
  {
    public int Id { get; set; }
    public double? DamagedAmount { get; set; }
    public double? ShortageAmount { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
