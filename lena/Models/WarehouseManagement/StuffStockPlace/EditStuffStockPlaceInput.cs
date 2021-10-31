using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffStockPlace
{
  public class EditStuffStockPlaceInput
  {
    public int StuffId { get; set; }
    public int StockPlaceId { get; set; }
    public byte[] RowVersion { get; set; }
    public int NewStuffId { get; set; }
    public int NewStockPlaceId { get; set; }
  }
}
