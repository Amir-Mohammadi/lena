using lena.Models.Planning.WorkPlanStep;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkPlan
{
  public class AddWorkPlanInput
  {
    public int BillOfMaterialStuffId { get; set; }
    public short BillOfMaterialVersion { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
    public AddWorkPlanStepInput[] WorkPlanStepInputs { get; set; }
  }
}
