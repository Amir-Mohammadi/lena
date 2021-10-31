using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.EmployeeWorkReportItem
{
  public class EditEmployeeWorkReportItemInput
  {
    public int Id { get; set; }
    public TimeSpan FromTime { get; set; }
    public TimeSpan ToTime { get; set; }
    public string Operation { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
