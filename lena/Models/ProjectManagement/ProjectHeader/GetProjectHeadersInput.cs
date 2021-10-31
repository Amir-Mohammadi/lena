using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectHeader
{
  public class GetProjectHeadersInput : SearchInput<ProjectHeaderSortType>
  {
    public int? OwnerCustomerId { get; set; }
    public TValue<string> Code { get; set; }

    public GetProjectHeadersInput(PagingInput pagingInput, ProjectHeaderSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
