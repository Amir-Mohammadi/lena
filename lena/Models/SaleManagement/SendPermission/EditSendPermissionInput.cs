using lena.Domains.Enums;
namespace lena.Models.SaleManagement.SendPermission
{
  public class EditSendPermissionInput : AddSendPermissionInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
