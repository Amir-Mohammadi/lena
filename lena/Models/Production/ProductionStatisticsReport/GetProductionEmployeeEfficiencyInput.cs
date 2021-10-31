using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class GetProductionEmployeeEfficiencyInput : SearchInput<ProductionEmployeeEfficiencySortType>
  {
    public GetProductionEmployeeEfficiencyInput(PagingInput pagingInput, ProductionEmployeeEfficiencySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? EmployeeId { get; set; }
    //public int? ProductionLineId { get; set; }
    //public int[] ProductionLineIds { get; set; }
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }
    //public bool GroupByProductionLine { get; set; }
    public bool GroupByIntervals { get; set; }
  }
}
