using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionFaultType
{
  public class GetProductionFaultTypesInput : SearchInput<ProductionFaultTypeSortType>
  {
    public int? OperationId { get; set; }
    public GetProductionFaultTypesInput(PagingInput pagingInput, ProductionFaultTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

  }
}
