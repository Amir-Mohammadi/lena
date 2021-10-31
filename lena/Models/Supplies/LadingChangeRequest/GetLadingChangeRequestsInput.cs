using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingChangeRequest
{
  public class GetLadingChangeRequestsInput : SearchInput<LadingChangeRequestSortType>
  {
    public GetLadingChangeRequestsInput(PagingInput pagingInput, LadingChangeRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public string LadingCode { get; set; }
    public LadingChangeRequestStatus? Status { get; set; }
  }
}
