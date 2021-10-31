using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControl
{
  public class AddQualityControlPaymentSuggestInput
  {
    public int QualityControlId { get; set; }
    public QualityControlPaymentSuggestStatus? QualityControlPaymentSuggestStatus { get; set; }
  }
}
