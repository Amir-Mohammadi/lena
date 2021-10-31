using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.EquivalentStuffUsage
{
  public class GetEquivalentStuffUsagesInput : SearchInput<EquivalentStuffUsageSortType>
  {
    public int? StuffId { get; set; }
    public int? ProductionPlanId { get; set; }
    public string ProductionOrderCode { get; set; }
    public int? ProductionOrderId { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public EquivalentStuffType? EquivalentStuffType { get; set; }
    public EquivalentStuffUsageStatus? Status { get; set; }
    public EquivalentStuffUsageStatus[] Statuses { get; set; }

    public GetEquivalentStuffUsagesInput(PagingInput pagingInput, EquivalentStuffUsageSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
