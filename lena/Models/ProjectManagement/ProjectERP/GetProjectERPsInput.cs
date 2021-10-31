using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERP
{
  public class GetProjectERPsInput : SearchInput<ProjectERPSortType>
  {
    public int? ProjectERPId { get; set; }
    public TValue<string> Code { get; set; }

    public GetProjectERPsInput(PagingInput pagingInput, ProjectERPSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
