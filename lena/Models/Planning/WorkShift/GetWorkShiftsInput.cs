using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkShift
{
  public class GetWorkShiftsInput : SearchInput<WorkShiftSortType>
  {
    public GetWorkShiftsInput(PagingInput pagingInput, WorkShiftSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
