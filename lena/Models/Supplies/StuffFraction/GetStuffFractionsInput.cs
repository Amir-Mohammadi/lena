using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffFraction
{
  public class GetStuffFractionsInput : SearchInput<StuffFractionSortType>
  {
    public DateTime DateTime { get; set; }
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }
    public InventoryPlanStatus[] Statuses { get; set; }
    public int? StuffId { get; set; }
    public int? ProductStuffId { get; set; }
    public int? ProductStuffVersion { get; set; }
    public bool? IncludeSerialBuffer { get; set; }
    public bool IncludeMainProductionPlan { get; set; }
    public bool IncludeTemporaryProductionPlan { get; set; }
    public bool IncludeStuffFaultyPercentage { get; set; }
    public int? PlannerUserId { get; set; }
    public int[] WarehouseIds { get; set; }

    public GetStuffFractionsInput(PagingInput pagingInput, StuffFractionSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
