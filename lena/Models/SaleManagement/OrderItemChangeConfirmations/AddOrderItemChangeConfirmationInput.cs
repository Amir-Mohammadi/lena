using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemChangeConfirmations
{
  public class AddOrderItemChangeConfirmationInput
  {
    public string Description { get; set; }
    public bool Confirmed { get; set; }
    public int OrderItemChangeRequestId { get; set; }
    public OrderItemChangeStatus CurrentChangeStatus { get; set; }
    public byte[] OrderItemChangeRequestRowVersion { get; set; }
  }
}
