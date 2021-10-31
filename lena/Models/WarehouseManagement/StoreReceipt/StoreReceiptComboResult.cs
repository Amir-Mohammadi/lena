using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceipt
{
  public class StoreReceiptComboResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int? ReceiptId { get; set; }
    public string ReceiptCode { get; set; }
    public ReceiptStatus ReceiptStatus { get; set; }
  }
}
