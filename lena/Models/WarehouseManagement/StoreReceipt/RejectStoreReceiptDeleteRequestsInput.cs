using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceiptDeleteRequestInput
{
  public class RejectStoreReceiptDeleteRequestInput
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }

  }
}