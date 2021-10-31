using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.Receipt
{
  public class EditReceiptInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int CooperatorId { get; set; }
    public string Description { get; set; }
    public AddReceiptItemInput[] AddStoreReceipts { get; set; }
    public DeleteReceiptItemInput[] DeleteStoreReceipts { get; set; }

  }
}
