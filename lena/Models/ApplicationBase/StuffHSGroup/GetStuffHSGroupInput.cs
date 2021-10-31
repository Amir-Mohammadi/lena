using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.StuffHSGroup
{
  public class GetStuffHSGroupInput : SearchInput<StuffHSGroupSortType>
  {
    public GetStuffHSGroupInput(PagingInput pagingInput, StuffHSGroupSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {

    }
  }
}
