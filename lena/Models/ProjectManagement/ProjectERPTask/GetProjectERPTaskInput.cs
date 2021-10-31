using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPTask
{
  public class GetProjectERPTaskInput : SearchInput<ProjectERPTaskSortType>
  {
    public int? ProjectERPId { get; set; }
    public int? AssigneeEmployeeId { get; set; }

    public GetProjectERPTaskInput(PagingInput pagingInput, ProjectERPTaskSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
