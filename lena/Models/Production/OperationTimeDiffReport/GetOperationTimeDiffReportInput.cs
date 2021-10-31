using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Production.OperationTimeDiffReport
{
  public class GetOperationTimeDiffReportInput : SearchInput<OperationTimeDiffReportSortType>
  {

    public int[] EmployeeIds { get; set; }
    public DateTime? FromWorkDate { get; set; }
    public DateTime? ToWorkDate { get; set; }
    public int? ProductionLineId { get; set; }
    public int? OperationId { get; set; }
    public int? StuffId { get; set; }

    public GetOperationTimeDiffReportInput(PagingInput pagingInput, OperationTimeDiffReportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

  }
}
