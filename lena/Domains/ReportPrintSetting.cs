using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ReportPrintSetting : IEntity
  {
    protected internal ReportPrintSetting()
    {
    }
    public int Id { get; set; }
    public int ReportId { get; set; }
    public int PrinterId { get; set; }
    public DateTime CreationTime { get; set; }
    public byte[] RowVersion { get; set; }
    public int UserId { get; set; }
    public bool ShowPreview { get; set; }
    public bool ShowPrintDialog { get; set; }
    public int NumberOfCopies { get; set; }
    public virtual Report Report { get; set; }
    public virtual Printer Printer { get; set; }
    public virtual User User { get; set; }
  }
}