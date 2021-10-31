using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.EmployeeWorkReport
{
  public class GetEmployeeWorkReportsInput : SearchInput<EmployeeWorkReportSortType>
  {
    public int? Id { get; set; }
    public int? EmployeeId { get; set; }
    public int? ProjectERPTaskId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public short? DepartmentId { get; set; }
    public int? OrganizationPostId { get; set; }
    public GetEmployeeWorkReportsInput(PagingInput pagingInput, EmployeeWorkReportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}