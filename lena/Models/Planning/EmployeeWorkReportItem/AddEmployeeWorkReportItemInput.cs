using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.EmployeeWorkReportItem
{
  public class AddEmployeeWorkReportItemInput
  {
    public TimeSpan FromTime { get; set; }
    public TimeSpan ToTime { get; set; }
    public string Operation { get; set; }
    public string Description { get; set; }

  }
}
