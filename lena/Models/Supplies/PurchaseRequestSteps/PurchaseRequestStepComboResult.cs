using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequestSteps
{
  public class PurchaseRequestStepComboResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool AllowUploadDocument { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
