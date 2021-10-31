using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Currency
{
  public class AddCurrencyInput
  {
    public string Title { get; set; }
    public CurrencyType Type { get; set; }
    public string Code { get; set; }
    public string Sign { get; set; }
    public bool IsMain { get; set; }
    public byte DecimalDigitCount { get; set; }
  }
}
