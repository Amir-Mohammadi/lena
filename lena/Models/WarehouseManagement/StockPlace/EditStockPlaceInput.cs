using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockPlace
{
  public class EditStockPlaceInput
  {

    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Code { get; set; }
    public short WarehouseId { get; set; }
    public string Title { get; set; }
  }
}
