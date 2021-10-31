using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using System;

using lena.Domains.Enums;
namespace lena.Models.ScrumManagement.ScrumBackLog
{
  public class GetScrumBackLogsInput : SearchInput<ScrumBackLogSortType>
  {
    public short? DepartmentId { get; set; }
    public int? ScrumTaskTypeId { get; set; }
    public int? UserId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public ScrumTaskStep? ScrumTaskStep { get; set; }
    public ScrumTaskStep[] ScrumTaskSteps { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public GetScrumBackLogsInput(PagingInput pagingInput, ScrumBackLogSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
