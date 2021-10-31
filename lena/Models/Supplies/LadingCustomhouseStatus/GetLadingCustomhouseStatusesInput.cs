using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingCustomhouseStatus
{
  public class GetLadingCustomhouseStatusesInput : SearchInput<LadingCustomhouseStatusSortType>
  {
    public GetLadingCustomhouseStatusesInput(PagingInput pagingInput, LadingCustomhouseStatusSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public string Code { get; set; }
    public string Name { get; set; }
  }
}
