using System;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.User
{
  public class UserResult
  {
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }
    public int? EmployeeId { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string FullName { get; set; }
    public string EmployeeCode { get; set; }
    public int LoginFailedCount { get; set; }
    public DateTime LockOutDateTime { get; set; }
    public string Barcode { get; set; }
    public byte[] RowVersion { get; set; }
    public object Name { get; set; }
    public bool CanAccessFromInternet { get; set; }
  }
}
