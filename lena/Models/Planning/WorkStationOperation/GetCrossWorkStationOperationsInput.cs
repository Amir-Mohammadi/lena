using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.Planning.WorkStationOperation
{
  public class GetCrossWorkStationOperationsInput : SearchInput<CrossWorkStationOperationSortType>
  {
    public short? WorkStationId { get; set; }
    public int? OperationId { get; set; }
    public bool? IsExist { get; set; }

    public GetCrossWorkStationOperationsInput(PagingInput pagingInput, CrossWorkStationOperationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
