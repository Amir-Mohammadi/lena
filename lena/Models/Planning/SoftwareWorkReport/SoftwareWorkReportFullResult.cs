using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Planning.SoftwareWorkReport
{
  public class SoftwareWorkReportFullResult
  {
    public int Id { get; set; }
    public int SoftwareWorkReportItemId { get; set; }
    public DateTime ReportDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public int Spent { get; set; }
    public int Estimated { get; set; }
    public string SpentTimeFormat { get; set; }
    public string EstimatedTimeFormat { get; set; }
    public string SpentGitFormat { get; set; }
    public string EstimatedGitFormat { get; set; }
    public string Issue { get; set; }
    public byte[] RowVersion { get; set; }
    public RestTimeStatus? RestTimeIssue { get; set; }
  }
}
