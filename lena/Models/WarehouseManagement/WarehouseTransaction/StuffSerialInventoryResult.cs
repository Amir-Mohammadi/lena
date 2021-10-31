using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseTransaction
{
  public class StuffSerialInventoryResult
  {
    public short WarehouseId { get; set; }
    public int StuffId { get; set; }
    public long? StuffSerialCode { get; set; }
    public double AvailableAmount { get; set; }
    public double BlockedAmount { get; set; }
    public double QualityControlAmount { get; set; }
    public double PlanAmount { get; set; }

    public double TotalAmount { get; set; }
    public double WasteAmount { get; set; }
    public byte UnitId { get; set; }

  }
}
