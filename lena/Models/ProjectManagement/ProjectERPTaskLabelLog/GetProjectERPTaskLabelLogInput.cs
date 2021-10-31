using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPTaskLabelLog
{
  public class GetProjectERPTaskLabelLogInput : SearchInput<ProjectERPTaskLabelLogSortType>
  {
    public int? ProjectERPTaskId { get; set; }
    public short? ProjectERPLabelId { get; set; }

    public GetProjectERPTaskLabelLogInput(PagingInput pagingInput, ProjectERPTaskLabelLogSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
