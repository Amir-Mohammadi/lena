using lena.Domains.Enums;
using lena.Models.QualityControl.QualityControlConfirmationTestItem;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlConfirmationTest
{
  public class AddQualityControlConfirmationTestInput
  {
    public int StuffId { get; set; }
    public long QualityControlTestId { get; set; }
    public int TestConditionId { get; set; } // اینارو تازه اضافه کردم
    public QualityControlConfirmationTestStatus Status { get; set; }
    public string Description { get; set; }
    public double AQLAmount { get; set; }
    public int QualityControlConfirmationId { get; set; }

    public AddQualityControlConfirmationTestItemInput[] AddQualityControlConfirmationTestItemsInput { get; set; }
  }
}
