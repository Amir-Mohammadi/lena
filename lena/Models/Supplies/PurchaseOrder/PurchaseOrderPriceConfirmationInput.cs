using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrder
{
  public class PurchaseOrderPriceConfirmationInput
  {
    public int PurchaseOrderId { get; set; }
    public ConfirmationStatus Status { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
