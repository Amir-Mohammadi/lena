using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models
{
  public class GetEmployeeContactInput : SearchInput<EmployeeContactSortType>
  {
    public int? EmployeeId { get; set; }
    public GetEmployeeContactInput(PagingInput pagingInput, EmployeeContactSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
