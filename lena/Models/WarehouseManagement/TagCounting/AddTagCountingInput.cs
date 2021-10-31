using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.TagCounting
{
  public class AddTagCountingInput
  {
    public int StockCheckingTagId { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public bool ReplaceIfExist { get; set; }
  }
}
