using lena.Domains.Enums;
namespace lena.Models
{
  public class ChangePasswordInput
  {
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordConfirmation { get; set; }
  }
}
