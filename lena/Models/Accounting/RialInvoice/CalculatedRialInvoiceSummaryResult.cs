using lena.Domains.Enums;
namespace lena.Models.Accounting.RialInvoice
{
  public class CalculatedRialInvoiceSummaryResult
  {
    public double TotalGrossPriceSum { get; set; }
    public double TotalTransferCostSum { get; set; }
    public double TotalDutyCostSum { get; set; }
    public double TotalOtherCostSum { get; set; }
    public double TotalDiscountSum { get; set; }
    public double TotalNetPriceSum { get; set; }
  }
}
