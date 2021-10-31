using lena.Models.Planning.EmployeeWorkReportItem;
using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class EditEmployeeWorkReportInput
  {
    public int Id { get; set; }
    public DateTime ReportDateTime { get; set; }
    public int? ProjectERPTaskId { get; set; }
    public AddEmployeeWorkReportItemInput[] addEmployeeWorkReportItemInputs { get; set; }
    public EditEmployeeWorkReportItemInput[] editEmployeeWorkReportItemInputs { get; set; }
    public int[] DeletedIds { get; set; }
    public byte[] RowVersion { get; set; }
  }
}