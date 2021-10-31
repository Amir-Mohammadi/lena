using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.Receipt
{
  public class DeleteReceiptItemInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
