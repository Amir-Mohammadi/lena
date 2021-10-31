using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequestSteps
{
  public class AddPurchaseRequestStepInput
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool AllowUploadDocument { get; set; }
  }
}
