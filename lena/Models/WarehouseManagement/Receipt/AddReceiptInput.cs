using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.Receipt
{
  public class AddReceiptInput
  {
    public int CooperatorId { get; set; }
    public string Description { get; set; }
    public AddReceiptItemInput[] StoreReceipts { get; set; }
    public DateTime ReceiptDateTime { get; set; }
  }
}
