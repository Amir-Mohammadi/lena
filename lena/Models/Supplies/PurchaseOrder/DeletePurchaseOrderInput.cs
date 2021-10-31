using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrder
{
  public class DeletePurchaseOrderInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
