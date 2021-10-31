using lena.Domains.Enums;
namespace lena.Models.Users
{
  public class EditProfileInput
  {
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
  }
}