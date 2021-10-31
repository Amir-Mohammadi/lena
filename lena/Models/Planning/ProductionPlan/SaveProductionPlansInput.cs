using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionPlan
{
  public class SaveProductionPlansInput
  {
    public int ProductionRequestId { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
    public AddProductionPlanInput[] AddProductionPlans { get; set; }
    public EditProductionPlanInput[] EditProductionPlans { get; set; }
    public DeleteProductionPlanInput[] DeleteProductionPlans { get; set; }
  }
}
