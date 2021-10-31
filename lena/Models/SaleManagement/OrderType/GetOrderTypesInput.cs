using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models
{
  public class GetOrderTypesInput : SearchInput<OrderTypeSortType>
  {
    public GetOrderTypesInput(PagingInput pagingInput, OrderTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

  }
}
