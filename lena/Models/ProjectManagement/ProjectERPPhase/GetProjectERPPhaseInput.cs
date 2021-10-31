using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPPhase
{
  public class GetProjectERPPhaseInput : SearchInput<ProjectERPPhaseSortType>
  {
    public string Name { get; set; }
    public bool? IsActive { get; set; }

    public GetProjectERPPhaseInput(PagingInput pagingInput, ProjectERPPhaseSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
