using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionSchedule
{
  public class ProductionScheduleResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTime DateTime { get; set; }
    public long Duration { get; set; }
    public int ProductionPlanDetailId { get; set; }
    public int ProductionPlanId { get; set; }
    public byte[] ProductionPlanRowVersion { get; set; }
    public string ProductionPlanCode { get; set; }
    public int WorkPlanStepId { get; set; }
    public int ProductionLineId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double ProducedQty { get; set; }
    public int ProducedUnitId { get; set; }
    public string ProducedUnitName { get; set; }
    public bool ApplySwitchTime { get; set; }
    public int SwitchTime { get; set; }
    public int ProductionStepId { get; set; }
    public string ProductionLineName { get; set; }
    public string ProductionStepName { get; set; }
    public DateTime ToDateTime => DateTime.AddSeconds(Duration);
    public string SemiProductStuffName { get; set; }
    public string SemiProductStuffCode { get; set; }
    public int SemiProductStuffId { get; set; }
    public bool IsPublished { get; set; }
    public ProductionScheduleStatus Status { get; set; }
    public int OperatorCount { get; set; }
    public bool PlanningWithoutMachineLimit { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
