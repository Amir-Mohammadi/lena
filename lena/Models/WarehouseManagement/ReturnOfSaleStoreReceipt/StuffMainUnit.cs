using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceipt
{
  public class StuffMainUnit
  {
    public int StuffId { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double UnitConversionRatio { get; set; }
  }
}
