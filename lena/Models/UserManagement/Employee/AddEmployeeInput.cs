using System;
using lena.Domains.Enums;
namespace lena.Models
{
  public class AddEmployeeInput
  {
    public int? Code { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalCode { get; set; }
    public string FatherName { get; set; }
    public string BirthPlace { get; set; }
    public DateTime? BirthDate { get; set; }
    public short? DepartmentId { get; set; }
    public DateTime? EmployeementDate { get; set; }
    public string ImageKey { get; set; }
    public string FileKey { get; set; }
    public bool IsActive { get; set; }
    public int? OrganizationPostId { get; set; }
    public int? OrganizationJobId { get; set; }
  }
}
