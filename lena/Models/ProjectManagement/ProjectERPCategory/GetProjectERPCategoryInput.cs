using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPCategory
{
  public class GetProjectERPCategoryInput : SearchInput<ProjectERPCategorySortType>
  {
    public string Name { get; set; }
    public bool? IsActive { get; set; }

    public GetProjectERPCategoryInput(PagingInput pagingInput, ProjectERPCategorySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
