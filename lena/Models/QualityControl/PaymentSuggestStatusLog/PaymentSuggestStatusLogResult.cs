using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.PaymentSuggestStatusLog
{
  public class PaymentSuggestStatusLogResult
  {
    public int Id { get; set; }
    public int QualityControlId { get; set; }
    public string QualityControlCode { get; set; }
    public string RegisterarEmployeeName { get; set; }
    public QualityControlPaymentSuggestStatus? QualityControlPaymentSuggestStatus { get; set; }
    public DateTime RegisterDateTime { get; set; }
    public string Description { get; set; }

  }
}
