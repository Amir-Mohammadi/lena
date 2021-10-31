using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums;
namespace lena.Models.UnitType
{
  public class GetUnitTypesInput : SearchInput<UnitTypeSortType>
  {
    public GetUnitTypesInput(PagingInput pagingInput, UnitTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}