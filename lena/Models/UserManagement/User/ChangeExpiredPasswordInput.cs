using lena.Domains.Enums;
namespace lena.Models.UserManagement.User
{
  public class ChangeExpiredPasswordInput
  {
    public string UserName { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmNewPassword { get; set; }

  }
}
