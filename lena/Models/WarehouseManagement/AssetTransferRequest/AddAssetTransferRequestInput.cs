using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement
{
  public class AddAssetTransferRequestInput
  {
    public int AssetId { get; set; }
    public int? NewEmployeeId { get; set; }
    public short? NewDepartmentId { get; set; }
    public string Description { get; set; }
  }
}
