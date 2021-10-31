using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffFractionResult : IEntity
  {
    protected internal StuffFractionResult()
    {
    }
    public int StuffId { get; set; }
    public string StuffName { get; set; }
    public string StuffTitle { get; set; }
    public string StuffNoun { get; set; }
    public string StuffCode { get; set; }
    public double StuffFaultyPercentage { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public byte UnitTypeId { get; set; }
    public string UnitTypeName { get; set; }
    public int StuffStockSafety { get; set; }
    public double AvailableAmount { get; set; }
    public double BlockedAmount { get; set; }
    public double PlanAmount { get; set; }
    public double QualityControlAmount { get; set; }
    public double WasteAmount { get; set; }
    public double TotalAmount { get; set; }
    public double RemainedAmount { get; set; }
    public double BeforePeriodPlanPlusAmount { get; set; }
    public double BeforePeriodPlanMinusAmount { get; set; }
    public double BeforePeriodPlanAmount { get; set; }
    public double PeriodPlanPlusAmount { get; set; }
    public double PeriodPlanMinusAmount { get; set; }
    public double PeriodPlanAmount { get; set; }
    public double AfterPeriodPlanPlusAmount { get; set; }
    public double AfterPeriodPlanMinusAmount { get; set; }
    public double AfterPeriodPlanAmount { get; set; }
    public InventoryPlanStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public double BeforePeriodPlanConsumeAmount { get; set; }
    public double BeforePeriodPlanProduceAmount { get; set; }
    public double BeforePeriodPlanSaleOrderAmount { get; set; }
    public double BeforePeriodPlanPurchaseRequestAmount { get; set; }
    public double BeforePeriodPlanPurchaseOrderAmount { get; set; }
    public double BeforePeriodPlanCargoAmount { get; set; }
    public double PeriodPlanConsumeAmount { get; set; }
    public double PeriodPlanProduceAmount { get; set; }
    public double PeriodPlanSaleOrderAmount { get; set; }
    public double PeriodPlanPurchaseRequestAmount { get; set; }
    public double PeriodPlanPurchaseOrderAmount { get; set; }
    public double PeriodPlanCargoAmount { get; set; }
    public double AfterPeriodPlanConsumeAmount { get; set; }
    public double AfterPeriodPlanProduceAmount { get; set; }
    public double AfterPeriodPlanSaleOrderAmount { get; set; }
    public double AfterPeriodPlanPurchaseRequestAmount { get; set; }
    public double AfterPeriodPlanPurchaseOrderAmount { get; set; }
    public double AfterPeriodPlanCargoAmount { get; set; }
    public double PlanCargoAmount { get; set; }
    public double PlanConsumeAmount { get; set; }
    public double PlanProduceAmount { get; set; }
    public double PlanSaleOrderAmount { get; set; }
    public double PlanPurchaseOrderAmount { get; set; }
    public double PlanPurchaseRequestAmount { get; set; }
    public double BufferRemainingAmount { get; set; }
  }
}