using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockAdjustment
{
  public class AddStockAdjustmentInput
  {
    public int StockCheckingId { get; set; }
    public short WarehouseId { get; set; }
    public int TagTypeId { get; set; }
    public int StuffId { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public string Serial { get; set; }
    public long StuffSerialCode { get; set; }
    public string Description { get; set; }


  }
}
