using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffSerial
{
  public class GetStuffSerialInput
  {
    public string Serial { get; set; }
    public int? StuffId { get; set; }
    public long? Code { get; set; }
    public int? ProductionOrderId { get; set; }
  }
}
