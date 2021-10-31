using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class GetDutyCostLadingsInput : GetFullLadingsInput
  {
    public GetDutyCostLadingsInput(PagingInput pagingInput, LadingItemSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
