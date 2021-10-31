using lena.Domains.Enums;
namespace lena.Models.Unit
{
  public class UnitResult
  {
    public byte Id { get; set; }
    public string Name { get; set; }
    public bool IsMainUnit { get; set; }
    public double ConversionRatio { get; set; }
    public bool IsActive { get; set; }
    public string UnitTypeName { get; set; }
    public byte UnitTypeId { get; set; }
    public byte DecimalDigitCount { get; set; }
    public string Symbol { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
