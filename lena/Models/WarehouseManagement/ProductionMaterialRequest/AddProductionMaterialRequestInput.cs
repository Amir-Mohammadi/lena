using lena.Models.WarehouseManagement.StuffRequest;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ProductionMaterialRequest
{
  public class AddProductionMaterialRequestInput
  {
    public int[] ProductionOrderIds { get; set; }
    public AddStuffRequestInput[] AddStuffRequests { get; set; }
    public string Description { get; set; }
  }
}
