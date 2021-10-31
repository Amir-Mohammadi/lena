using System.Collections.Generic;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.Accounting.BillOfMaterialPrice
{
  public class BillOfMaterialPriceSummary
  {
    public BillOfMaterialPriceSummary()
    {
      CurrencyPriceList = new List<CurrencyPrice>();
    }
    public double CustomsPrice { get; set; }
    public double TransportPrice { get; set; }
    public List<CurrencyPrice> CurrencyPriceList { get; set; }

    public double Total
    {
      get { return CurrencyPriceList.Sum(i => i.ConvertedPrice) + this.CustomsPrice + this.TransportPrice; }

    }

  }
}
