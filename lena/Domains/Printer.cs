using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Printer : IEntity
  {
    protected internal Printer()
    {
      this.PrinterSettings = new HashSet<ReportPrintSetting>();
    }
    public int Id { get; set; }
    public string NameInSystem { get; set; }
    public string NetworkAddress { get; set; }
    public string Manufacture { get; set; }
    public string Model { get; set; }
    public string Logo { get; set; }
    public bool IsColored { get; set; }
    public PrinterType PrinterType { get; set; }
    public string Location { get; set; }
    public bool SupportLan { get; set; }
    public string ModuleName { get; set; }
    public bool IsActive { get; set; }
    public bool IsSupportTemplatePrint { get; set; }
    public string Setting { get; set; }
    public DateTime CreationTime { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public int CreatorUserId { get; set; }
    public virtual ICollection<ReportPrintSetting> PrinterSettings { get; set; }
    public virtual User CreatorUser { get; set; }
  }
}
