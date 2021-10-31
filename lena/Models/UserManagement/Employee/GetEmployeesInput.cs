using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.Employee
{
  public class GetEmployeesInput : SearchInput<EmployeeSortType>
  {
    public int Id { get; set; }
    public short? DepartmentId { get; set; }
    public bool? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string Code { get; set; }
    public int? OrganizationPostId { get; set; }
    public int? OrganizationJobId { get; set; }
    public byte[] DocumentId { get; set; }

    public GetEmployeesInput(PagingInput pagingInput, EmployeeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
