using lena.Models.QualityControl.ConditionalQualityControlItem;
using lena.Domains.Enums;
namespace lena.Models.QualityControl.ConditionalQualityControl
{
  public class AddConditionalQualityControlInput
  {
    public int QualityControlAccepterId;
    public int QualityControlConfirmationId;
    public AddConditionalQualityControlItemInput[] ConditionalQualityControlItems;
    public string Description { get; set; }
  }
}
