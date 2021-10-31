using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrderSteps
{
  public class PurchaseOrderStepResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool AllowUploadDocument { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
