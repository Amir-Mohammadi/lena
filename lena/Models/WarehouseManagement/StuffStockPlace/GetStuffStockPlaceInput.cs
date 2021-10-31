using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffStockPlace
{
  public class GetStuffStockPlaceInput
  {
    public int StuffId { get; set; }
    public int StockPlaceId { get; set; }
    public short WarehouseId { get; set; }
  }
}
