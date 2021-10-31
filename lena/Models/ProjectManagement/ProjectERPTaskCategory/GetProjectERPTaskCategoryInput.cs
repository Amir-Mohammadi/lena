using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPTaskCategory
{
  public class GetProjectERPTaskCategoryInput : SearchInput<ProjectERPTaskCategorySortType>
  {
    public string Name { get; set; }
    public bool? IsActive { get; set; }

    public GetProjectERPTaskCategoryInput(PagingInput pagingInput, ProjectERPTaskCategorySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
