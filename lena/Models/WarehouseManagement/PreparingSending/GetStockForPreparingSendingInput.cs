using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.PreparingSending
{
  public class GetStockForPreparingSendingInput
  {
    public int SendPermissionId { get; set; }
    public string Serial { get; set; }
    public string[] SelectedSerial { get; set; }
  }
}
