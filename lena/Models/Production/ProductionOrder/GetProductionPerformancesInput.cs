using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOrder
{
  public class GetProductionPerformancesInput : SearchInput<ProductionPerformanceSortType>
  {
    public int? Id { get; set; }
    public int? StuffId { get; set; }
    public int? Version { get; set; }
    public string Code { get; set; }
    public string OrderCode { get; set; }
    public string ProductionRequestCode { get; set; }
    public string ProductionPlanCode { get; set; }
    public string ProductionScheduleCode { get; set; }
    public int? ProductionLineId { get; set; }
    public int? ProductionStepId { get; set; }
    public DateTime? DateTime { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public ProductionOrderStatus? Status { get; set; }
    public ProductionOrderStatus[] Statuses { get; set; }
    public ProductionOrderStatus[] NotHasStatuses { get; set; }
    public GetProductionPerformancesInput(PagingInput pagingInput, ProductionPerformanceSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
