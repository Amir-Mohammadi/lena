using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrderGroup
{
  public class EditPurchaseOrderGroupInput
  {
    public PurchaseOrderGroupItemInput[] AddedPurchaseOrderGroupItems { get; set; }
    public TValue<string> Description { get; set; }
    public int Id { get; set; }
    public PurchaseOrderGroupItemInput[] RemovedPurchaseOrderGroupItems { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
