using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockPlace
{
  public class AddStockPlacesInput
  {
    public short WarehouseId { get; set; }
    public WarehouseLayoutItemInput[] WarehouseLayoutItems { get; set; }


  }
}
