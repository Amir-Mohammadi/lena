using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.Cardex
{
  public class CardexGroupedResult
  {
    public int? WarehouseFiscalPeriodId { get; set; }
    public string WarehouseFiscalPeriodName { get; set; }
    public int TransnsactionBatchId { get; set; }
    public short? WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime EffectDateTime { get; set; }
    public long? StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public string UnitName { get; set; }
    public string UserName { get; set; }
    public string EmployeeFullName { get; set; }
    public double? AvailableAmount { get; set; } = null;
    public double? BlockedAmount { get; set; } = null;
    public double? QualityControlAmount { get; set; } = null;
    public double? PlanAmount { get; set; } = null;
    public double? WasteAmount { get; set; } = null;
    public int? BillOfMaterialVersion { get; set; }
    public string BaseEntityDescription { get; set; }
    public string BaseEntityCode { get; set; }
    public TransactionLevel TransactionLevel { get; set; }
  }
}
