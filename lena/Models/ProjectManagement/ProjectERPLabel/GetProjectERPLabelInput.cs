using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPLabel
{
  public class GetProjectERPLabelInput : SearchInput<ProjectERPLabelSortType>
  {
    public int? Id { get; set; }
    public int? ProjectERPId { get; set; }

    public GetProjectERPLabelInput(PagingInput pagingInput, ProjectERPLabelSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
