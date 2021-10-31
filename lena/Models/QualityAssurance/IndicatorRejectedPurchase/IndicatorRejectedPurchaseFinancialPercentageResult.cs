using lena.Models.QualityAssurance;

using lena.Domains.Enums;
namespace lena.Models
{
  public class IndicatorRejectedPurchaseFinancialPercentageResult : IIndicatorResult
  {

    public double Amount { get; set; }
    public int CurrencyId { get; set; }

    public double SumQualityControlPassedPriceIRR { get; set; }
    public double SumQualityControlRejectedPriceIRR { get; set; }
    public double TotalQualityControlPriceIRR { get; set; }
  }
}
