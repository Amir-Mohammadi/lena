using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionSchedule
{
  public class GetProductionSchedulesInput : SearchInput<ProductionScheduleSortType>
  {
    public GetProductionSchedulesInput(PagingInput pagingInput, ProductionScheduleSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }


    public int? StuffId { get; set; }
    public bool? IsPublished { get; set; }
    public int? ProductionLineId { get; set; }
    public DateTime? ToDateTime { get; set; }
    public int? ProductionStepId { get; set; }
    public int? ProductionPlanId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public int? ProductionPlanDetailId { get; set; }
    public ProductionScheduleStatus[] Statuses { get; set; }
    public ProductionScheduleStatus[] NotHasStatuses { get; set; }
  }
}
