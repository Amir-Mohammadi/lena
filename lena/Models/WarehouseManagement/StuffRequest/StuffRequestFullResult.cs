using System;
using System.Collections.Generic;
using System.Linq;
using lena.Domains.Enums;
using lena.Models.WarehouseManagement.StuffRequestItem;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequest
{
  public class StuffRequestFullResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public byte[] RowVersion { get; set; }
    public IQueryable<StuffRequestItemResult> StuffRequestItems { get; set; }
    public int FromWarehouseId { get; set; }
    public string FromWarehouseName { get; set; }
    public int? ToWarehouseId { get; set; }
    public string ToWarehouseName { get; set; }
    public StuffRequestType StuffRequestType { get; set; }
    public DateTime DateTime { get; set; }
    public bool IsDelete { get; set; }
    public string Description { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public int? ToEmployeeId { get; set; }
    public string ToEmployeeFullName { get; set; }
    public int? ToDepartmentId { get; set; }
    public string ToDepartmentName { get; set; }
    public string ScrumProjectCode { get; set; }
    public string ScrumProjectName { get; set; }
    public int? ProductionOrderId { get; set; }
    public string ProductionOrderCode { get; set; }
    public IEnumerable<int> ProductionOrderIds { get; set; }
    public IEnumerable<string> ProductionOrderCodes { get; set; }
    public int? ScrumProjectId { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public IEnumerable<string> StuffCodes { get; set; }
    public int StuffRequestItemId { get; set; }
    public string StuffRequestItemCode { get; set; }
    public int? StuffRequestItemStuffId { get; set; }
    public StuffType? StuffType { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public string StuffRequestItemStuffCode { get; set; }
    public string StuffRequestItemStuffName { get; set; }
    public double Qty { get; set; }
    public int AvailableAmount { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double ResponsedQty { get; set; }
    public StuffRequestItemStatusType Status { get; set; }
    public string StuffRequestItemDescription { get; set; }
    public bool StuffRequestItemIsDelete { get; set; }
    public int StuffRequestId { get; set; }
    public Byte[] StuffRequestItemRowVersion { get; set; }
    public string StockPlaceCode { get; set; }
    public string StockPlaceTitle { get; set; }
  }
}
