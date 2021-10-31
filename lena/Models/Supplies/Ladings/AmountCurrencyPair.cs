using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class AmountCurrencyPair
  {
    public double Amount { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
  }
}
