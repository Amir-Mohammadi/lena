using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrderSteps
{
  public class ChangePurchaseOrderStepInput
  {
    public int PurchaseOrderId { get; set; }
    public int PurchaseOrderStepId { get; set; }
    public string Description { get; set; }
    public string FileKey { get; set; }
    public byte[] RowVersion { get; set; }
  }

}
