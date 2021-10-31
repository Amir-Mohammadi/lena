using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOrder
{
  public class ProductionOrderResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int? ProductionScheduleId { get; set; }
    public string ProductionScheduleCode { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public int StuffId { get; set; }
    public int BillOfMaterialVersion { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public int WorkPlanId { get; set; }
    public string WorkPlanTitle { get; set; }
    public int WorkPlanStepId { get; set; }
    public string WorkPlanStepTitle { get; set; }
    public double Qty { get; set; }
    public double ToleranceQty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double ProducedQty { get; set; }
    public double? InProductionQty { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime StartDateTime { get; set; }
    public long Duration { get; set; }
    public int WorkPlanVersion { get; set; }
    public DateTime ToDateTime { get; set; } // => StartDateTime.AddSeconds(Duration);
    public string OrderCode { get; set; }
    public string ProductionPlanCode { get; set; }
    public string ProductionRequestCode { get; set; }
    public int ProductionStepId { get; set; }
    public int ProductionLineId { get; set; }
    public string ProductionStepName { get; set; }
    public string ProductionLineName { get; set; }
    public ProductionOrderStatus Status { get; set; }
    public int ConsumeWarehouseId { get; set; }
    public string ConsumeWarehouseName { get; set; }
    public int ProductWarehouseId { get; set; }
    public string ProductWarehouseName { get; set; }
    public double UnitConversionRatio { get; set; }
    public double BillOfMaterialUnitConversionRatio { get; set; }
    public int BillOfMaterialUnitId { get; set; }
    public double BillOfMaterialValue { get; set; }
    public string Barcode { get; set; }
    public int SupervisorEmployeeId { get; set; }
    public string SupervisorEmployeeFullName { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}