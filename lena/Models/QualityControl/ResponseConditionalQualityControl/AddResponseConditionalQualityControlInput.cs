using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.ResponseConditionalQualityControl
{
  public class AddResponseConditionalQualityControlInput
  {
    public int ConditionalQualityControlId { get; set; }
    public ConditionalQualityControlStatus ConditionalQualityControlStatus { get; set; }
    public byte[] ConditionalQualityControlRowVersoin { get; set; }
    public string Description { get; set; }
  }
}
