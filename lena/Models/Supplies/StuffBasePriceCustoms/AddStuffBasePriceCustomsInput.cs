using lena.Domains.Enums;
using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffBasePriceCustoms
{
  public class AddStuffBasePriceCustomsInput
  {
    public double Price { get; set; }
    public byte CurrencyId { get; set; }
    public double Percent { get; set; }
    public short? HowToBuyId { get; set; }
    public double HowToBuyRatio { get; set; }
    public double? Tariff { get; set; }
    public double? Weight { get; set; }
    public StuffBasePriceCustomsType Type { get; set; }
  }
}