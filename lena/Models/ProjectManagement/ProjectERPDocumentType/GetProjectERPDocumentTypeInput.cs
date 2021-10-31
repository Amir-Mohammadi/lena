using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPDocumentType
{
  public class GetProjectERPDocumentTypeInput : SearchInput<ProjectERPDocumentTypeSortType>
  {
    public string Name { get; set; }
    public bool? IsActive { get; set; }

    public GetProjectERPDocumentTypeInput(PagingInput pagingInput, ProjectERPDocumentTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
