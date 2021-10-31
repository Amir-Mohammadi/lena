using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingBlocker
{
  public class GetLadingBlockersInput : SearchInput<LadingBlockerSortType>
  {
    public GetLadingBlockersInput(PagingInput pagingInput, LadingBlockerSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
