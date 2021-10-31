using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.GeneralStuffRequest
{
  public class GeneralStuffRequestResult
  {

    public int Id { get; set; }
    public int? ProductionMaterialRequestId { get; set; }
    public string ProductionOrderCode { get; set; }
    public StuffRequestType StuffRequestType { get; set; }
    public GeneralStuffRequestStatus Status { get; set; }
    public StuffType StuffType { get; set; }
    public string StuffCode { get; set; }
    public int StuffId { get; set; }
    public string StuffName { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public double Qty { get; set; }
    public double StuffRequestQty { get; set; }
    public double PurchaseRequestQty { get; set; }
    public double AlternativePurchaseRequestQty { get; set; }
    public double RemainQty { get; set; }
    public string StatusDescription { get; set; }
    public int? ScrumProjectId { get; set; }
    public string ScrumProjectCode { get; set; }
    public string ScrumProjectName { get; set; }
    public double? WarehouseAvailableQty { get; set; }
    public int? WarehouseAvailableUnitId { get; set; }
    public string WarehouseAvailableUnitName { get; set; }
    public short FromWarehouseId { get; set; }
    public string FromWarehouseName { get; set; }
    public short? ToWarehouseId { get; set; }
    public string ToWarehouseName { get; set; }
    public int? ToDepartmentId { get; set; }
    public string ToDepartmentName { get; set; }
    public int? ToEmployeeId { get; set; }
    public string ToEmployeeFullName { get; set; }
    public DateTime Deadline { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
