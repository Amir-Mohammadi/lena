using lena.Domains.Enums;
namespace lena.Models.UserManagement.User
{
  public class AddUserInput
  {
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string PasswordConfimation { get; set; }
    public int? EmployeeId { get; set; }
  }
}
