using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockPlace
{
  public class StockPlaceResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
