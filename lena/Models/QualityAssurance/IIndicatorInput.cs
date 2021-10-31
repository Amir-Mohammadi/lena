using System;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance
{
  public interface IIndicatorInput
  {
    DateTime FromDate { get; set; }
    DateTime ToDate { get; set; }
  }
}
