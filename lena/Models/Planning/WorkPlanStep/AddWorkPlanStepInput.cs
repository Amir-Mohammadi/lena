using lena.Models.Planning.OperationSequence;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkPlanStep
{
  public class AddWorkPlanStepInput
  {
    public string Title { get; set; }
    public long InitialTime { get; set; }
    public long SwitchTime { get; set; }
    public long BatchTime { get; set; }
    public double BatchCount { get; set; }
    public int ProductionLineId { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool NeedToQualityControl { get; set; }
    public bool PlanningWithoutMachineLimit { get; set; }
    public AddOperationSequenceInput[] OperationSequenceInputs { get; set; }
    public short ProductWarehouseId { get; set; }
    public short ConsumeWarehouseId { get; set; }
  }
}
