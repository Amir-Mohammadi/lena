// Auto generted file
using System;
using lena.Domains.Enums;
namespace lena.Models
{
  public class EmployeeResult
  {
    public int Id { get; set; }

    public string EmployeeCode { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalCode { get; set; }
    public string FatherName { get; set; }
    public string BirthPlace { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? EmployeementDate { get; set; }
    public int? DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public bool IsActive { get; set; }
    public int? OrganizationPostId { get; set; }
    public string OrganizationPostName { get; set; }
    public int? OrganizationJobId { get; set; }
    public string OrganizationJobName { get; set; }
    public byte[] Image { get; set; }
    public byte[] File { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
