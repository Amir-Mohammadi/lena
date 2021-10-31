using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionSchedule
{
  public class AddProductionScheduleInput
  {
    public DateTime DateTime { get; set; }
    public long Duration { get; set; }
    public int ProductionPlanDetailId { get; set; }
    public int WorkPlanStepId { get; set; }
    public int SwitchTime { get; set; }
    public double Qty { get; set; }
    public bool ApplySwitchTime { get; set; }
    public int OperatorCount { get; set; }
    public bool PlanningWithoutMachineLimit { get; set; }
  }
}