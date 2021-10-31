using lena.Models.QualityControl.TestCondition;
using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlTest
{
  public class AddQualityControlTestInput
  {
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public AddTestConditionInput[] AddTestConditionInputs { get; set; }
    public AddTestEquipmentInput[] AddTestEquipmentInputs { get; set; }
    public AddTestImportanceDegreeInput[] AddTestImportanceDegreeInputs { get; set; }
    public AddTestOperationInput[] AddTestOperationInputs { get; set; }
  }
}
