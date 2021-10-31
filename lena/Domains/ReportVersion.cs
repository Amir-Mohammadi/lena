using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ReportVersion : IEntity
  {
    protected internal ReportVersion()
    {
    }
    public int Id { get; set; }
    public string ApiUrl { get; set; }
    public string ReportContent { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreationTime { get; set; }
    public int CreatorUserId { get; set; }
    public byte[] RowVersion { get; set; }
    public int ReportId { get; set; }
    public string CultureName { get; set; }
    public bool IsForExport { get; set; }
    public Nullable<StimulExportFormat> ExportFormat { get; set; }
    public bool IsBarcodeTemplate { get; set; }
    public virtual User CreatorUser { get; set; }
    public virtual Report Report { get; set; }
  }
}