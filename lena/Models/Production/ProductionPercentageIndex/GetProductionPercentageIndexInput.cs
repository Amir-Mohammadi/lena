using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Models.Common;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class GetProductionPercentageIndexInput : SearchInput<ProductionPercentageIndexSortType>
  {
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }
    public int[] ProductionLineIds { get; set; }
    public GetProductionPercentageIndexInput(PagingInput pagingInput, ProductionPercentageIndexSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
