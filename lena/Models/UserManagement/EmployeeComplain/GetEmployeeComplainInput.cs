using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models
{
  public class GetEmployeeComplainInput : SearchInput<EmployeeComplainSortType>
  {

    public int? EmployeeId { get; set; }
    public DateTime? DateTime { get; set; }
    public GetEmployeeComplainInput(PagingInput pagingInput, EmployeeComplainSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

