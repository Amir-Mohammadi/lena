using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Currency
{
  public class GetCurrencysInput : SearchInput<CurrencySortType>
  {
    public GetCurrencysInput(PagingInput pagingInput, CurrencySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
