

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.ExitReceiptRequest
{
  public class GetExitReceiptRequestsComboInput
  {

    public int? Id { get; set; }
    public int? CooperatorId { get; set; }
    public int? StuffId { get; set; }
    public int? OrderItemId { get; set; }
  }
}
