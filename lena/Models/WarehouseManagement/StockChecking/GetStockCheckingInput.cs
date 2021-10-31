using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models
{
  public class GetStockCheckingInput : SearchInput<StockCheckingSortType>
  {
    public GetStockCheckingInput(PagingInput pagingInput, StockCheckingSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public StockCheckingStatus? StockCheckingStatus { get; set; }
  }
}