using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequestSteps
{
  public class EditPurchaseRequestStepInput : AddPurchaseRequestStepInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
