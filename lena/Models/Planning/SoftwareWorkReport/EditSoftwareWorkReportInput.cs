using lena.Models.Planning.SoftwareWorkReportItem;
using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.SoftwareWorkReport
{
  public class EditSoftwareWorkReportInput
  {
    public int Id { get; set; }
    public DateTime ReportDateTime { get; set; }
    public AddSoftwareWorkReportItemInput[] AddSoftwareWorkReportItemInputs { get; set; }
    public EditSoftwareWorkReportItemInput[] EditSoftwareWorkReportItemInputs { get; set; }
    public int[] DeletedIds { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
