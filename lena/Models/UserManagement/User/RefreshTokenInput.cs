using lena.Domains.Enums;
namespace lena.Models.UserManagement.User
{
  public class RefreshTokenInput
  {
    public string Token { get; set; }

    public string RefreshToken { get; set; }
  }
}
