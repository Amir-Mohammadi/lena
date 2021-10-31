using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Production.StuffProductionFaultType
{
  public class GetStuffProductionFaultTypesInput : SearchInput<StuffProductionFaultTypeSortType>
  {
    public int? ProductionFaultTypeId { get; set; }
    public int? StuffId { get; set; }
    public int? OperationId { get; set; }


    public GetStuffProductionFaultTypesInput(PagingInput pagingInput, StuffProductionFaultTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
