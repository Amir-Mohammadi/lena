using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class SoftwareWorkReport : IEntity
  {
    public SoftwareWorkReport()
    {
      this.SoftwareWorkReportItems = new HashSet<SoftwareWorkReportItem>();
    }
    public int Id { get; set; }
    public DateTime ReportDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public int EmployeeId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual ICollection<SoftwareWorkReportItem> SoftwareWorkReportItems { get; set; }
  }
}