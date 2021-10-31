using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrderGroup
{
  public class AddPurchaseOrderGroupInput
  {
    public string Description { get; set; }
    public PurchaseOrderGroupItemInput[] PurchaseOrderGroupItems { get; set; }
  }
}
