using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class AddIranKhodroSerialInput
  {
    public int ProductionYearId { get; set; }
    public int CustomerStuffId { get; set; }
    public int CustomerStuffVersionId { get; set; }
    public string ManufacturerCode { get; set; }
    public DateTime? ProductionDateTime { get; set; }
    public string TechnicalNumber { get; set; }
    public int Qty { get; set; }
  }
}