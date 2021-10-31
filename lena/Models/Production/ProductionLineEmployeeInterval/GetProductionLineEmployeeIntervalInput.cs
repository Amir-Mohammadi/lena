using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionLineEmployeeInterval
{
  public class GetProductionLineEmployeeIntervalInput : SearchInput<ProductionLineEmployeeIntervalSortType>
  {
    public int? ProductionLineId { get; set; }
    public int? EmployeeId { get; set; }
    public int? StuffId { get; set; }
    public DateTime? ExitDateTime { get; set; }
    public DateTime? EntranceDateTime { get; set; }
    public DateTime FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public bool IsCompleted { get; set; }

    public int? OperationId { get; set; }


    public GetProductionLineEmployeeIntervalInput(PagingInput pagingInput, ProductionLineEmployeeIntervalSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
