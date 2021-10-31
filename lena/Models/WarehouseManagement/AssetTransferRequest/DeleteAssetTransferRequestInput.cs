using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement
{
  public class DeleteAssetTransferRequestInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
