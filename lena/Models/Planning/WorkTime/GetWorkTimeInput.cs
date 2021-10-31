using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkTime
{
  public class GetWorkTimeInput : SearchInput<WorkTimeSortType>
  {
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int? WorkShiftId { get; set; }

    public GetWorkTimeInput(PagingInput pagingInput, WorkTimeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
      WorkShiftId = null;
    }
  }
}
