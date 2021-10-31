using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.SoftwareWorkReport
{
  public class SoftwareWorkReportResult
  {
    public int Id { get; set; }
    public DateTime ReportDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
