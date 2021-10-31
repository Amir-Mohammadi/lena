using lena.Models.WarehouseManagement.ExitReceiptDeleteRequestStuffSerial;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ExitReceiptDeleteRequestInput
{
  public class AddExitReceiptDeleteRequestInput
  {
    public int ExitReceiptId { get; set; }
    public string Description { get; set; }
    public AddStuffSerialToDeleteExitReceipt[] StuffSerials { get; set; }
  }
}
