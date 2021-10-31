using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionPlan
{
  public class EditProductionPlanProcessInput
  {
    public byte[] RowVersion { get; set; }
    public int PrductionRequestId { get; set; }
    public AddProductionPlanInput[] AddedProductionPlans { get; set; }
    public EditProductionPlanInput[] EditedProductionPlans { get; set; }
    public int[] DeletedProductionPlanIds { get; set; }
  }
}
