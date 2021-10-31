using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Unit
{
  public class GetUnitsInput : SearchInput<UnitSortType>
  {
    public GetUnitsInput(PagingInput pagingInput, UnitSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public byte? UnitTypeId { get; set; }
  }
}
