using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffSerial
{
  public class GetStuffSerialStockInput
  {
    public short WarehouseId { get; set; }
    public string Serial { get; set; }
    public int? Version { get; set; }
  }
}
