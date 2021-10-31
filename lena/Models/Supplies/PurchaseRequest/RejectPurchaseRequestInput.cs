using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequest
{
  public class RejectPurchaseRequestInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
  }
}
