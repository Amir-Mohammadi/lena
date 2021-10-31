using lena.Models.QualityControl.StuffQualityControlTestCondition;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.StuffQualityControlTest
{
  public class AddStuffQualityControlTestDocument
  {
    public long QualityControlTestId { get; set; }
    public string FileKey { get; set; }
    public AddStuffQualityControlTestConditionInput[] AddStuffQualityControlTestConditionInputs { get; set; }
    public AddStuffQualityControlTestEquipmentInput[] AddStuffQualityControlTestEquipmentInputs { get; set; }
    public AddStuffQualityControlTestImportanceDegreeInput[] AddStuffQualityControlTestImportanceDegreeInputs { get; set; }
    public AddStuffQualityControlTestOperationInput[] AddStuffQualityControlTestOperationInputs { get; set; }
  }
}
