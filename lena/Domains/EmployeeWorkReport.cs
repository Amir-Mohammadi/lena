using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EmployeeWorkReport : IEntity
  {
    protected internal EmployeeWorkReport()
    {
      this.EmployeeWorkReportItems = new HashSet<EmployeeWorkReportItem>();
    }
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EmployeeId { get; set; }
    public Nullable<int> ProjectERPTaskId { get; set; }
    public DateTime ReportDateTime { get; set; }
    public DateTime DateTime { get; set; } // تاریخ ثبت
    public byte[] RowVersion { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual User User { get; set; }
    public virtual ProjectERPTask ProjectERPTask { get; set; }
    public virtual ICollection<EmployeeWorkReportItem> EmployeeWorkReportItems { get; set; }
  }
}
