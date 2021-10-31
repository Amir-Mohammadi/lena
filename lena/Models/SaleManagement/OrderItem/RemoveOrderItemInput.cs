using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItem
{
  public class RemoveOrderItemInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
