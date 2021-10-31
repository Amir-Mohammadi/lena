using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequestStepDetail
{
  public class AddPurchaseRequestStepDetailInput
  {
    public int PurchaseRequestId { get; set; }
    public int PurchaseRequestStepId { get; set; }
    public string FileKey { get; set; }
    public string Description { get; set; }
    public byte[] PurchaseRequestRowVersion { get; set; }
  }
}
