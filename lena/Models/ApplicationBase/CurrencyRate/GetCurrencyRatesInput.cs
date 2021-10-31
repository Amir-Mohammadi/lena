using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.CurrencyRate
{
  public class GetCurrencyRatesInput : SearchInput<CurrencyRateSortType>
  {
    public int? FromCurrencyId { get; set; }
    public int? ToCurrencyId { get; set; }
    public GetCurrencyRatesInput(PagingInput pagingInput, CurrencyRateSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
