using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkStationOperation
{
  public class GetWorkStationOperationsInput : SearchInput<WorkStationOperationSortType>
  {

    public GetWorkStationOperationsInput(PagingInput pagingInput, WorkStationOperationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public short? WorkStationId { get; set; }
    public int? OperationId { get; set; }
  }
}
