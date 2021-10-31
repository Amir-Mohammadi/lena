using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionSchedule
{
  public class EditProductionScheduleInput
  {
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public long Duration { get; set; }
    public double? Qty { get; set; }
    public bool ApplySwitchTime { get; set; }
    public int SwitchTime { get; set; }
    public int OperatorCount { get; set; }
    public bool PlanningWithoutMachineLimit { get; set; }
    public int ProductionPlanDetailId { get; set; }
    public int WorkPlanStepId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
