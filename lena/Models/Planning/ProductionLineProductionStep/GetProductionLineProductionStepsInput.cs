using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionLineProductionStep
{
  public class GetProductionLineProductionStepsInput : SearchInput<ProductionLineProductionStepSortType>
  {
    public int? ProductionLineId { get; set; }
    public int? ProductionStepId { get; set; }

    public GetProductionLineProductionStepsInput(PagingInput pagingInput, ProductionLineProductionStepSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
