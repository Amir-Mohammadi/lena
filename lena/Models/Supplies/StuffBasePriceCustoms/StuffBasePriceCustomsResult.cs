using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffBasePriceCustoms
{
  public class StuffBasePriceCustomsResult
  {
    public int Id { get; set; }
    public StuffBasePriceCustomsType Type { get; set; }
    public double? Price { get; set; }
    public double? Percent { get; set; }
    public int? HowToBuyId { get; set; }
    public double Tariff { get; set; }
    public string HowToBuyTitle { get; set; }
    public byte[] RowVersion { get; set; }
  }
}