using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkStation
{
  public class GetWorkStationsInput : SearchInput<WorkStationSortType>
  {
    public int? ProductionLineId { get; set; }

    public GetWorkStationsInput(PagingInput pagingInput, WorkStationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
