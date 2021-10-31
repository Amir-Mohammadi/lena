using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionPlanDetail
{
  public class GetProductionPlanDetailsInput : SearchInput<ProductionPlanDetailSortType>
  {
    public DateTime? DeadlineFromDate { get; set; }
    public DateTime? DeadlineToDate { get; set; }
    public int? OrderId { get; set; }
    public int? ProductionPlanId { get; set; }
    public DateTime? FromPlanEstimatedDate { get; set; }
    public DateTime? ToPlanEstimatedDate { get; set; }
    public GetProductionPlanDetailsInput(PagingInput pagingInput, ProductionPlanDetailSortType sortType, SortOrder sortOrder) :
        base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
