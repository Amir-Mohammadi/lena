using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemConfirmation
{
  public class AddOrderItemConfirmationInput
  {
    public int OrderItemId { get; set; }
    public byte[] OrderItemRowVersion { get; set; }
    public bool Confirmed { get; set; }
    public string Description { get; set; }
  }
}
