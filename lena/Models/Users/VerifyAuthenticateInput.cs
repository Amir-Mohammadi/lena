using lena.Domains.Enums;
namespace lena.Models.Users
{
  public class VerifyAuthenticateInput
  {
    public string VerificationCode { get; set; }
    public string VerificationToken { get; set; }
  }
}