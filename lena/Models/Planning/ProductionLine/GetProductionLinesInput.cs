using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionLine
{
  public class GetProductionLinesInput : SearchInput<ProductionLineSortType>
  {
    public int? Id { get; set; }
    public GetProductionLinesInput(PagingInput pagingInput, ProductionLineSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
