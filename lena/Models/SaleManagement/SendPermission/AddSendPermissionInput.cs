using lena.Domains.Enums;
namespace lena.Models.SaleManagement.SendPermission
{
  public class AddSendPermissionInput
  {
    public int ExitReceiptRequestId { get; set; }
    public string Description { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
  }
}
