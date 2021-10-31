using lena.Models.QualityControl.QualityControlConfirmationItem;
using lena.Models.QualityControl.QualityControlConfirmationTest;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlConfirmation
{
  public class AddQualityControlConfirmationInput
  {
    public string Description { get; set; }
    public int QualityControlId { get; set; }
    public byte[] QualityControlRowVersion { get; set; }
    public bool Confirmed { get; set; }
    public bool IsEmergency { get; set; }
    public bool ApplayInAllItems { get; set; }
    public string FileKey { get; set; }
    public AddQualityControlConfirmationItemInput[] QualityControlConfirmationItems { get; set; }
    public AddQualityControlConfirmationTestInput[] QualityControlConfirmationTests { get; set; }
  }
}
