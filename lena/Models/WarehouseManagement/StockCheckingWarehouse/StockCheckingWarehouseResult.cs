using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockCheckingWarehouse
{
  public class StockCheckingWarehouseResult
  {
    public int StockCheckingId { get; set; }

    public short WarehouseId { get; set; }
    public string StockCheckingTitle { get; set; }
    public string WarehouseName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
