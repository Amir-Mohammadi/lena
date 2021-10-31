using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequest
{
  public class AssignResponsiblePurchaseRequestInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int? ResponsibleEmployeeId { get; set; }
  }
}
