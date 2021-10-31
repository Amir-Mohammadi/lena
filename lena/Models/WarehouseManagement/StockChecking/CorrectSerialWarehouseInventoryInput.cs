using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockChecking
{
  public class CorrectSerialWarehouseInventoryInput
  {
    public int StockCheckingId { get; set; }
    public int TagTypeId { get; set; }
    public short WarehouseId { get; set; }
    public int? StockCheckingTagId { get; set; }
    public double TagAmount { get; set; }
    public double StockSerialAmount { get; set; }
    public double ContradictionAmount { get; set; }
    public byte UnitId { get; set; }
    public string Serial { get; set; }
    public long? StuffSerialCode { get; set; }
    public int StuffSerialStuffId { get; set; }
  }
}
