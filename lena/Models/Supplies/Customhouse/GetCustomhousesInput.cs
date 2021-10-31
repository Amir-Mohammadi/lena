using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Customhouse
{
  public class GetCustomhousesInput : SearchInput<CustomhouseSortType>
  {
    public GetCustomhousesInput(PagingInput pagingInput, CustomhouseSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
