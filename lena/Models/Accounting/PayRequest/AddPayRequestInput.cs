using lena.Domains.Enums;
namespace lena.Models.Accounting.PayRequest
{
  public class AddPayRequestInput
  {
    public int QualityControlId { get; set; }
    public string Description { get; set; }
    public double? DiscountedTotalPrice { get; set; }
  }
}
