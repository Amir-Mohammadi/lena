using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffSerial
{
  public class GetStuffSerialForSerialCountingInput
  {
    public string Serial { get; set; }
    public int StockCheckingId { get; set; }
    public short WarehouseId { get; set; }
    public int TagTypeId { get; set; }
  }
}