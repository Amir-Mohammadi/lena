using lena.Models.Planning.EmployeeWorkReportItem;
using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class AddEmployeeWorkReportInput
  {
    public int EmployeeId { get; set; }
    public DateTime ReportDateTime { get; set; }
    public int? ProjectERPTaskId { get; set; }
    public AddEmployeeWorkReportItemInput[] AddEmployeeWorkReportItemInputs { get; set; }
  }
}
