using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ScrumManagement.ScrumProject
{
  public class GetScrumProjectsInput : SearchInput<ScrumProjectSortType>
  {
    public GetScrumProjectsInput(PagingInput pagingInput, ScrumProjectSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? DepartmentId { get; set; }
  }
}
