using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Report : IEntity
  {
    protected internal Report()
    {
      this.ReportVersions = new HashSet<ReportVersion>();
      this.PrinterSettings = new HashSet<ReportPrintSetting>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ReportVersion> ReportVersions { get; set; }
    public virtual ICollection<ReportPrintSetting> PrinterSettings { get; set; }
  }
}