using lena.Models.QualityAssurance;
using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models
{
  public class IntervalBetweenLadingItemAndNewShoppingIndicatorResult : IIndicatorResult
  {
    public double Amount { get; set; }

    public int DelayDay { get; set; }
    public double WeightDay { get; set; }
    public double? Percentage { get; set; }
    public int NewShoppingCount { get; set; }
    public IEnumerable<int> NewShoppingIds { get; set; }
    public double? TotalPercentage { get; set; }
    public bool TimelyCheck { get; set; } // وضعیت بررسی به موقع یا عدم بررسی به موقع

  }
}
