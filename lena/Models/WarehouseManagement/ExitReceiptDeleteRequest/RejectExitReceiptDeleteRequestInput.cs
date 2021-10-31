using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ExitReceiptDeleteRequest
{
  public class RejectExitReceiptDeleteRequestInput
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
