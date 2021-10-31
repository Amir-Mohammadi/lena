using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.Supplies.Forwarder
{
  public class GetForwardersInput : SearchInput<ForwarderSortType>
  {
    public GetForwardersInput(PagingInput pagingInput, ForwarderSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
