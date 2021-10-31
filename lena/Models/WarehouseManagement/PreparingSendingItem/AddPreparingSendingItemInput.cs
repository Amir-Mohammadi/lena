using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.PreparingSendingItem
{
  public class AddPreparingSendingItemInput
  {
    public short WarehouseId { get; set; }
    public int StuffId { get; set; }
    public int? Version { get; set; }
    public string Serial { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public string Description { get; set; }
  }
}
