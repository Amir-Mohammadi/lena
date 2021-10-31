using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockPlace
{
  public class AddStockPlaceInput
  {
    public short WarehouseId { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }

  }
}
