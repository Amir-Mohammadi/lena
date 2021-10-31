using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums;
namespace lena.Models.EmployeeAttendance
{
  public class GetEmployeeWorkDetailInput : SearchInput<EmployeeWorkDetailSortType>
  {
    public GetEmployeeWorkDetailInput(PagingInput pagingInput, EmployeeWorkDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public DateTime? Date { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
  }
}