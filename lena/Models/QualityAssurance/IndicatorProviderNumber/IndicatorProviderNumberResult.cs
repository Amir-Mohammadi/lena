using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class IndicatorProviderNumberResult
  {
    public int StuffId { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public string ProviderName { get; set; }
    public Nullable<int> ProviderId { get; set; }
    public double RequestQty { get; set; }
    public int ProviderBuyCount { get; set; }
    public Nullable<int> ResponsibleEmployeeId { get; set; }
    public string ResponsibleEmployeeName { get; set; }
    public double Qty { get; set; }
    public double QtyPercentage { get; set; }
  }
}
