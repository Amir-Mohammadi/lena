using lena.Domains.Enums;
namespace lena.Models.UserManagement.User
{
  public class EditUserInput
  {
    public int Id { get; set; }
    public string UserName { get; set; }
    public int? EmployeeId { get; set; }

    public byte[] RowVersion { get; set; }
  }
}
