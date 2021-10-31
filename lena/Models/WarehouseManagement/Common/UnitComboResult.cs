using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.Common
{
  public class UnitComboResult
  {
    public byte Id { get; set; }
    public string Name { get; set; }
    public bool IsMainUnit { get; set; }
    public byte UnitTypeId { get; set; }

    public double UnitConversionRatio { get; set; }
    public byte DecimalDigitCount { get; set; }
  }
}
