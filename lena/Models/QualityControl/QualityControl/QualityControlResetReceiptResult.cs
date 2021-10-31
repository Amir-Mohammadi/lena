using System;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControl
{
  public class QualityControlResetReceiptResult
  {
    public int StoreReceiptId { get; set; }
    public int StuffId { get; set; }
    public double Qty { get; set; }
    public DateTime DateTime { get; set; }
    public double ReturnOfSaleConversionRatio { get; set; }
    public double PayedAmount { get; set; }
  }
}
