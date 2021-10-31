using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItem
{
  public class AddOrderItemConfirmationRequestInput
  {
    public int OrderItemId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
