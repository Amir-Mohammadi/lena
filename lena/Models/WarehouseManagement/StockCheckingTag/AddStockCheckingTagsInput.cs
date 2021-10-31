using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockCheckingTag
{
  public class AddStockCheckingTagsInput
  {
    public int StockCheckingId { get; set; }
    public short WarehouseId { get; set; }
    public int TagTypeId { get; set; }
    public int? Number { get; set; }
    public int? StuffId { get; set; }
    public StuffType? StuffType { get; set; }
    public int? StuffCategoryId { get; set; }
    public bool? HasTag { get; set; }
    public int[] StuffIds { get; set; }
    public bool IsExist { get; set; }
  }
}
