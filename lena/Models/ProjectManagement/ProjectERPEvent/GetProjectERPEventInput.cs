using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPEvent
{
  public class GetProjectERPEventInput : SearchInput<ProjectERPEventSortType>
  {
    public int? ProjectERPId { get; set; }

    public GetProjectERPEventInput(PagingInput pagingInput, ProjectERPEventSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
