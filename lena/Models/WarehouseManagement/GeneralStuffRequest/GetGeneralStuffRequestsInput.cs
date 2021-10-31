using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;



using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.GeneralStuffRequest
{
  public class GetGeneralStuffRequestsInput : SearchInput<GeneralStuffRequestSortType>
  {
    public GetGeneralStuffRequestsInput(PagingInput pagingInput, GeneralStuffRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? Id { get; set; }
    public int[] Ids { get; set; }
    public int? ProductionMaterialRequestId { get; set; }
    public string ProductionOrderCode { get; set; }
    public StuffRequestType? StuffRequestType { get; set; }
    public int? StuffId { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public double? Qty { get; set; }
    public int? ScrumEntityId { get; set; }
    public int? FromWarehouseId { get; set; }
    public int? ToWarehouseId { get; set; }
    public int? ToDepartmentId { get; set; }
    public int? ToEmployeeId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public DateTime? Deadline { get; set; }
    public int? UserId { get; set; }
    public string ScrumProjectCode { get; set; }
    public bool? LoadStuffsAvailableQty { get; set; }
    public GeneralStuffRequestStatus? Status { get; set; }
    public int? EmployeeId { get; set; }
    public int? RequestConfirmerUserGroupId { get; set; }

  }
}
