using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPLabelLog
{
  public class GetProjectERPLabelLogInput : SearchInput<ProjectERPLabelLogSortType>
  {
    public int? ProjectERPId { get; set; }
    public short? ProjectERPLabelId { get; set; }

    public GetProjectERPLabelLogInput(PagingInput pagingInput, ProjectERPLabelLogSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
