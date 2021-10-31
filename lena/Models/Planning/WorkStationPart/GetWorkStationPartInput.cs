using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkStationPart
{
  public class GetWorkStationPartInput : SearchInput<WorkStationPartSortType>
  {
    public WorkStationPartType? WorkStationPartType { get; set; }
    public int? WorkStationId { get; set; }

    public GetWorkStationPartInput(PagingInput pagingInput, SortOrder sortOrder, WorkStationPartSortType sortType) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
