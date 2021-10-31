using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.ApplicationBase.CurrencyRate;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Accounting.BillOfMaterialPrice
{
  public class BillOfMaterialPriceInput : SearchInput<BillOfMaterialPriceSortType>
  {
    public int BillOfMaterialStuffId { get; set; }
    public short? BillOfMaterialVersion { get; set; }
    public int CurrencyId { get; set; }
    public CurrencyRateValue[] CurrencyRateValues { get; set; }
    public bool CalculatePurchasePriceByOwnCurrencyRate { get; set; }
    public BillOfMaterialPriceListDisplayType DisplayType { get; set; }

    public BillOfMaterialPriceInput(PagingInput pagingInput, BillOfMaterialPriceSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
