using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EmployeeWorkReportItem : IEntity
  {
    protected internal EmployeeWorkReportItem()
    {
    }
    public int Id { get; set; }
    public int EmployeeWorkReportId { get; set; }
    public TimeSpan FromTime { get; set; }
    public TimeSpan ToTime { get; set; }
    public string Operation { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual EmployeeWorkReport EmployeeWorkReport { get; set; }
  }
}
