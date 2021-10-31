using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.TagCounting
{
  public class AddSerialTagCountingInput
  {
    public int StockCheckingId { get; set; }
    public short WarehouseId { get; set; }
    public int TagTypeId { get; set; }
    public string Serial { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public bool ReplaceIfExist { get; set; }
  }
}
