using System.Collections.Generic;
using lena.Models.QualityAssurance;

using lena.Domains.Enums;
namespace lena.Models
{
  public class IntervalBetweenCargoItemAndLadingItemIndicatorResult : IIndicatorResult
  {
    public double Amount { get; set; }

    public double WeightDay { get; set; }
    public int DelayDay { get; set; }
    public double? Percentage { get; set; }
    public int LadingItemCount { get; set; }
    public IEnumerable<int> LadingItemIds { get; set; }
    public double? TotalPercentage { get; set; }
    public bool TimelyCheck { get; set; } // وضعیت بررسی به موقع یا عدم بررسی به موقع

  }
}
