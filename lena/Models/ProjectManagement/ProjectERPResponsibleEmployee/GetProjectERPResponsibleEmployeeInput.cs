using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPResponsibleEmployee
{
  public class GetProjectERPResponsibleEmployeeInput : SearchInput<ProjectERPResponsibleEmployeeSortType>
  {
    public string Name { get; set; }
    public bool? IsActive { get; set; }

    public GetProjectERPResponsibleEmployeeInput(PagingInput pagingInput, ProjectERPResponsibleEmployeeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
