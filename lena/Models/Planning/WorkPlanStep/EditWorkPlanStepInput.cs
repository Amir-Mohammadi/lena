using lena.Models.Planning.OperationSequence;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkPlanStep
{
  public class EditWorkPlanStepInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public long SwitchTime { get; set; }
    public long BatchTime { get; set; }
    public double BatchCount { get; set; }
    public int ProductionLineId { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool NeedToQualityControl { get; set; }
    public bool PlanningWithoutMachineLimit { get; set; }
    public AddOperationSequenceInput[] AddOperationSequenceInputs { get; set; }
    public EditOperationSequenceInput[] EditOperationSequenceInputs { get; set; }
    public int[] DeleteOperationSequenceInputs { get; set; }
    public byte[] RowVersion { get; set; }
    public short ProductWarehouseId { get; set; }
    public short ConsumeWarehouseId { get; set; }
  }
}
