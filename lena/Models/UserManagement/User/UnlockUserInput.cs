using lena.Domains.Enums;
namespace lena.Models.UserManagement.User
{
  public class UnlockUserInput
  {
    public byte[] RowVersion { get; set; }
    public int UserId { get; set; }
  }
}
