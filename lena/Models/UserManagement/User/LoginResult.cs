using System;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.User
{
  public class LoginResult
  {
    public string UserName { get; set; }
    public int UserId { get; set; }
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
    public string UserEmployeeCode { get; set; }
    public int? UserEmployeeId { get; set; }
    public string UserFullName => UserFirstName + " " + UserLastName;
    public byte[] Image { get; set; }
    public short? DepartmentId { get; set; }

    public string RefreshToken { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresIn { get; set; }
  }
}
