using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Exchange
{
  public class GetExchangeInput : SearchInput<ExchangeSortType>
  {

    public GetExchangeInput(PagingInput pagingInput, ExchangeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}