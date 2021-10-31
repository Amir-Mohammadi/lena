using lena.Domains.Enums;
namespace lena.Models.UserManagement.Employee
{
  public class DeactivateEmployeeInput
  {
    public int EmployeeId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
