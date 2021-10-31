using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemBlock
{
  public class UnblockExitReceiptRequestInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
