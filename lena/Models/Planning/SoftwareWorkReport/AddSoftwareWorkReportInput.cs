using lena.Models.Planning.SoftwareWorkReportItem;
using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.SoftwareWorkReport
{
  public class AddSoftwareWorkReportInput
  {
    public int EmployeeId { get; set; }
    public DateTime ReportDateTime { get; set; }
    public AddSoftwareWorkReportItemInput[] AddSoftwareWorkReportItemInputs { get; set; }
  }
}
