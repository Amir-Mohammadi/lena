using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionStep
{
  public class GetProductionSetpInput : SearchInput<ProductionStepSortType>
  {
    public GetProductionSetpInput(PagingInput pagingInput, ProductionStepSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
