using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Currency
{
  public class CurrencyComboResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public CurrencyType Type { get; set; }
    public byte DecimalDigitCount { get; set; }
  }
}
