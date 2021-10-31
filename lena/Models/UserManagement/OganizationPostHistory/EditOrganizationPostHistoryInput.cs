using lena.Domains.Enums;
namespace lena.Models.UserManagement.OganizationPostHistory
{
  public class EditOrganizationPostHistoryInput : AddOrganizationPostHistoryInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
