using lena.Models.Planning.WorkPlanStep;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkPlan
{
  public class EditWorkPlanInput
  {
    public int Id { get; set; }
    public int BillOfMaterialStuffId { get; set; }
    public short BillOfMaterialVersion { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public string Title { get; set; }
    public int Version { get; set; }
    public AddWorkPlanStepInput[] AddWorkPlanStepInputs { get; set; }
    public EditWorkPlanStepInput[] EditWorkPlanStepInputs { get; set; }
    public int[] DeleteWorkPlanStepInputs { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
