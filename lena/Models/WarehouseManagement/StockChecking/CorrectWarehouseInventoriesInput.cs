using lena.Models.WarehouseManagement.StuffSerial;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockChecking
{
  public class CorrectWarehouseInventoriesInput
  {
    public int StockCheckingId { get; set; }
    public int TagTypeId { get; set; }
    public short WarehouseId { get; set; }
    public GetStuffSerialInput[] Serials { get; set; }
  }
}
