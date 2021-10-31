using lena.Domains.Enums;
namespace lena.Models.Accounting.BillOfMaterialPrice
{
  public class CurrencyPrice
  {
    public int Id { get; set; }
    public double ConvertedPrice { get; set; }
    public string BaseCurrencyTitle { get; set; }
    public int BaseCurrencyId { get; set; }
    public double BasePrice { get; set; }
    public string BaseCurrencySign { get; set; }
    public int BaseCurrencyDecimalDigitCount { get; set; }
  }
}
