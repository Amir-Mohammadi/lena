using lena.Models.QualityAssurance;

using lena.Domains.Enums;
namespace lena.Models
{
  public class IndicatorRejectedPurchaseQtyPercentageResult : IIndicatorResult
  {

    public double Amount { get; set; }

    public double SumQualityControlPassedQty { get; set; }
    public double SumQualityControlRejectedQty { get; set; }
    public double TotalQualityControlQty { get; set; }

  }
}
