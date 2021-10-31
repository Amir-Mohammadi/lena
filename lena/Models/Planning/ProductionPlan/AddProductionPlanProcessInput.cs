using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionPlan
{
  public class AddProductionPlanProcessInput
  {
    public int ProductionRequestId { get; set; }
    public AddProductionPlanInput[] AddedProductionPlans { get; set; }
  }
}
