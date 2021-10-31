using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequest
{
  public class GetStuffRequestsInput : SearchInput<StuffRequestSortType>
  {
    public int? FromWarehouseId { get; set; }
    public int? ToWarehouseId { get; set; }
    public int? StuffRequestTypeId { get; set; }
    public int? StuffRequestStatusTypeId { get; set; }
    public int? ScrumProjectId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public int? StuffId { get; set; }
    public int? ToDepartmentId { get; set; }
    public int? EmployeeId { get; set; }
    public int? ToEmployeeId { get; set; }
    public string ProductionOrderCode { get; set; }

    public string Code { get; set; }

    public GetStuffRequestsInput(PagingInput pagingInput, StuffRequestSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}