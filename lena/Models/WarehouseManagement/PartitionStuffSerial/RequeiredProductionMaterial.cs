using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.PartitionStuffSerial
{
  public class RequiredProductionMaterial
  {
    public int StuffId { get; set; }
    public int? Version { get; set; }
    public byte UnitId { get; set; }
    public double Qty { get; set; }
  }
}
