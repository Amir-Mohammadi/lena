using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrderSteps
{
  public class EditPurchaseOrderStepInput : AddPurchaseOrderStepInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
