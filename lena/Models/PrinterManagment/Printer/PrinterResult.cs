using System;
using lena.Domains.Enums;
namespace lena.Models.Printers
{
  public class PrinterResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string NameInSystem { get; set; }
    public string NetworkAddress { get; set; }
    public string Manufacture { get; set; }
    public string Model { get; set; }
    public string Logo { get; set; }
    public bool IsColored { get; set; }
    public lena.Domains.Enums.PrinterType PrinterType { get; set; }
    public string Location { get; set; }
    public bool SupportLan { get; set; }
    public string ModuleName { get; set; }
    public bool IsActive { get; set; }
    public bool IsSupportTemplatePrint { get; set; }
    public string Setting { get; set; }
    public int CreatorUserId { get; set; }
    public string CreatorUserName { get; set; }
    public DateTime CreationTime { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
  }
}
