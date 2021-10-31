using System;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.ProductionFaultSeparationDayReport
{
  public class ProductionFaultSeparationDayResult
  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int ProductionFaultTypeId { get; set; }
    public string ProductionFaultTypeTitle { get; set; }
    public int ProductionFaultTypeCount { get; set; }
    public DateTime? DateTime { get; set; }

  }
}
