using lena.Models.QualityAssurance;
using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlDelayPercentageIndex
{
  public class QualityControlDelayPercentageIndexResult : IIndicatorResult
  {
    public double Amount { get; set; }
    public string DelayDay { get; set; }
    public IEnumerable<int> QualityControlIds { get; set; }
    public IEnumerable<int> QualityControlItemIds { get; set; }
    public int DelayDayInt { get; set; }
    public double Count { get; set; }
    public double Percentage { get; set; }
    public double TotalPercentage { get; set; }
    public double WeightDay { get; set; }
    public double AppliedWeight { get; set; }
    public double TotalWeight { get; set; }
    public bool IsOnTime { get; set; }
  }
}
