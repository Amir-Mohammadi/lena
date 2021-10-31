using lena.Domains.Enums;
namespace lena.Models.UserManagement.Employee
{
  public class ActivateEmployeeInput
  {
    public int EmployeeId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
