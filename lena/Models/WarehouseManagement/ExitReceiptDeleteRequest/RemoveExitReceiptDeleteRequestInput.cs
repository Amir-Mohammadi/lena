using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ExitReceipt
{
  public class RemoveExitReceiptDeleteRequestInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
