using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffPrice
{
  public class StuffCalculationPriceResult
  {
    public double Price { get; set; }
    public double Customs { get; set; } = 0;
    public double Transport { get; set; } = 0;
    public double Total => this.Price + this.Customs + this.Transport;
  }
}