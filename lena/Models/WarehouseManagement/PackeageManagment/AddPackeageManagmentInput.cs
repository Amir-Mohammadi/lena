using lena.Models.WarehouseManagement.StuffSerial;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.PackeageManagment
{
  public class AddPackeageManagmentInput
  {
    public int StuffId { get; set; }
    public long StuffSerialCode { get; set; }
    public short BillOfMaterialVersion { get; set; }
    public StuffSerialPackage[] StuffSerialPackages { get; set; }
  }
}
