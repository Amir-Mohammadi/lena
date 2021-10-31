using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models
{
  public class GetDepartmentInput : SearchInput<DepartmentSortType>
  {
    public short? Id { get; set; }
    public string Name { get; set; }
    public short? ParentDepartmentId { get; set; }

    public GetDepartmentInput(PagingInput pagingInput, DepartmentSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}