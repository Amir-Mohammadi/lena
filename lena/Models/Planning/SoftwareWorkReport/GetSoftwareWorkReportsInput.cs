using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Planning.SoftwareWorkReport
{
  public class GetSoftwareWorkReportsInput : SearchInput<SoftwareWorkReportSortType>
  {
    public int? EmployeeId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }

    public GetSoftwareWorkReportsInput(PagingInput pagingInput, SoftwareWorkReportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
