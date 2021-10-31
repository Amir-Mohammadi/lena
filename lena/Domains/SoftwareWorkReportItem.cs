using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class SoftwareWorkReportItem : IEntity
  {
    public int Id { get; set; }
    public int SoftwareWorkReportId { get; set; }
    public int Spent { get; set; }
    public int Estimated { get; set; }
    public string Issue { get; set; }
    public RestTimeStatus? RestTimeIssue { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual SoftwareWorkReport SoftwareWorkReport { get; set; }
  }
}