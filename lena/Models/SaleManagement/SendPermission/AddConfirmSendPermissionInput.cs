using lena.Domains.Enums;
namespace lena.Models.SaleManagement.SendPermission
{
  public class AddConfirmSendPermissionInput

  {
    public int SendPermissionId { get; set; }
    public byte[] RowVersion { get; set; }
    public bool Confirmed { get; set; }
    public string Description { get; set; }
  }
}
