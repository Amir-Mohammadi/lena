using lena.Domains.Enums;
namespace lena.Models
{
  public class ResetPasswordInput
  {
    public int UserId { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordConfirmation { get; set; }
  }
}
