using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequestSteps
{
  public class ChangePurchaseRequestStepInput
  {
    public int PurchaseRequestId { get; set; }
    public int PurchaseRequestStepId { get; set; }
    public string FileKey { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
