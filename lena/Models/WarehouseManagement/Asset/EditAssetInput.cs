using lena.Domains.Enums;
using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.Asset
{
  public class EditAssetInput
  {
    public int Id { get; set; }
    public int? EmployeeId { get; set; }
    public short? DepartmentId { get; set; }
    public string Description { get; set; }
    public AssetStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
