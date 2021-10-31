using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffStockPlace
{
  public class StuffStockPlaceResult
  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffTitle { get; set; }
    public int StockPlaceId { get; set; }
    public string StockPlaceCode { get; set; }
    public string StockPlaceTitle { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
