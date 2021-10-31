using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffFractionDetail
{
  public class StuffFractionDetailResult
  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public string StuffTitle { get; set; }
    public double StuffFaultyPercentage { get; set; }
    public string UnitName { get; set; }
    public double Value { get; set; }
    public DateTime EffectDateTime { get; set; }
    public double AvailableAmount { get; set; }
    public double QualityControlAmount { get; set; }
    public double BlockedAmount { get; set; }
    public double PlanAmount { get; set; }
    public double TotalAmount { get; set; }
    public int StuffStockSafety { get; set; }
    public double RemainedAmount { get; set; }
    public string OrderItemCode { get; set; }
    public string ProductionRequestCode { get; set; }
    public string ProductionPlanCode { get; set; }
    public string ProductionScheduleCode { get; set; }
    public byte UnitId { get; set; }
    public int? ProductionScheduleId { get; set; }
    public int? ProductionPlanId { get; set; }
    public int? ProductionRequestId { get; set; }
    public int? OrderItemId { get; set; }
    public int? ProductionPlanDetailId { get; set; }
    public int? BillOfMaterialDetailId { get; set; }
  }
}
