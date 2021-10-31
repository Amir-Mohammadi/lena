using lena.Models.Production.ProductionOrder;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialPublishRequest
{
  public class AcceptBillOfMaterialPublishRequestInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public EditProductionOrderWorkPlanStepInput[] ModifiedProductionOrders { get; set; }
  }
}
