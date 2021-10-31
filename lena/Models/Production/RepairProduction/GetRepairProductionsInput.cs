using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Production.RepairProduction
{
  public class GetRepairProductionsInput : SearchInput<RepairProductionSortType>
  {
    public GetRepairProductionsInput(PagingInput pagingInput, RepairProductionSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? StuffId { get; set; }
    public string Serial { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? FromDate { get; set; }
    public RepairProductionStatus? RepairProductionStatus { get; set; }
    public RepairProductionStatus[] RepairProductionStatuses { get; set; }
  }
}
