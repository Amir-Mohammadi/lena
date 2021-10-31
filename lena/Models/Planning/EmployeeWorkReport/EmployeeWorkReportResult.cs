using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class EmployeeWorkReportResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string RegistrantEmployeeFullName { get; set; }
    public double TotalEmployeeWorkReportDurationInSecond { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public string EmployeeFullName { get; set; }
    public int? DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public int? OrganizationPostId { get; set; }
    public string OrganizationPostTitle { get; set; }
    public DateTime ReportDateTime { get; set; }
    public DateTime DateTime { get; set; } // تاریخ ثبت
    public TimeSpan TotalEmployeeWorkReportDuration
    {
      get
      {
        return TimeSpan.FromSeconds(this.TotalEmployeeWorkReportDurationInSecond);
      }
    }
    public byte[] RowVersion { get; set; }
  }
}
