using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseTransaction
{
  public class StuffAvailableInventoryResult
  {
    public int WarehouseId { get; set; }
    public int StuffId { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
  }
}
