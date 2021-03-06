using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseIssue
{
  public class GetSerialIssueDetailInput : SearchInput<SerialIssueDetailSortType>
  {
    public GetSerialIssueDetailInput(PagingInput pagingInput, SerialIssueDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public string Serial { get; set; }
  }


}
