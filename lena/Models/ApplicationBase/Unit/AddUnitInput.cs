using lena.Domains.Enums;
namespace lena.Models.Unit
{
  public class AddUnitInput
  {
    public string Name { get; set; }
    public bool IsMainUnit { get; set; }
    public double ConversionRatio { get; set; }
    public bool IsActive { get; set; }
    public byte UnitTypeId { get; set; }
    public byte DecimalDigitCount { get; set; }
    public string Symbol { get; set; }
  }
}