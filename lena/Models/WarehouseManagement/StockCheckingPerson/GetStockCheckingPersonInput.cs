using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.StockCheckingPerson
{
  public class GetStockCheckingPersonInput : SearchInput<StockCheckingPersonSortType>
  {
    public int StockCheckingId { get; set; }
    public int UserId { get; set; }
    public GetStockCheckingPersonInput(PagingInput pagingInput, StockCheckingPersonSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
