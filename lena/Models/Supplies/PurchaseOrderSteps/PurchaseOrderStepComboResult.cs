using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrderSteps
{
  public class PurchaseOrderStepComboResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool AllowUploadDocument { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
