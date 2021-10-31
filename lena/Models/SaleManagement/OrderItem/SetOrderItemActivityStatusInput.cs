using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItem
{
  public class SetOrderItemActivityStatusInput
  {
    public int OrderItemId { get; set; }
    public byte[] OrderItemRowVersion { get; set; }
    public bool Activated { get; set; }

  }
}
