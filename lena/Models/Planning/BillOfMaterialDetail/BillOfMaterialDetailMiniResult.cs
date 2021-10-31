using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialDetail
{
  public class BillOfMaterialDetailMiniResult
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public int? Version { get; set; }
    public BillOfMaterialDetailType BillOfMaterialDetailType { get; set; }
    public double Value { get; set; }
    public double UnitConversionRatio { get; set; }
    public StuffType StuffType { get; set; }
  }
}
