using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPEventCategory
{
  public class GetProjectERPEventCategoryInput : SearchInput<ProjectERPEventCategorySortType>
  {
    public string Name { get; set; }
    public bool? IsActive { get; set; }

    public GetProjectERPEventCategoryInput(PagingInput pagingInput, ProjectERPEventCategorySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
