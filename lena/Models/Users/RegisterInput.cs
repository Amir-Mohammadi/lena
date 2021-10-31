using lena.Domains.Enums;
namespace lena.Models.Users
{
  public class RegisterInput
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string RegisterToken { get; set; }
  }
}